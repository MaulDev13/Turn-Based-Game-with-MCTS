using System.Collections.Generic;
using UnityEngine;

public class BattleUnitAnimation : MonoBehaviour
{
    [SerializeField] private BattleUnit battleUnit;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animControl;

    private Skill currentSkill;

    private void OnDisable()
    {
        //battleUnit.iAction -= AnimAct;
        battleUnit.iAction2 -= AnimAct;
        battleUnit.iTakeDamage2 -= AnimTakeDamage;
        battleUnit.iDead -= AnimDead;

        battleUnit.iShield2 -= AnimTarget;
        battleUnit.iHeal2 -= AnimTarget;

        LocalManager_ArenaUI.instance.iAnimSpeedUpdate -= AnimSpeed;
    }

    public void Init(Sprite art)
    {
        if (battleUnit.myUnit.animator != null)
            animControl.runtimeAnimatorController = battleUnit.myUnit.animator;

        //battleUnit.iAction += AnimAct;
        battleUnit.iAction2 += AnimAct;
        battleUnit.iTakeDamage2 += AnimTakeDamage;
        battleUnit.iDead += AnimDead;

        battleUnit.iShield2 += AnimTarget;
        battleUnit.iHeal2 += AnimTarget;

        LocalManager_ArenaUI.instance.iAnimSpeedUpdate += AnimSpeed;

        spriteRenderer.sprite = art;

        if(!battleUnit.isPlayerUnit)
        {
            //spriteRenderer.flipX = true;
        } else
        {
            //spriteRenderer.flipX = false;
        }
    }

    public void AnimAct(Skill _skill)
    {
        currentSkill = _skill;

        if (currentSkill._animator != null)
            animControl.runtimeAnimatorController = currentSkill._animator;

        animControl.SetTrigger("Attack");

        //ActEffect()
        ActEffect();
    }

    public void ActEffect()
    {
        if (currentSkill == null)
            return;

        if (currentSkill.actEffect != null)
            Instantiate(currentSkill.actEffect, transform.position, transform.rotation);
    }

    public void HitEffect()
    {
        if (currentSkill == null)
            return;

        if (currentSkill.hitEffect != null)
            Instantiate(currentSkill.hitEffect, transform.position, transform.rotation);
    }

    public void TakeDamageEffect()
    {
        if (battleUnit.myUnit.bloodPrefabs != null)
            Instantiate(battleUnit.myUnit.bloodPrefabs, transform.position, transform.rotation);
    }

    public void AnimTakeDamage(Skill _skill)
    {
        currentSkill = _skill;

        animControl.SetTrigger("TakeDamage");

        //HitEffect()
        HitEffect();

        //TakeDamageEffect()
        TakeDamageEffect();
    }

    public void AnimTarget(Skill _skill)
    {
        currentSkill = _skill;

        //??
        if (currentSkill.hitEffect != null)
            Instantiate(_skill.hitEffect, transform.position, transform.rotation);
    }

    public void AnimDead()
    {
        animControl.SetTrigger("Dead");
    }

    public void AnimSpeed()
    {
        animControl.speed = LocalManager_Arena.instance.animationSpeed;

        Debug.Log($"Animation speed = {animControl.speed}");
    }
}
