using UnityEngine;
using System;

public class Player : Charactor
{
    public event Action<int, int> OnHPChanged;
    public event Action OnDeath;
 
    public override void TakeDamage(int damage)
    {
        if(IsDead)
            { return; }
        base.TakeDamage(damage);
        OnHPChanged?.Invoke(CurrentHP, maxHP);
    }
    public override void Heal(int amount)
    {
        if(IsDead)
        {
            return;
        }
        int oldHP = CurrentHP;
        base.Heal(amount);
        int actualHeal = CurrentHP - oldHP;
        Debug.Log($"HPが回復");
        OnHPChanged?.Invoke(CurrentHP, maxHP);
    }
  protected override void Die()
    {
        OnDeath?.Invoke();
        Debug.Log("プレイヤーは死亡した。ゲームオーバー！");
        // ゲームオーバー処理をここに追加
    }

}
