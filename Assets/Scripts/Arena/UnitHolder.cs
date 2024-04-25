using System.Collections;
using UnityEngine;

public class UnitHolder : MonoBehaviour
{
    [HideInInspector] public BattleUnit myBattleUnit;

    [HideInInspector] public bool onPlayerSide = true;

    [TextArea(8,4)]
    [SerializeField] private string tipsToShow;
    [SerializeField] private float timeToWait = 0.5f;

    public void Init(BattleUnit battleUnit, bool isPlayerSide)
    {
        myBattleUnit = battleUnit;
        onPlayerSide = isPlayerSide;

        tipsToShow = $"{myBattleUnit.myUnit.unitName}\n{myBattleUnit.myUnit.healthPoint} / {myBattleUnit.myUnit.maxHealtPoint}";
    }

    private void OnMouseEnter()
    {
        StopAllCoroutines();

        StartCoroutine(StartTimer());
    }

    private void OnMouseExit()
    {
        StopAllCoroutines();

        TooltipsManager.OnMouseLoseFocus();
    }

    private void ShowTips()
    {
        TooltipsManager.OnMouseHover(tipsToShow, Input.mousePosition);
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timeToWait);

        ShowTips();
    }
}
