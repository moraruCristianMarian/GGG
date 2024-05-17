using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseSingletonScript : MonoBehaviour
{
    public bool CanPause = true;
    public GameObject PauseMenu;
    public GameObject NoProgressGuy;
    public TextMeshProUGUI MainMenuButtonText;
    private static PauseSingletonScript _singleton;
    private bool _paused = false;
    private bool _confirmMainMenu = false;

    void Awake()
    {
        _singleton = this;
    }
    public static PauseSingletonScript Get()
    {
        return _singleton;
    }

    public void PauseGame()
    {
        if (!CanPause)
            return;

        _paused = true;
        Time.timeScale = 0f;
        PauseMenu.SetActive(true);
    }
    public void ResumeGame()
    {
        if (!CanPause)
            return;
            
        _paused = false;
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);

        SetConfirmMainMenu(false);
    }
    public void TogglePause()
    {
        if (_paused)
                ResumeGame();
            else
                PauseGame();
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
        else
            SetConfirmMainMenu(true);
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.P)) || (Input.GetKeyDown(KeyCode.Escape)))
        {
            TogglePause();
        }
    }
}
