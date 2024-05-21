using UnityEngine;
using UnityEngine.Animations;

public class BattleUnitAnimation : MonoBehaviour
{
    [SerializeField] private BattleUnit battleUnit;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animControl;

    [HideInInspector] public GameObject hitEffect;
    [SerializeField] private GameObject defaultHitEffect;

    private void OnDisable()
    {
        //battleUnit.iAction -= AnimAct;
        battleUnit.iTakeDamage -= AnimTakeDamage;
        battleUnit.iDead -= AnimDead;
    }

    public void Init(Sprite art)
    {
        if (battleUnit.myUnit.animator != null)
            animControl.runtimeAnimatorController = battleUnit.myUnit.animator;

        hitEffect = defaultHitEffect;

        //battleUnit.iAction += AnimAct;
        battleUnit.iAction2 += AnimAct;
        battleUnit.iTakeDamage += AnimTakeDamage;
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
    }

    public void AnimTakeDamage()
    {
        animControl.SetTrigger("TakeDamage");

        Instantiate(defaultHitEffect, transform.position, transform.rotation);
    }

    public void AnimDead()
    {
        animControl.SetTrigger("Dead");
    }
}
