using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Heal")]
public class Skill_Heal : Skill
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
                    user.Heal(value, this);
                }
                break;
            case SkillTarget.Enemy:
                for (int i = 0; i < repeat; i++)
                {
                    enemy.Heal(value, this);
                }
                break;
            case SkillTarget.Both:
                for (int i = 0; i < repeat; i++)
                {
                    user.Heal(value, this);
                    enemy.Heal(value, this);
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
        skillDesc = $"Heal {GetTarget()} with value {value}";
    }
}
