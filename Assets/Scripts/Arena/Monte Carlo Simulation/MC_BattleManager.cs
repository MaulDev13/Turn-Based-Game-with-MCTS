using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MC_BattleManager : MonoBehaviour
{
    [Header("Inspector")]
    [HideInInspector] public bool isEnd = false;
    [HideInInspector] public bool isP1Win = false;
    [SerializeField] public MC_BattleUnit playerUnit1; // playerUnit1 is the one who use MCTS. Always move first
    [SerializeField] public MC_BattleUnit playerUnit2;
    [HideInInspector] public MC_BattleUnit unitTurn;

    private string p1Name = "P1";
    //private string p2Name = "P2";

    int turnAvaliable = 0;
    int currentTurn;

    enum BattleState
    {
        P1Turn, P2Turn, Result
    }
    private BattleState state;

    public bool Init(Unit _user, Unit _enemy, int _turnAvaliable, int _firstSkillIndex)
    {
        turnAvaliable = _turnAvaliable;
        currentTurn = 0;

        // Initiation player unit
        playerUnit1.Init(true, _firstSkillIndex, _user, this);
        playerUnit1.name = p1Name;

        // Initiation enemy unit
        playerUnit2.Init(false, 0, _enemy, this);

        // Check who is enemy
        playerUnit1.myEnemy = playerUnit2;
        playerUnit2.myEnemy = playerUnit1;

        state = BattleState.P1Turn;
        StartCoroutine(BattleStateManager(BattleState.P1Turn));

        return isP1Win;
    }

    IEnumerator BattleStateManager(BattleState value)
    {
        yield return new WaitForEndOfFrame();

        if (state == BattleState.Result)
            yield return null;

        currentTurn++;
        if (currentTurn >= turnAvaliable && value != BattleState.Result)
            value = BattleState.Result;

        state = value;

        switch (state)
        {
            case BattleState.P1Turn:
                unitTurn = playerUnit1;
                unitTurn.Turn();
                break;
            case BattleState.P2Turn:
                unitTurn = playerUnit2;
                unitTurn.Turn();
                break;
            case BattleState.Result:
                // Give a feedback
                // If draw than it a lose
                // ???
                break;
        }
    }

    public void EndBattle(bool _isP1Win)
    {
        if (_isP1Win)
        {
            isP1Win = false;
            //Debug.LogWarning("Lose!");
        }
        else
        {
            isP1Win = true;
            //Debug.LogWarning("Win!");
        }
        StartCoroutine(BattleStateManager(BattleState.Result));
    }

    public void EndTurn()
    {
        if (state == BattleState.Result)
            return;

        if (unitTurn.name == p1Name)
            StartCoroutine(BattleStateManager(BattleState.P2Turn));
        else
            StartCoroutine(BattleStateManager(BattleState.P1Turn));
    }

}
