using UnityEngine;

public class BattleUnitAnimation : MonoBehaviour
{
    [SerializeField] private BattleUnit battleUnit;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animControl;

    private void OnDisable()
    {
        battleUnit.iAction -= AnimAct;
        battleUnit.iTakeDamage -= AnimTakeDamage;
        battleUnit.iDead -= AnimDead;
    }

    public void Init(Sprite art)
    {
        battleUnit.iAction += AnimAct;
        battleUnit.iTakeDamage += AnimTakeDamage;
        battleUnit.iDead += AnimDead;

        spriteRenderer.sprite = art;

        if(!battleUnit.isPlayerUnit)
        {
            spriteRenderer.flipX = true;
        } else
        {
            spriteRenderer.flipX = false;
        }
    }

    public void AnimAct()
    {
        animControl.SetTrigger("Attack");
    }

    public void AnimTakeDamage()
    {
        animControl.SetTrigger("TakeDamage");
    }

    public void AnimDead()
    {
        animControl.SetTrigger("Dead");
    }
}
