using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject MainMainMenu;
    public GameObject LevelsMenu;

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
