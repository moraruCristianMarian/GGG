using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public GameObject GameOverPanel;
    public bool CanGameOverNow = false;
    private CanvasGroup _myCanvasGroup;

    void Start()
    {
        _myCanvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    public void GameOverStart()
    {
        //  This function is usually called if the King Goblin is destroyed
        //  This can happen during gameplay, when his inhabited piece is destroyed (good)
        //  ... but it can also happen in placement phase, if the center piece is dragged off the grid (bad)
        //  For the second case, a check is needed (this is enabled on StartLevel() of PlacementGridScript)
        if (!CanGameOverNow)
            return;

        PauseSingletonScript.Get().CanPause = false;

        GameOverPanel.SetActive(true);

        StartCoroutine(FadeIn());
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        PauseSingletonScript.Get().CanPause = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        PauseSingletonScript.Get().CanPause = true;
        
        SceneManager.LoadScene("MainMenu");
    }
    
    private IEnumerator FadeIn()
    {
        _myCanvasGroup.alpha = 0;

        //  Fade in
        for (int i = 0; i < 100; i++)
        {
            _myCanvasGroup.alpha += 0.01f;
            yield return new WaitForSecondsRealtime(0.01f);
        }

        _myCanvasGroup.interactable = true;
    }
}
