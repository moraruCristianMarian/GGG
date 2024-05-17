using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public GameObject GameOverPanel;
    public bool CanGameOverNow = false;
    private CanvasGroup _myCanvasGroup;
    private bool _appeared = false;
    private bool _appearFadeInDone = false;

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

        _myCanvasGroup.alpha = 0;
        _appeared = true;

        GameOverPanel.SetActive(true);
    }

    public void RetryLevel()
    {
        PauseSingletonScript.Get().CanPause = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoToMainMenu()
    {
        PauseSingletonScript.Get().CanPause = true;
        
        SceneManager.LoadScene("MainMenu");
    }

    void Update()
    {
        if ((_appeared) && (!_appearFadeInDone))
        {
            _myCanvasGroup.alpha += Time.deltaTime;
            if (_myCanvasGroup.alpha >= 1)
            {
                _myCanvasGroup.interactable = true;
                _appearFadeInDone = true;
            }
        }
    }
}
