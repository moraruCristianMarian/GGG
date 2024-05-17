using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupTextSingletonScript : MonoBehaviour
{
    public TextMeshProUGUI PopupText;
    private static PopupTextSingletonScript _singleton;
    private CanvasGroup _myCanvasGroup;

    void Awake()
    {
        _singleton = this;
        _myCanvasGroup = gameObject.GetComponent<CanvasGroup>();
    }
    public static PopupTextSingletonScript Get()
    {
        return _singleton;
    }

    public void Show(string text)
    {
        Show(text, Color.red);
    }
    public void Show(string text, Color textColor)
    {
        PopupText.text = text;
        PopupText.color = textColor;

        StartCoroutine(FadeInFadeOut());
    }

    private IEnumerator FadeInFadeOut()
    {
        _myCanvasGroup.alpha = 0;
        PopupText.gameObject.SetActive(true);

        //  Fade in
        for (int i = 0; i < 100; i++)
        {
            _myCanvasGroup.alpha += 0.01f;
            yield return null;
        }

        //  Remain visible for 4 seconds
        yield return new WaitForSecondsRealtime(4);

        //  Fade out
        for (int i = 0; i < 100; i++)
        {
            _myCanvasGroup.alpha -= 0.01f;
            yield return null;
        }

        _myCanvasGroup.alpha = 0;
        PopupText.gameObject.SetActive(false);
    }
}
