using UnityEngine;
using System;
using System.Collections;

public class SideAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform battleArea;
    [SerializeField] private Bullet BulletPrefab;
    [SerializeField] private Transform bulletContainer;
    [SerializeField] private PlayerHeart playerHearrt;

    [Header("Attack Setting")]
    [SerializeField] private float attackDuration = 5f;
    [SerializeField] private float bulletInterval = 0.5f;

    [SerializeField] private float spawnOffset = 30f;

    private Coroutine attackCoroutine;

    public void StartAttack(Action onFinished)
    {
        if(attackCoroutine != null)
        {
            return;
        }
        attackCoroutine = StartCoroutine(AttackRoutine(onFinished));
    }
    public void StopAttack()
    {
        if(attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
        ClearBullets();
    }
    private IEnumerator AttackRoutine(Action onFinished)
    {
        float elapsedTime = 0f;
        while (elapsedTime < attackDuration)
        {
            ShootFromBothSides();
            yield return new WaitForSeconds(bulletInterval);
            elapsedTime += bulletInterval;
        }
        attackCoroutine = null;
        ClearBullets();
        onFinished?.Invoke();
    }
    private void ShootFromBothSides()
    {
        Rect areaRect = battleArea.rect;
        float randomY = UnityEngine.Random.Range(areaRect.yMin, areaRect.yMax);
        Vector2 leftSpwanPosition = new Vector2(areaRect.xMin -spawnOffset, randomY);
        SpwanBullet(leftSpwanPosition, Vector2.right);
        float randomYRight = UnityEngine.Random.Range(areaRect.yMin, areaRect.yMax);
        Vector2 rightSpwanPosition = new Vector2(areaRect.xMax + spawnOffset, randomYRight);
        SpwanBullet(rightSpwanPosition, Vector2.left);
    }
    private void SpwanBullet(Vector2 spwanPosition, Vector2 direction)
    {
        Bullet bullet = Instantiate(BulletPrefab, bulletContainer);
        RectTransform bulletRect = bullet.GetComponent<RectTransform>();
        bulletRect.anchoredPosition= spwanPosition;
        bullet.Initialize(direction, playerHearrt);
        
    }
    private void ClearBullets()
    {
        if(bulletContainer == null)
        {
            return;
        }
        foreach(Transform child in bulletContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
