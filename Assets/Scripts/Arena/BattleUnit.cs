using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public delegate void UnitEvent();
    public UnitEvent iUpdate;
    public UnitEvent iAction;
    public UnitEvent iTakeDamage;
    public UnitEvent iDead;

    public delegate void UnitEvent2(Skill _skill);
    public UnitEvent2 iAction2;
    public UnitEvent2 iTakeDamage2;
    public UnitEvent2 iHeal2;
    public UnitEvent2 iShield2;

    [SerializeField] private BattleUnitAnimation myAnim;

    [SerializeField] public Unit myUnit;
    [HideInInspector] public BattleUnit myEnemy;

    [HideInInspector] public bool isAlive;
    [SerializeField] public bool isPlayerUnit = true;

    [SerializeField] private float timeToEndTurn = 0.2f;

    private bool isAct;
    public bool IsAct => isAct;

    public virtual void Init(bool _isP1, int _firstSkillIndex, Unit _unit, MC_BattleManager _manager) { }

    public virtual void Init(Unit _unit, bool _isPlayerUnit)
    {
        isPlayerUnit = _isPlayerUnit;

        myUnit = _unit.Clone();
        myUnit.Init(_unit);

        myAnim.Init(myUnit.art);

        if (isPlayerUnit)
        {
            myUnit.unitName = $"Player".ToString(); ;
        }
        else
        {
            myUnit.unitName = $"AI".ToString();
        }

        ZeroShield();

        isAlive = true;
        isAct = false;

        iUpdate?.Invoke();
    }

    public virtual void Turn()
    {
        if (LocalManager_Arena.instance.isEnd)
            return;

        CooldownSkill();

        iUpdate?.Invoke();

        isAct = true;

        if (!isPlayerUnit)
        {
            StartCoroutine(MovementAI_MonteCarlo(LocalManager_Arena.instance.loopOnSimulation, LocalManager_Arena.instance.maxTurnOnSimulation));
        }

        iUpdate?.Invoke();
    }

    // AI have a selected skill
    IEnumerator MovementAI(int _index)
    {
        yield return new WaitForSeconds(2f);

        Action(myUnit.skillSet[_index]);

        yield return new WaitForSeconds(0.2f);
    }

    // AI use random selection for search of skill
    IEnumerator MovementAI()
    {
        yield return new WaitForSeconds(2f);

        bool checkAct = false;
        int index = 0;
        while (!checkAct)
        {
            index = Random.Range(0, myUnit.skillSet.Count);

            if (myUnit.skillSet[index].CheckCD())
            {
                checkAct = true;

                Action(myUnit.skillSet[index]);
            }

            yield return new WaitForSeconds(0.2f);
        };
    }

    // AI search for best skill avaliable with reward base method
    IEnumerator MovementAI2()
    {
        yield return new WaitForSeconds(2f);

        float currentReward = 0f;
        float nextReward = 0f;
        int index = -1;

        for(int i = 0; i < myUnit.skillSet.Count; i++)
        {
            if (myUnit.skillSet[i].CheckCD())
            {
                nextReward = myUnit.skillSet[i].GetValue();
            }
            else
            {
                nextReward = 0;
            }

            if (currentReward < nextReward || currentReward == -1)
            {
                currentReward = nextReward;
                index = i;
            }
        }

        Action(myUnit.skillSet[index]);

        yield return new WaitForSeconds(0.2f);
    }

    // Monte Carlo Tree Search
    IEnumerator MovementAI_MonteCarlo(int loop, int maxTurn)
    {
        yield return new WaitForEndOfFrame();

        // Create dummy
        Unit tmpUnit1 = myUnit.Clone();
        tmpUnit1.Init(myUnit);

        // Create dummy
        Unit tmpUnit2 = myEnemy.myUnit.Clone();
        tmpUnit2.Init(myEnemy.myUnit);
        //tmpUnit2.Init(myEnemy.myUnit);

        // Result check
        List<FinalResult> finalResults = new List<FinalResult>();
        Node bestResult = null;

        LocalManager_ArenaUI.instance.StepAI($"#{LocalManager_Arena.instance.CurrentTurn}");
        LocalManager_ArenaUI.instance.StepAI($"{myUnit.unitName} turn, using Monte Carlo simulation\n");

        
        // Index
        for (int i = 0; i < myUnit.skillSet.Count; i++)
        {
            Node tmpResult = null;
            Node tmpBestResult = null;

            if (myUnit.skillSet[i].CheckCD())
            {
                int winCount = 0;

                for (int j = 0; j < loop; j++)
                {
                    // Reset all resource
                    tmpUnit1.Reset(myUnit);
                    tmpUnit2.Reset(myEnemy.myUnit);

                    // Simulation
                    tmpResult = LocalManager_Arena.instance.mc_manager.Init(tmpUnit1, tmpUnit2, maxTurn, i);

                    if (tmpResult.State == LastState.Win)
                        winCount++;

                    // Best result
                    if (bestResult == null)
                    {
                        bestResult = tmpResult;
                    }
                    else
                    {
                        bestResult = bestResult.Compare(tmpResult);
                    }

                    // Tmp Best Result
                    if (tmpBestResult == null)
                    {
                        tmpBestResult = tmpResult;
                    }
                    else
                    {
                        tmpBestResult = tmpBestResult.Compare(tmpResult);
                    }

                    yield return new WaitForEndOfFrame();
                }

                // Adding final result in this index. Get bestResultlist and winCountList
                var newFinalResult = new FinalResult(i, tmpBestResult.Score, winCount, loop, tmpBestResult);
                finalResults.Add(newFinalResult);

                var newInfo = $"{myUnit.skillSet[i].skillName}: {winCount}/{loop}. Current best result {tmpBestResult.GetDescription()}\n\n";
                LocalManager_ArenaUI.instance.StepAI(newInfo);
                Debug.LogWarning(newInfo);
            }
            else
            {
                LocalManager_ArenaUI.instance.StepAI($"{myUnit.skillSet[i].skillName} in cooldown\n\n");
            }

        }

        // Check final result
        FinalResult bestFinalResult = null;

        for(int i = 0; i < finalResults.Count; i++)
        {
            if (bestFinalResult == null)
                bestFinalResult = finalResults[i];
            else
                bestFinalResult = bestFinalResult.Compare(finalResults[i]);
        }

        var index = bestFinalResult.skillIndex;
        LocalManager_ArenaUI.instance.StepAI($"{myUnit.unitName} use {myUnit.skillSet[bestFinalResult.skillIndex].skillName}.\n" +
            $"Current best result {bestFinalResult.bestNode.GetDescription()}.\n Skill index win ratio = {bestFinalResult.winCount}/{bestFinalResult.loop}.\n" +
            $"Final score = {bestFinalResult.bestNode.Score}+{bestFinalResult.GetWinValue()} => {bestFinalResult.GetValue()} \n\n\n");

        //var index = bestResult.SkillIndex;
        //LocalManager_ArenaUI.instance.StepAI($"{myUnit.unitName} use {myUnit.skillSet[index].skillName}\n\n\n");


        StartCoroutine(MovementAI(index));
    }

    public virtual void Action(Skill s)
    {
        if (LocalManager_Arena.instance.isEnd)
            return;

        if (!isAct)
            return;

        isAct = false;

        //iAction?.Invoke();
        iAction2?.Invoke(s);

        s.Use(this, myEnemy);
        LocalManager_ArenaUI.instance.LastMove($"#{LocalManager_Arena.instance.CurrentTurn} - {s.CreateLastMove(this, myEnemy)}");

        Invoke("EndTurn", timeToEndTurn);
    }

    public virtual void EndTurn()
    {
        if (LocalManager_Arena.instance.isEnd)
            return;

        iUpdate?.Invoke();
        
        LocalManager_Arena.instance.EndTurn();
    }

    public float Heal(float _value, Skill _skill)
    {
        if(ChangeHealth(_value) <= 0)
        {
            Dead();
        }

        iUpdate?.Invoke();
        iHeal2?.Invoke(_skill);

        return _value;
    }

    public float TakeDamage(float _value, AttackType _attackType, Skill _skill)
    {
        if (_attackType == AttackType.Break && myUnit.shieldPoint > 0)
            _value *= 2f;
        else if(_attackType == AttackType.Simple && myUnit.shieldPoint <= 0)
            _value *= 2f;

        if (myUnit.shieldPoint > 0 && _attackType != AttackType.Penetration)
        {
            if(myUnit.shieldPoint >= _value)
            {
                myUnit.shieldPoint -= _value;
                _value = 0;
            } else
            {
                _value -= myUnit.shieldPoint;
                myUnit.shieldPoint = 0;
            }
        }

        //if(_value > 0)
        iTakeDamage?.Invoke();
        iTakeDamage2?.Invoke(_skill);

        if (ChangeHealth(-_value) <= 0)
        {
            Dead();
        }

        iUpdate?.Invoke();

        return _value;
    }

    public virtual void Dead()
    {
        if (LocalManager_Arena.instance.isEnd)
            return;

        isAlive = false;

        iDead?.Invoke();

        Debug.Log($"{myUnit.unitName} dead!");

        LocalManager_Arena.instance.EndBattle(!isPlayerUnit);

        iUpdate?.Invoke();
    }

    private float ChangeHealth(float value)
    {
        myUnit.healthPoint = Mathf.Clamp(myUnit.healthPoint + value, 0f, myUnit.maxHealtPoint);
        return myUnit.healthPoint;
    }

    public void CooldownSkill()
    {
        foreach(Skill s in myUnit.skillSet)
        {
            s.ReduceCD(1);
        }
    }

    public void ZeroShield()
    {
        myUnit.shieldPoint = 0f;
    }

    public void AddShield(float _value, Skill _skill)
    {
        myUnit.shieldPoint += _value;

        iShield2?.Invoke(_skill);
    }

    public string GetName()
    {
        return myUnit.unitName;
    }
}