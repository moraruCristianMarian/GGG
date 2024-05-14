using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSingletonScript : MonoBehaviour
{
    private static TooltipSingletonScript _singleton;
    public TooltipScript _tooltip;

    void Awake()
    {
        _singleton = this;
    }

    public static void Show(string title, string content)
    {
        _singleton._tooltip.SetText(title, content);
        _singleton._tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        _singleton._tooltip.gameObject.SetActive(false);
    }
}
