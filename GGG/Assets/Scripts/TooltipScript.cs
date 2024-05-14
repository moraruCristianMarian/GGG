using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipScript : MonoBehaviour
{
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI ContentText;

    public void SetText(string title, string content)
    {
        if (string.IsNullOrEmpty(title))
            TitleText.gameObject.SetActive(false);
        else
        {
            TitleText.gameObject.SetActive(true);
            TitleText.text = title;
        }

        ContentText.text = content;
    }

    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
