using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Shield")]
public class Skill_Shield : Skill
{
    public float repeat = 1;
    public float value = 0;

    public override void Use(BattleUnit user, BattleUnit enemy)
    {
        base.Use(user, enemy);

        switch (target)
        {
            case SkillTarget.Self:
                for (int i = 0; i < repeat; i++)
                {
                    user.AddShield(value);
                }
                break;
            case SkillTarget.Enemy:
                for (int i = 0; i < repeat; i++)
                {
                    enemy.AddShield(value);
                }                    
                break;
            case SkillTarget.Both:
                for (int i = 0; i < repeat; i++)
                {
                    user.AddShield(value);
                    enemy.AddShield(value);
                }                    
                break;
            default:
                Debug.LogWarning($"Skill {skillName} not have a target");
                break;
        }
    }

    public override float GetValue()
    {
        return value * repeat / 2;
    }

    public override void CreateDesc()
    {       

        skillDesc = $"{GetTarget()} gain shield with value {repeat} x {value} for 1 turn";
    }
}
