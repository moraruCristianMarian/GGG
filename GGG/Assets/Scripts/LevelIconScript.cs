using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelIconScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI LevelNameText;
    public Image MyImage;
    public string Name;
    public int LevelIndex;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        transform.localScale = new Vector2(1.2f, 1.2f);
        LevelNameText.text = Name;
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        transform.localScale = new Vector2(1.0f, 1.0f);
        LevelNameText.text = "";
    }

    public void OpenLevel()
    {
        int levelExists = SceneUtility.GetBuildIndexByScenePath("Level" + LevelIndex);

        if (levelExists == -1)
        {
            Debug.Log("uh oh");
            return;
        }

        SceneManager.LoadScene("Level" + LevelIndex);
    }
}
