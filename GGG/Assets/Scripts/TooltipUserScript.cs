using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipUserScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string Title = "";
    public string Content = "";
    
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        TooltipSingletonScript.Show(Title, Content);
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        TooltipSingletonScript.Hide();
    }
}
