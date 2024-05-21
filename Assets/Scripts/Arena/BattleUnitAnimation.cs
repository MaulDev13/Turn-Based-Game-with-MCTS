using UnityEngine;
using UnityEngine.Animations;

public class BattleUnitAnimation : MonoBehaviour
{
    [SerializeField] private BattleUnit battleUnit;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animControl;

    [HideInInspector] public GameObject hitEffect;
    [SerializeField] private GameObject defaultHitEffect;

    [HideInInspector] public GameObject actEffect;
    [SerializeField] private GameObject defaultActEffect;

    private void OnDisable()
    {
        //battleUnit.iAction -= AnimAct;
        battleUnit.iAction2 -= AnimAct;
        battleUnit.iTakeDamage2 -= AnimTakeDamage;
        battleUnit.iDead -= AnimDead;
    }

    public void Init(Sprite art)
    {
        if (battleUnit.myUnit.animator != null)
            animControl.runtimeAnimatorController = battleUnit.myUnit.animator;

        hitEffect = defaultHitEffect;
        actEffect = defaultActEffect;

        //battleUnit.iAction += AnimAct;
        battleUnit.iAction2 += AnimAct;
        battleUnit.iTakeDamage2 += AnimTakeDamage;
        battleUnit.iDead += AnimDead;

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
            actEffect = _skill.actEffect;
        else
            actEffect = defaultActEffect;

        Instantiate(actEffect, transform.position, transform.rotation);
    }

    public void AnimTakeDamage(Skill _skill)
    {
        animControl.SetTrigger("TakeDamage");

        if (_skill.hitEffect != null)
            hitEffect = _skill.hitEffect;
        else
            hitEffect = defaultHitEffect;

        Instantiate(hitEffect, transform.position, transform.rotation);
    }

    public void AnimDead()
    {
        animControl.SetTrigger("Dead");
    }

    public void GetHitEffect(Skill s)
    {

    }

    public void GetActEffect(Skill s)
    {

    }
}
