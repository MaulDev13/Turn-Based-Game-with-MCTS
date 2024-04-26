using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MC_BattleUnit : BattleUnit
{
    private MC_BattleManager manager;
    private bool isP1 = false;
    private bool isFirstTurn = true;
    private int firstSkillIndex = -99;

    public override void Init(bool _isP1, int _firstSkillIndex, Unit _unit, MC_BattleManager _manager)
    {
        isP1 = _isP1;
        firstSkillIndex = _firstSkillIndex;
        isFirstTurn = true;

        myUnit = _unit;

        manager = _manager;

        isAlive = true;
    }

    public override void Turn()
    {
        if(!isFirstTurn)
            CooldownSkill();

        int index = 0;

        if (isFirstTurn && isP1)
        {
            index = firstSkillIndex;
        }
        else
        {
            // Active a random skill
            bool checkAct = false;
            while (!checkAct)
            {
                index = Random.Range(0, myUnit.skillSet.Count);

                if (myUnit.skillSet[index].CheckCD())
                {
                    checkAct = true;
                }
            };
        }

        if (isFirstTurn)
            isFirstTurn = false;

        Action(myUnit.skillSet[index]);
    }

    public override void Action(Skill s)
    {
        s.Use(this, myEnemy);

        //Debug.Log($"{myUnit.unitName} use {s.skillName}");

        EndTurn();
    }

    public override void EndTurn()
    {
        if (manager.isEnd)
            return;

        //Debug.Log($"{myUnit.unitName} health {myUnit.healthPoint}, {myEnemy.myUnit.unitName} health {myEnemy.myUnit.healthPoint}");

        if (!isAlive || !myEnemy.isAlive)
        {
            manager.EndBattle();
        }
        else
            manager.EndTurn();
    }

    public override void Dead()
    {
        //Debug.Log($"{myUnit.unitName} is dead!");

        isAlive = false;
    }

}
