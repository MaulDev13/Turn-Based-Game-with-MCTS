using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTips : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string tipsToShow;
    [SerializeField] private float timeToWait = 0.5f;

    public void InitTipsText(string value)
    {
        tipsToShow = value;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();

        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
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
