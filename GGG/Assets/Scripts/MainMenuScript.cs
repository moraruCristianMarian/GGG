using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    public GameObject MainMainMenu;
    public GameObject LevelsMenu;
    public GameObject LevelsHolder;
    public TextMeshProUGUI LevelNameText;

    public GameObject LevelIconPrefab;    
    public List<string> LevelNames;
    public List<Sprite> LevelSprites;

    void Start()
    {
        for (int i = 0; i < LevelNames.Count; i++)
        {
            GameObject levelIcon = Instantiate(LevelIconPrefab);

            levelIcon.transform.SetParent(LevelsHolder.transform, false);

            float iconX = 0 - 250 + (i % 5) * 100;
            float iconY = 0 + 100 - (i / 5) * 100;
            levelIcon.transform.localPosition = new Vector2(iconX, iconY);

            levelIcon.GetComponent<LevelIconScript>().MyImage.sprite = LevelSprites[i];
            levelIcon.GetComponent<LevelIconScript>().Name = LevelNames[i];
            levelIcon.GetComponent<LevelIconScript>().LevelIndex = i;

            levelIcon.GetComponent<LevelIconScript>().LevelNameText = LevelNameText;
        }
    }

    public void OpenLevelsMenu()
    {
        MainMainMenu.SetActive(false);
        LevelsMenu.SetActive(true);
    }

    public void GoBackToMainMainMenu()
    {
        MainMainMenu.SetActive(true);
        LevelsMenu.SetActive(false);
    }

    public void SecretThirdThing()
    {
        Debug.Log("not yet");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
