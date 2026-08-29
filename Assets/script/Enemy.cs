using UnityEngine;
using System;

public class Enemy : Charactor
{
    public event Action<int, int> OnHPChanged;
    public event Action OnDeath;
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        OnHPChanged?.Invoke(CurrentHP, MaxHP);
    }
    protected override void Die()
    {
        OnDeath?.Invoke();
        gameObject.SetActive(false);
    }
}
