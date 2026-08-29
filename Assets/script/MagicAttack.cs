using System;

public class MagicAttack
{
    private readonly IAttckMiniGame miniGame;
    public MagicAttack(IAttckMiniGame miniGame)
    {
        this.miniGame = miniGame;
    }
    public void Execute(Player attacker, Enemy target, Action<AttackResults> onFinished)
    {
        miniGame.StartGame((result) =>
        {
            target.TakeDamage(result.Damage);
            onFinished?.Invoke(result);
        });
    }
}
