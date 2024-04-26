using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LastState
{
    Win, Lose, Draw
}

public class Node
{
    private int endTurn;
    private float playerHealth;
    private float enemyHealth;
    private LastState state;
    private int skillIndex;
    private float score;

    public int EndTurn => endTurn;
    public LastState State => state;
    public int SkillIndex => skillIndex;
    public float Score => score;

    public Node(int _skillIndex, int _endTurn, float _playerHealth, float _enemyHealth, LastState _state)
    {
        skillIndex = _skillIndex;
        endTurn = _endTurn;
        playerHealth = _playerHealth;
        enemyHealth = _enemyHealth;
        state = _state;

        score = 0;
        switch (state) 
        {
            case LastState.Win:
                score = 1000 + playerHealth;
                break;
            case LastState.Lose:
                score = -1000 - enemyHealth;
                break;
            case LastState.Draw:
                score = playerHealth - enemyHealth;
                break;
        }
        
    }

    public Node Compare(Node _other)
    {
        if (state == LastState.Win && _other.State != LastState.Win)
            return this;
        else if (state != LastState.Win && _other.State == LastState.Win)
            return _other;
        else if (state == LastState.Win && _other.State == LastState.Win)
        {
            if(endTurn < _other.EndTurn)
                return this;
            else //if (endTurn >= _other.EndTurn)
                return _other;
        }
        else if (state == LastState.Lose && _other.State != LastState.Lose)
            return _other;
        else if (state != LastState.Lose && _other.State == LastState.Lose)
            return this;
        else if (state == LastState.Lose && _other.State == LastState.Lose)
        {
            if (endTurn < _other.EndTurn)
                return _other;
            else //if (endTurn >= _other.EndTurn)
                return this;
        }
        else if (state == LastState.Draw && _other.State == LastState.Draw)
        {
            if (score >= _other.score)
                return this;
            else //if (score < _other.score)
                return _other;
        }
        else
            return this;
    }

    public string GetDescription()
    {
        switch (state) 
        {
            case LastState.Draw:
                return $"Skill index = {skillIndex}, Result = Draw, Score = {score}, turn {endTurn}";
            case LastState.Win:
                return $"Skill index = {skillIndex}, Result = Win, Score = {score}, turn {endTurn}";
            case LastState.Lose:
                return $"Skill index = {skillIndex}, Result = Lose, Score = {score}, turn {endTurn}";
            default:
                return $"This is default state, you are not supose to be here. please check your code!";
        }
    }
}

public class MC_BattleManager : MonoBehaviour
{
    [Header("Inspector")]
    [HideInInspector] public bool isEnd = false;
    [HideInInspector] public LastState lastState = LastState.Draw;
    [SerializeField] public MC_BattleUnit playerUnit1; // playerUnit1 is the one who use MCTS. Always move first
    [SerializeField] public MC_BattleUnit playerUnit2;
    [HideInInspector] public MC_BattleUnit unitTurn;

    private string p1Name = "P1";
    //private string p2Name = "P2";

    int turnAvaliable = 0;
    int currentTurn;

    int firstSkillIndex;

    enum BattleState
    {
        P1Turn, P2Turn, Result
    }
    private BattleState state;

    public Node Init(Unit _user, Unit _enemy, int _turnAvaliable, int _firstSkillIndex)
    {
        turnAvaliable = _turnAvaliable;
        currentTurn = 0;
        firstSkillIndex = _firstSkillIndex;

        // Initiation player unit
        playerUnit1.Init(true, _firstSkillIndex, _user, this);
        playerUnit1.name = p1Name;

        // Initiation enemy unit
        playerUnit2.Init(false, 0, _enemy, this);

        // Check who is enemy
        playerUnit1.myEnemy = playerUnit2;
        playerUnit2.myEnemy = playerUnit1;

        state = BattleState.P1Turn;
        //StartCoroutine(BattleStateManager(BattleState.P1Turn));
        BattleStateManager(BattleState.P1Turn);

        Debug.LogWarning($"Skill index = {firstSkillIndex}, Current turn = {currentTurn}, last state = {lastState}, player health = {playerUnit1.myUnit.healthPoint}, enemy health = {playerUnit2.myUnit.healthPoint}");

        return new Node(firstSkillIndex, currentTurn, playerUnit1.myUnit.healthPoint, playerUnit2.myUnit.healthPoint, lastState);
    }

    void BattleStateManager(BattleState value)
    {
        //yield return new WaitForEndOfFrame();

        if (state == BattleState.Result)
            //yield return null;
            return;

        currentTurn++;
        if (currentTurn >= turnAvaliable && value != BattleState.Result)
        {
            value = BattleState.Result;
            lastState = LastState.Draw;
        }

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
                // End of simulation
                break;
        }
    }

    public void EndBattle()
    {
        if (!playerUnit1.isAlive)
        {
            lastState = LastState.Lose;
            //Debug.Log("Lose!");
        }
        else if(!playerUnit2.isAlive)
        {
            lastState = LastState.Win;
            //Debug.Log("Win!");
        } else
        {
            Debug.Log("Some is wrong with mc end battle");
        }
        //StartCoroutine(BattleStateManager(BattleState.Result));
        BattleStateManager(BattleState.Result);
    }

    public void EndTurn()
    {
        if (state == BattleState.Result)
            return;

        if (unitTurn.name == p1Name)
            //StartCoroutine(BattleStateManager(BattleState.P2Turn));
            BattleStateManager(BattleState.P2Turn);
        else
            //StartCoroutine(BattleStateManager(BattleState.P1Turn));
            BattleStateManager(BattleState.P1Turn);
    }

}
