using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArenaUnitInfoPanel : MonoBehaviour
{
    [SerializeField] private Sprite avatar;

    [SerializeField] private Image avatarHolder;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image shieldBar;

    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI hpTxt;
    [SerializeField] private TextMeshProUGUI shieldTxt;



    private BattleUnit myBattleUnit;

    private void OnDisable()
    {
        CutLink();
    }

    public void CutLink()
    {
        myBattleUnit.iUpdate -= UpdateView;
    }

    public void Init(BattleUnit unit)
    {
        myBattleUnit = unit;

        if (avatarHolder != null && avatar == null && myBattleUnit.myUnit.avatar != null)
            avatarHolder.sprite = myBattleUnit.myUnit.avatar;
        else if (avatarHolder != null && avatar != null)
            avatarHolder.sprite = avatar;

        myBattleUnit.iUpdate += UpdateView;

        UpdateView();
    }

    public void UpdateView()
    {
        nameTxt.text = myBattleUnit.myUnit.unitName.ToString();
        hpTxt.text = $"HP {myBattleUnit.myUnit.healthPoint} / {myBattleUnit.myUnit.maxHealtPoint}";
        shieldTxt.text = $"Shield {myBattleUnit.myUnit.shieldPoint}";

        if (healthBar != null)
            healthBar.fillAmount = myBattleUnit.myUnit.healthPoint / myBattleUnit.myUnit.maxHealtPoint;

        if(shieldBar!=null)
        {
            if (myBattleUnit.myUnit.shieldPoint > 0)
                shieldBar.fillAmount = myBattleUnit.myUnit.shieldPoint / myBattleUnit.myUnit.shieldPoint;
            else
                shieldBar.fillAmount = 0;
        }
        
    }
}
