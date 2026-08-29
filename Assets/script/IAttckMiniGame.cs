using System;

public interface IAttckMiniGame
{
    void StartGame(Action<AttackResults> onFinished);    
}