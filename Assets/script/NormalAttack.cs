using System;
using UnityEngine;

public class NormalAttack :MonoBehaviour
{
    private readonly IAttckMiniGame miniGame;
    public NormalAttack(IAttckMiniGame miniGame)
    {
        this.miniGame = miniGame;
    }
    public void Execute(Player attacker, Enemy target,System.Action<AttackResults> onFinished)
    {
        miniGame.StartGame((result) =>
        {
            target.TakeDamage(result.Damage);
            onFinished?.Invoke(result);
        });
    }
}
