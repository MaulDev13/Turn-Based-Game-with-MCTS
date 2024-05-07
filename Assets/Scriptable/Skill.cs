using System;
using UnityEngine;

public enum SkillTarget
{
    Self,
    Enemy,
    Both
}

public enum AttackType
{
    Normal,
    Penetration, // Can't be block by shield
    Break, // Double damage when target have shield
    Simple // Double damage when target not have a shield
}

[Serializable]
public class Skill : ScriptableObject
{
    [SerializeField] public AnimatorOverrideController _animator;

    [SerializeField] public string skillName;

    public int cd;
    [HideInInspector] public int currentCd = 0;

    [TextArea(4, 8)]
    [SerializeField] public string skillDesc;

    public SkillTarget target;

    public virtual void Use(BattleUnit user, BattleUnit enemy)
    {
        currentCd = cd;
    }

    public virtual float GetValue()
    {
        return 0;
    }

    public virtual bool CheckCD()
    {
        if(currentCd <= 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public virtual void ReduceCD(int value)
    {
        currentCd = Mathf.Clamp(currentCd - value, 0, cd);
    }

    public virtual string CreateLastMove(BattleUnit user, BattleUnit enemy)
    {
        Debug.Log($"{user.GetName()} use {skillName}");
        return $"{user.GetName()} use {skillName}";
    }

    public virtual void CreateDesc()
    {
        skillDesc = $"{skillName.ToUpper()} is a skill";
    }

    public virtual string GetTarget()
    {
        switch(target)
        {
            case SkillTarget.Self:
                return "Oneself";
            case SkillTarget.Enemy:
                return "Enemy";
            case SkillTarget.Both:
                return "Both of you";
            default:
                Debug.LogWarning($"Skill {skillName} not have a target");
                return "default";
        }
    }

    public virtual string GetAttackType(AttackType _attackType)
    {
        switch (_attackType)
        {
            case AttackType.Normal:
                return "normal";
            case AttackType.Penetration:
                return "penetration (can't be block by shield)";
            case AttackType.Break:
                return "break (double if target have shield";
            case AttackType.Simple:
                return "simple (double if target not have a shield";
            default:
                Debug.LogWarning($"Skill {skillName} not have an attack type");
                return "default";
        }
    }
}
