using UnityEngine;
using UnityEngine.Animations;

public class BattleUnitAnimation : MonoBehaviour
{
    [SerializeField] private BattleUnit battleUnit;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animControl;

    private void OnDisable()
    {
        //battleUnit.iAction -= AnimAct;
        battleUnit.iAction2 -= AnimAct;
        battleUnit.iTakeDamage2 -= AnimTakeDamage;
        battleUnit.iDead -= AnimDead;

        battleUnit.iShield2 -= AnimTarget;
        battleUnit.iHeal2 -= AnimTarget;
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
        if(_skill._animator != null)
            animControl.runtimeAnimatorController = _skill._animator;

        animControl.SetTrigger("Attack");

        if (_skill.actEffect != null)
            Instantiate(_skill.actEffect, transform.position, transform.rotation);
    }

    public void AnimTakeDamage(Skill _skill)
    {
        animControl.SetTrigger("TakeDamage");

        if (_skill.hitEffect != null)
            Instantiate(_skill.hitEffect, transform.position, transform.rotation);
        
        if(battleUnit.myUnit.bloodPrefabs != null)
            Instantiate(battleUnit.myUnit.bloodPrefabs, transform.position, transform.rotation);
    }

    public void AnimTarget(Skill _skill)
    {
        if (_skill.hitEffect != null)
            Instantiate(_skill.hitEffect, transform.position, transform.rotation);
    }

    public void AnimDead()
    {
        animControl.SetTrigger("Dead");
    }
}
