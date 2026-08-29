using System;
using System.Collections;
using UnityEngine;

public class RainAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform battleArea;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform bulletContainer;
    [SerializeField] private PlayerHeart playerHeart;

    [Header("Attack Settings")]
    [SerializeField] private float attackDuration = 5f;
    [SerializeField] private float bulletInterval = 0.3f;

    private Coroutine attackCoroutine;


    public void StartAttack(Action onFinished)
    {
        if (attackCoroutine != null)
        {
            return;
        }

        attackCoroutine =
            StartCoroutine(
                AttackRoutine(onFinished)
            );
    }


    public void StopAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);

            attackCoroutine = null;
        }

        ClearBullets();
    }


    private IEnumerator AttackRoutine(
        Action onFinished)
    {
        float elapsedTime = 0f;

        while (elapsedTime < attackDuration)
        {
            SpawnBullet();

            yield return new WaitForSeconds(
                bulletInterval
            );

            elapsedTime += bulletInterval;
        }

        attackCoroutine = null;

        ClearBullets();

        onFinished?.Invoke();
    }


    private void SpawnBullet()
    {
        Rect areaRect = battleArea.rect;

        float randomX =
            UnityEngine.Random.Range(
                areaRect.xMin,
                areaRect.xMax
            );

        float spawnY =
            areaRect.yMax + 30f;

        Vector2 spawnPosition =
            new Vector2(
                randomX,
                spawnY
            );

        Bullet bullet =
            Instantiate(
                bulletPrefab,
                bulletContainer
            );

        RectTransform bulletRect =
            bullet.GetComponent<RectTransform>();

        bulletRect.anchoredPosition =
            spawnPosition;

        // 真下に移動
        bullet.Initialize(
            Vector2.down,
            playerHeart
        );
    }


    private void ClearBullets()
    {
        foreach (Transform child in bulletContainer)
        {
            Destroy(child.gameObject);
        }
    }
}