using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Attack and Shield")]
public class Skill_AttackAndDefense : Skill
{
    public int repeat = 1;
    public float shield = 0;
    public float damage = 0;

    public AttackType attackType;

    public override void Use(BattleUnit user, BattleUnit enemy)
    {
        base.Use(user, enemy);

        switch (target)
        {
            case SkillTarget.Self:
                for (int i = 0; i < repeat; i++)
                {
                    enemy.AddShield(shield);
                    user.TakeDamage(damage, attackType, this);
                }
                break;
            case SkillTarget.Enemy:
                for (int i = 0; i < repeat; i++)
                {
                    user.AddShield(shield);
                    enemy.TakeDamage(damage, attackType, this);
                }
                break;
            case SkillTarget.Both:
                for (int i = 0; i < repeat; i++)
                {
                    user.AddShield(shield);
                    enemy.AddShield(shield);
                    user.TakeDamage(damage, attackType, this);
                    enemy.TakeDamage(damage, attackType, this);
                }
                break;
            default:
                Debug.LogWarning($"Skill {skillName} not have a target");
                break;
        }
    }

    public override float GetValue()
    {
        return (damage + shield) * repeat;
    }

    public override void CreateDesc()
    {
        skillDesc = $"Deal damage to {GetTarget()} with value {damage}. Attack type is {GetAttackType(attackType)}. Gain shield with value {shield} for 1 turn";
    }
}

