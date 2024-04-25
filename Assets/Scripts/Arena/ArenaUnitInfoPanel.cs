using UnityEngine;
using TMPro;

public class ArenaUnitInfoPanel : MonoBehaviour
{
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

        myBattleUnit.iUpdate += UpdateView;

        UpdateView();
    }

    public void UpdateView()
    {
        nameTxt.text = myBattleUnit.myUnit.unitName.ToString();
        hpTxt.text = $"HP {myBattleUnit.myUnit.healthPoint} / {myBattleUnit.myUnit.maxHealtPoint}";
        shieldTxt.text = $"Shield {myBattleUnit.myUnit.shieldPoint}";
    }
}
