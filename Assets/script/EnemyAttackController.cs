using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject enemyAttackPanel;

    [SerializeField] private RectTransform battleArea;

    [SerializeField] private RectTransform playerHeart;

    [SerializeField] private PlayerHeart playerHeartComponent;

    [SerializeField] private Bullet bulletPrefab;

    [SerializeField] private Transform bulletContainer;

    [SerializeField] private RainAttack rainAttack;
    [SerializeField]private CircleAttack circleAttack;
    [SerializeField]private SideAttack sideAttack;


    [Header("Normal Attack Settings")]
    [SerializeField] private float attackDuration = 5f;

    [SerializeField] private float bulletInterval = 0.7f;


    private bool isAttacking;

    private Coroutine normalAttackCoroutine;


    public void StartAttack(Action onFinished)
    {
        if (isAttacking)
        {
            return;
        }

        isAttacking = true;

        enemyAttackPanel.SetActive(true);

        // 0 または 1をランダムで選ぶ
        int attackType =
            UnityEngine.Random.Range(0, 4);

        switch (attackType)
        {
            case 0:

                Debug.Log(
                    "通常攻撃！"
                );

                normalAttackCoroutine =
                    StartCoroutine(
                        NormalAttackCoroutine(
                            onFinished
                        )
                    );

                break;

            case 1:

                Debug.Log(
                    "RainAttack！"
                );

                rainAttack.StartAttack(
                    () =>
                    {
                        FinishAttack(
                            onFinished
                        );
                    }
                );
                break;
            case 2:
                circleAttack.StartAttack(
                    () =>
                    {
                        FinishAttack(
                            onFinished
                        );
                    }
                );

                break;
            case 3:
                sideAttack. StartAttack(
                    () => 
                    {
                    FinishAttack(
                        onFinished
                        );
                    }); break; 
        }
    }


    private IEnumerator NormalAttackCoroutine(
        Action onFinished)
    {
        float elapsedTime = 0f;

        while (elapsedTime < attackDuration)
        {
            ShootBullet();

            yield return new WaitForSeconds(
                bulletInterval
            );

            elapsedTime += bulletInterval;
        }

        FinishAttack(onFinished);
    }


    private void FinishAttack(
        Action onFinished)
    {
        ClearBullets();

        enemyAttackPanel.SetActive(false);

        isAttacking = false;

        onFinished?.Invoke();
    }


    private void ShootBullet()
    {
        Vector2 spawnPosition =
            GetRandomSpawnPosition();

        Bullet bullet =
            Instantiate(
                bulletPrefab,
                bulletContainer
            );

        RectTransform bulletRect =
            bullet.GetComponent<RectTransform>();

        bulletRect.anchoredPosition =
            spawnPosition;

        Vector2 direction =
            playerHeart.anchoredPosition -
            spawnPosition;

        bullet.Initialize(
            direction,
            playerHeartComponent
        );
    }


    private Vector2 GetRandomSpawnPosition()
    {
        Rect areaRect =
            battleArea.rect;

        float halfWidth =
            areaRect.width / 2f;

        float halfHeight =
            areaRect.height / 2f;

        int side =
            UnityEngine.Random.Range(
                0,
                4
            );

        switch (side)
        {
            case 0:

                return new Vector2(
                    UnityEngine.Random.Range(
                        -halfWidth,
                        halfWidth
                    ),
                    halfHeight
                );

            case 1:

                return new Vector2(
                    UnityEngine.Random.Range(
                        -halfWidth,
                        halfWidth
                    ),
                    -halfHeight
                );

            case 2:

                return new Vector2(
                    -halfWidth,
                    UnityEngine.Random.Range(
                        -halfHeight,
                        halfHeight
                    )
                );

            default:

                return new Vector2(
                    halfWidth,
                    UnityEngine.Random.Range(
                        -halfHeight,
                        halfHeight
                    )
                );
        }
    }


    public void StopAttack()
    {
        if (normalAttackCoroutine != null)
        {
            StopCoroutine(
                normalAttackCoroutine
            );

            normalAttackCoroutine = null;
        }

        if (rainAttack != null)
        {
            rainAttack.StopAttack();
        }

        ClearBullets();

        enemyAttackPanel.SetActive(false);

        isAttacking = false;
        if(circleAttack!= null)
        {
            circleAttack.StopAttack();
        }
        ClearBullets();
        enemyAttackPanel.SetActive(false);
        isAttacking = false;

        if (circleAttack != null)
        {
            circleAttack.StopAttack();
        }
        ClearBullets();
        enemyAttackPanel.SetActive(false);
        isAttacking =false;
    }


    private void ClearBullets()
    {
        if (bulletContainer == null)
        {
            return;
        }

        foreach (
            Transform child
            in bulletContainer
        )
        {
            Destroy(
                child.gameObject
            );
        }
    }
}