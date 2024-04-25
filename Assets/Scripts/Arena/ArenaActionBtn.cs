using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArenaActionBtn : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private Image cdLayer;
    [SerializeField] private HoverTips tips;

    private Skill mySkill;
    private BattleUnit myBattleUnit;

    public void Init(BattleUnit unit, Skill skill)
    {
        myBattleUnit = unit;
        mySkill = skill;

        nameTxt.text = mySkill.skillName.ToString();

        CooldownLayerUpdate(skill);

        tips.InitTipsText(mySkill.skillDesc);
    }

    public void CooldownLayerUpdate(Skill skill)
    {
        if(skill.currentCd > 1)
        {
            cdLayer.fillAmount = (float)skill.currentCd / (float)skill.cd;
        }
        else
        {
            cdLayer.fillAmount = 0f;
        }

        
    }

    public void OnBtnClick()
    {
        if (!myBattleUnit.IsAct)
            return;

        if(mySkill.CheckCD())
        {
            LocalManager_ArenaUI.instance.ClearActionButton();

            myBattleUnit.Action(mySkill);
        }
        else
        {
            Debug.Log("Skill in cooldown");
        }
    }
}
