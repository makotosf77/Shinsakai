using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
       [Header("Characters")]
    [SerializeField] private Player player;
    [SerializeField] private Enemy enemy;

    [Header("UI")]
    [SerializeField]
    private CommandMenuController commandMenuController;

    [Header("Attack Mini Games")]
    [SerializeField]
    private ClickAttackMiniGame clickAttackMiniGame;

    [SerializeField]
    private TargetAttackMiniGame targetAttackMiniGame;
    [SerializeField]
    private HealMiniGame healMiniGame;
    [SerializeField]
    private EnemyAttackController enemyAttackController;
    [SerializeField]
    private BattleResultUI battleResultUI;


    public BattleState CurrentState
    {
        get;
        private set;
    }


    private void Awake()
    {
        commandMenuController.OnCommandSelected
            += OnCommandSelected;
    }
    private void Start()
    {
        StartBattle();
        player.OnDeath += GameOver;
    }


    public void StartBattle()
    {
        Debug.Log("戦闘開始！");

        StartPlayerTurn();
    }


    private void StartPlayerTurn()
    {
        if (player.IsDead)
        {
            GameOver();
            return;
        }

        CurrentState =
            BattleState.PlayerTurn;

        Debug.Log(
            "プレイヤーのターン"
        );

        commandMenuController.Show();
    }


    private void OnCommandSelected(
        CommandType command)
    {
        switch (command)
        {
            case CommandType.Fight:
                StartNormalAttack();
                break;

            case CommandType.Magic:
                StartMagicAttack();
                break;

            case CommandType.Item:
                OnItemSelected();
                break;
        }
    }


    private void StartNormalAttack()
    {
        if (CurrentState !=
            BattleState.PlayerTurn)
        {
            return;
        }

        CurrentState =
            BattleState.PlayerAttack;

        NormalAttack attack =
            new NormalAttack(
                clickAttackMiniGame
            );

        attack.Execute(
            player,
            enemy,
            OnAttackFinished
        );
    }

    private void StartMagicAttack()
    {
        if (CurrentState !=
            BattleState.PlayerTurn)
        {
            return;
        }

        CurrentState =
            BattleState.PlayerAttack;

        MagicAttack attack =
            new MagicAttack(
                targetAttackMiniGame
            );

        attack.Execute(
            player,
            enemy,
            OnAttackFinished
        );
    }


    public void OnAttackFinished(
        AttackResults result)
    {
        Debug.Log(
            $"攻撃ダメージ: {result.Damage}"
        );

        if (enemy.IsDead)
        {
            Victory();
            return;
        }

        StartEnemyTurn();
    }


    private void StartEnemyTurn()
    {
        CurrentState =
            BattleState.EnemyTurn;

        Debug.Log(
            "敵のターン"
        );
        enemyAttackController.StartAttack(FinishEnemyTurn);

        
    }


    public void FinishEnemyTurn()
    {
        if (player.IsDead)
        {
            GameOver();
            return;
        }

        StartPlayerTurn();
    }


    private void OnItemSelected()
    {
        if(player == null)
        {
            return;
        }
        if (player.CurrentHP >= player.MaxHP)
        {
            return;
        }
        healMiniGame.StartGame(OnHealFinished);
    }
    private void OnHealFinished(int healAmount)
    {
        player.Heal(healAmount);
        StartEnemyTurn();
    }
    private void Victory()
    {
        CurrentState =
            BattleState.Victory;

        commandMenuController.Hide();

        Debug.Log(
            "勝利！"
        );
        commandMenuController.Hide();
        battleResultUI.ShowWin();
    }


    private void GameOver()
    {
       if(CurrentState == BattleState.Defeat)
        {
            return;
        }
        CurrentState = BattleState.Defeat;

        commandMenuController.Hide();
        enemyAttackController.StopAttack();
        battleResultUI.ShowGameOver();
    }
    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnDeath -= GameOver;
        }
        if (commandMenuController != null)
        {
            commandMenuController.OnCommandSelected
                -= OnCommandSelected;
        }
    }
}