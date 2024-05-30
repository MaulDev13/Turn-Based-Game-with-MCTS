using System;
using UnityEngine;
using TMPro;

public class TooltipsManager : MonoBehaviour
{
    public TextMeshProUGUI tipsText;
    public RectTransform tipsWindow;

    public TextMeshProUGUI tipsText2;
    public RectTransform tipsWindow2;

    public float preferredWidth = 200f;

    public static Action<string, Vector2> OnMouseHover;
    public static Action OnMouseLoseFocus;

    public static Action<string, Vector2> OnMouseHover_Sprite;
    public static Action OnMouseLoseFocus_Sprite;

    private void OnEnable()
    {
        OnMouseHover += ShowTip;
        OnMouseLoseFocus += HideTip;

        OnMouseHover_Sprite += ShowTipSprite;
        OnMouseLoseFocus_Sprite += HideTipSprite;
    }

    private void OnDisable()
    {
        OnMouseHover -= ShowTip;
        OnMouseLoseFocus -= HideTip;

        OnMouseHover_Sprite -= ShowTipSprite;
        OnMouseLoseFocus_Sprite -= HideTipSprite;
    }

    private void Start()
    {
        HideTip();
        HideTipSprite();
    }

    private void ShowTip(string tips, Vector2 mousePos)
    {
        tipsText.text = tips;

        tipsWindow.sizeDelta = new Vector2(tipsText.preferredWidth > preferredWidth ? preferredWidth : tipsText.preferredWidth, tipsText.preferredHeight);

        tipsWindow.gameObject.SetActive(true);

        if (mousePos.x < Screen.width / 2f)
        {
            // => left half
            if (mousePos.y < Screen.height / 2f)
            {
                // lower
                tipsWindow.transform.position = new Vector2(mousePos.x + (tipsWindow.sizeDelta.x / 2), mousePos.y + (tipsWindow.sizeDelta.y / 2)) + Vector2.one;
            }
            else
            {
                // upper
                tipsWindow.transform.position = new Vector2(mousePos.x + (tipsWindow.sizeDelta.x / 2), mousePos.y - (tipsWindow.sizeDelta.y / 2)) + new Vector2(1f, -1f);
            }
        }
        else
        {
            // => right half
            if (mousePos.y < Screen.height / 2f)
            {
                // lower
                tipsWindow.transform.position = new Vector2(mousePos.x - (tipsWindow.sizeDelta.x / 2), mousePos.y + (tipsWindow.sizeDelta.y / 2)) + new Vector2(-1f, 1f);
            }
            else
            {
                // upper
                tipsWindow.transform.position = new Vector2(mousePos.x - (tipsWindow.sizeDelta.x / 2), mousePos.y - (tipsWindow.sizeDelta.y / 2)) - Vector2.one;
            }
        }

    }

    private void HideTip()
    {
        tipsText.text = default;
        tipsWindow.gameObject.SetActive(false);
    }

    /// Sprite
    /// 
    private void ShowTipSprite(string tips, Vector2 mousePos)
    {
        tipsText2.text = tips;

        tipsWindow2.sizeDelta = new Vector2(tipsText2.preferredWidth > preferredWidth ? preferredWidth : tipsText2.preferredWidth, tipsText2.preferredHeight);

        tipsWindow2.gameObject.SetActive(true);

        if (mousePos.x < Screen.width / 2f)
        {
            // => left half
            if (mousePos.y < Screen.height / 2f)
            {
                // lower
                tipsWindow2.transform.position = new Vector2(mousePos.x + (tipsWindow2.sizeDelta.x / 2), mousePos.y + (tipsWindow2.sizeDelta.y / 2)) + Vector2.one;
            }
            else
            {
                // upper
                tipsWindow2.transform.position = new Vector2(mousePos.x + (tipsWindow2.sizeDelta.x / 2), mousePos.y - (tipsWindow2.sizeDelta.y / 2)) + new Vector2(1f, -1f);
            }
        }
        else
        {
            // => right half
            if (mousePos.y < Screen.height / 2f)
            {
                // lower
                tipsWindow2.transform.position = new Vector2(mousePos.x - (tipsWindow2.sizeDelta.x / 2), mousePos.y + (tipsWindow2.sizeDelta.y / 2)) + new Vector2(-1f, 1f);
            }
            else
            {
                // upper
                tipsWindow2.transform.position = new Vector2(mousePos.x - (tipsWindow2.sizeDelta.x / 2), mousePos.y - (tipsWindow2.sizeDelta.y / 2)) - Vector2.one;
            }
        }

    }

    private void HideTipSprite()
    {
        tipsText2.text = default;
        tipsWindow2.gameObject.SetActive(false);
    }

}