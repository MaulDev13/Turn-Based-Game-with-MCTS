using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Attack")]
public class Skill_BasicAtk : Skill
{
    public float repeat = 1;
    public float value = 0;

    public AttackType attackType;

    public override void Use(BattleUnit user, BattleUnit enemy)
    {
        base.Use(user, enemy);

        switch (target)
        {
            case SkillTarget.Self:
                for (int i = 0; i < repeat; i++)
                {
                    user.TakeDamage(value, attackType, this);
                }
                break;
            case SkillTarget.Enemy:
                for (int i = 0; i < repeat; i++)
                {
                    enemy.TakeDamage(value, attackType, this);
                }
                break;
            case SkillTarget.Both:
                for (int i = 0; i < repeat; i++)
                {
                    user.TakeDamage(value, attackType, this);
                    enemy.TakeDamage(value, attackType, this);
                }
                break;
            default:
                Debug.LogWarning($"Skill {skillName} not have a target");
                break;
        }
    }

    public override float GetValue()
    {
        return value * repeat;
    }

    public override void CreateDesc()
    {
        skillDesc = $"Deal damage to {GetTarget()} with value {repeat} x {value}. Attack type is {GetAttackType(attackType)}";
    }
}
