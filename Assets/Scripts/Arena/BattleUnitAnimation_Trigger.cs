using UnityEngine;

public class BattleUnitAnimation_Trigger : MonoBehaviour
{
    [SerializeField] BattleUnitAnimation anim;
    [SerializeField] BattleUnit unit;

    public void ActEffect()
    {
        anim.ActEffect();
    }

    public void HitEffect()
    {
        anim.HitEffect();
    }

    public void TakeDamageEffect()
    {
        anim.TakeDamageEffect();
    }
}
