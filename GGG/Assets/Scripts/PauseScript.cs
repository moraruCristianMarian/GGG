using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseScript : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject NoProgressGuy;
    public TextMeshProUGUI MainMenuButtonText;
    private bool _paused = false;
    private bool _confirmMainMenu = false;

    public void PauseGame()
    {
        _paused = true;
        Time.timeScale = 0f;
        PauseMenu.SetActive(true);
    }
    public void ResumeGame()
    {
        _paused = false;
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);

        SetConfirmMainMenu(false);
    }
    void SetConfirmMainMenu(bool isConfirmed)
    {
        _confirmMainMenu = isConfirmed;
        NoProgressGuy.SetActive(_confirmMainMenu);

        if (_confirmMainMenu)
            MainMenuButtonText.color = new Color(1f, 1f, 0.2f, 1f);
        else
            MainMenuButtonText.color = new Color(1f, 1f, 1f, 1f);
    }
    public void GoToMainMenu()
    {
        if (_confirmMainMenu)
            SceneManager.LoadScene("MainMenu");
        else
            SetConfirmMainMenu(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_paused)
                ResumeGame();
            else
                PauseGame();
        }
    }
}