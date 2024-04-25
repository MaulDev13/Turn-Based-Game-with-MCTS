using System;
using UnityEngine;
using TMPro;

public class TooltipsManager : MonoBehaviour
{
    public TextMeshProUGUI tipsText;
    public RectTransform tipsWindow;

    public static Action<string, Vector2> OnMouseHover;
    public static Action OnMouseLoseFocus;

    private void OnEnable()
    {
        OnMouseHover += ShowTip;
        OnMouseLoseFocus += HideTip;
    }

    private void OnDisable()
    {
        OnMouseHover -= ShowTip;
        OnMouseLoseFocus -= HideTip;
    }

    private void Start()
    {
        HideTip();
    }

    private void ShowTip(string tips, Vector2 mousePos)
    {
        tipsText.text = tips;

        tipsWindow.sizeDelta = new Vector2(tipsText.preferredWidth > 200 ? 200 : tipsText.preferredWidth, tipsText.preferredHeight);

        tipsWindow.gameObject.SetActive(true);
        //tipsWindow.transform.position = new Vector2(mousePos.x + tipsWindow.sizeDelta.x * 2, mousePos.y);
        tipsWindow.transform.position = new Vector2(mousePos.x + tipsWindow.sizeDelta.x, mousePos.y);
    }

    private void HideTip()
    {
        tipsText.text = default;
        tipsWindow.gameObject.SetActive(false);
    }
}
