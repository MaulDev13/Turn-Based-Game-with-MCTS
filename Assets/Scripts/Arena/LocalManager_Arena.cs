using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalManager_Arena : MonoBehaviour
{
    #region Singleton
    public static LocalManager_Arena instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }
    #endregion

    [Header("Sync")]
    public Transform playerHolder;
    public Transform enemyHolder;

    public GameObject unitPrefabs;

    public float enemyDelayAct = 0.5f;

    public bool isHideActBtnEnemy = true;
    [HideInInspector] public bool isEnd = false;

    [Header("MCTS")]
    [SerializeField] public MC_BattleManager mc_manager;
    [SerializeField] public int maxTurnOnSimulation = 100;
    [SerializeField] public int loopOnSimulation = 100;

    [Header("Inspector")]
    [SerializeField] public BattleUnit playerUnit;
    [SerializeField] public BattleUnit enemyUnit;

    [HideInInspector] public BattleUnit unitTurn;

    public enum BattleState
    {
        Preperation, PlayerTurn, EnemyTurn, Win, Defeat
    }

    [SerializeField] private BattleState state;
    public BattleState State => state;

    private int currentTurn;
    public int CurrentTurn => currentTurn;

    private void Start()
    {
        BattleStateManager(BattleState.Preperation);
    }

    private void BattleStateManager(BattleState value)
    {
        if (state == BattleState.Win || state == BattleState.Defeat)
            return;

        currentTurn++;

        state = value;
        LocalManager_ArenaUI.instance.TurnInfo();

        switch (state)
        {
            case BattleState.Preperation:
                Preperation();
                break;
            case BattleState.PlayerTurn:
                unitTurn = playerUnit;
                
                LocalManager_ArenaUI.instance.SetActionBtn();

                unitTurn.Turn();
                break;
            case BattleState.EnemyTurn:
                unitTurn = enemyUnit;

                if (isHideActBtnEnemy)
                    LocalManager_ArenaUI.instance.SetActionBtn();

                unitTurn.Turn();
                break;
            case BattleState.Win:
                // Win the battle
                //Debug.Log("You Win!");

                //ArenaGameManager.instance.BattleResult(true);
                break;
            case BattleState.Defeat:
                // Lose the battle
                //Debug.Log("You Lose!");

                //ArenaGameManager.instance.BattleResult(false);
                break;
        }
    }

    private void Preperation()
    {
        currentTurn = 0;

        // Initiation player unit
        var playerNewUnit = Instantiate(unitPrefabs, playerHolder) as GameObject;
        var playerBattleScript = playerNewUnit.GetComponent<BattleUnit>();
        playerBattleScript.isPlayerUnit = true;

        playerBattleScript.Init(ArenaGameManager.instance.playerUnitBase, true);

        playerUnit = playerBattleScript;

        var holderScriptPlayer = playerHolder.GetComponent<UnitHolder>();
        holderScriptPlayer.Init(playerBattleScript, true);

        // Initiation enemy unit
        var enemyNewUnit = Instantiate(unitPrefabs, enemyHolder) as GameObject;
        var enemyBattleScript = enemyNewUnit.GetComponent<BattleUnit>();
        enemyBattleScript.isPlayerUnit = false;
        enemyBattleScript.Init(ArenaGameManager.instance.enemyUnitBase, false);

        enemyUnit = enemyBattleScript;

        var holderScript = enemyHolder.GetComponent<UnitHolder>();
        holderScript.Init(enemyBattleScript, true);

        // Check who is enemy
        playerUnit.myEnemy = enemyUnit;
        enemyUnit.myEnemy = playerUnit;

        LocalManager_ArenaUI.instance.SetUnitInfoPanel();

        if (ArenaGameManager.instance.isPlayerFirstTurn)
            BattleStateManager(BattleState.PlayerTurn);
        else
            BattleStateManager(BattleState.EnemyTurn);
    }

    public void EndBattle(bool _isPlayerWin)
    {
        if(_isPlayerWin)
            BattleStateManager(BattleState.Win);
        else
            BattleStateManager(BattleState.Defeat);
    }

    public void EndTurn()
    {
        if (state == BattleState.Defeat || state == BattleState.Win)
            return;

        LocalManager_ArenaUI.instance.ClearActionButton();

        if (unitTurn.GetName() == playerUnit.GetName())
            BattleStateManager(BattleState.EnemyTurn);
        else
            BattleStateManager(BattleState.PlayerTurn);
    }

    public void Forfeit()
    {
        BattleStateManager(BattleState.Defeat);
    }
}