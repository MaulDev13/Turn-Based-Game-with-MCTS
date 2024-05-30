using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Attack and Heal")]
public class Skill_AttackAndHeal : Skill
{
    public int repeat = 1;
    public float heal = 0;
    public float damage = 0;

    public AttackType attackType;

    [SerializeField] public GameObject healEffect;
    [SerializeField] public GameObject attackEffect;

    public override void Use(BattleUnit user, BattleUnit enemy)
    {
        base.Use(user, enemy);

        switch (target)
        {
            case SkillTarget.Self:
                for (int i = 0; i < repeat; i++)
                {
                    hitEffect = attackEffect;
                    user.TakeDamage(damage, attackType, this);

                    hitEffect = healEffect;
                    enemy.Heal(heal, this);
                }
                break;
            case SkillTarget.Enemy:
                for (int i = 0; i < repeat; i++)
                {
                    hitEffect = attackEffect;
                    enemy.TakeDamage(damage, attackType, this);

                    hitEffect = healEffect;
                    user.Heal(heal, this);
                }
                break;
            case SkillTarget.Both:
                for (int i = 0; i < repeat; i++)
                {
                    hitEffect = attackEffect;
                    user.TakeDamage(damage, attackType, this);
                    enemy.TakeDamage(damage, attackType, this);

                    hitEffect = healEffect;
                    user.Heal(heal, this);
                    enemy.Heal(heal, this);
                }
                break;
            default:
                Debug.LogWarning($"Skill {skillName} not have a target");
                break;
        }
    }

    public override float GetValue()
    {
        return (damage + heal) * repeat;
    }

    public override void CreateDesc()
    {
        skillDesc = $"Deal damage to {GetTarget()} with value {damage}. Attack type is {GetAttackType(attackType)}. Heal with value {heal} for 1 turn";
    }
}
