using System;
using System.Collections;
using UnityEngine;

public class CircleAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform battleArea;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform bulletContainer;
    [SerializeField] private PlayerHeart playerHeart;

    [Header("Attack Settings")]
    [SerializeField] private float attackDuration = 5f;
    [SerializeField] private float shotInterval = 0.8f;
    [SerializeField] private int bulletsPerShot = 8;

    // BattleAreaの外側にどれくらい離して生成するか
    [SerializeField] private float spawnRadius = 350f;

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
            ShootCircle();

            yield return new WaitForSeconds(
                shotInterval
            );

            elapsedTime += shotInterval;
        }

        attackCoroutine = null;

        ClearBullets();

        onFinished?.Invoke();
    }


    private void ShootCircle()
    {
        // BattleAreaの中央
        Vector2 centerPosition = Vector2.zero;

        float angleStep =
            360f / bulletsPerShot;


        for (int i = 0; i < bulletsPerShot; i++)
        {
            float angle =
                angleStep * i;

            float radian =
                angle * Mathf.Deg2Rad;


            // 円周上の方向
            Vector2 circleDirection =
                new Vector2(
                    Mathf.Cos(radian),
                    Mathf.Sin(radian)
                );


            // 外側の位置から生成
            Vector2 spawnPosition =
                centerPosition +
                circleDirection * spawnRadius;


            // 外側 → 中央へ向かう方向
            Vector2 moveDirection =
                centerPosition -
                spawnPosition;


            SpawnBullet(
                spawnPosition,
                moveDirection
            );
        }
    }


    private void SpawnBullet(
        Vector2 spawnPosition,
        Vector2 direction)
    {
        Bullet bullet =
            Instantiate(
                bulletPrefab,
                bulletContainer
            );

        RectTransform bulletRect =
            bullet.GetComponent<RectTransform>();

        bulletRect.anchoredPosition =
            spawnPosition;


        bullet.Initialize(
            direction,
            playerHeart
        );
    }


    private void ClearBullets()
    {
        if (bulletContainer == null)
        {
            return;
        }

        foreach (Transform child in bulletContainer)
        {
            Destroy(child.gameObject);
        }
    }
}