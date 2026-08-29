using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 300f;

    [Header("Damage")]
    [SerializeField] private int damage = 10;

    [Header("Lifetime")]
    [SerializeField] private float lifeTime = 5f;

    [Header("Hit Settings")]
    [SerializeField] private float hitDistance = 30f;

    private Vector2 direction;

    private RectTransform rectTransform;
    private PlayerHeart playerHeart;

    private bool hasHit;


    private void Awake()
    {
        rectTransform =
            GetComponent<RectTransform>();
    }


    private void Start()
    {
        // 一定時間後に弾を消す
        Destroy(
            gameObject,
            lifeTime
        );
    }


    private void Update()
    {
        if (hasHit)
        {
            return;
        }

        Move();
        CheckHit();
    }


    // =========================================
    // プレイヤーを狙う弾
    // EnemyAttackControllerから使用
    // =========================================

    public void Initialize(
        Vector2 moveDirection,
        PlayerHeart targetHeart)
    {
        direction =
            moveDirection.normalized;

        playerHeart =
            targetHeart;
    }


    // =========================================
    // 指定方向に飛ぶ弾
    // RainAttackなどから使用
    // =========================================

    public void Initialize(
        Vector2 moveDirection)
    {
        direction =
            moveDirection.normalized;
    }


    // =========================================
    // 弾を移動させる
    // =========================================

    private void Move()
    {
        if (rectTransform == null)
        {
            return;
        }

        rectTransform.anchoredPosition +=
            direction *
            moveSpeed *
            Time.deltaTime;
    }


    // =========================================
    // プレイヤーとの当たり判定
    // =========================================

    private void CheckHit()
    {
        if (playerHeart == null)
        {
            return;
        }

        RectTransform heartRect =
            playerHeart.GetComponent<RectTransform>();

        if (heartRect == null)
        {
            return;
        }

        float distance =
            Vector2.Distance(
                rectTransform.anchoredPosition,
                heartRect.anchoredPosition
            );

        if (distance <= hitDistance)
        {
            HitPlayer();
        }
    }


    // =========================================
    // プレイヤーに命中
    // =========================================

    private void HitPlayer()
    {
        if (hasHit)
        {
            return;
        }

        hasHit = true;

        Debug.Log(
            "弾がプレイヤーに命中！"
        );

        playerHeart.TakeDamage(
            damage
        );

        Destroy(gameObject);
    }
}