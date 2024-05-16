using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinConScript : MonoBehaviour
{
    public GameObject LETSGOOOO;
    public GameObject Boom;
    public GameObject WinConPanel;
    private Vector2 _posOfLETSGOOOO;
    private bool _weAreGoing = false;
    private float _hype = 6f;
    private RectTransform _letsAnchor;

    void Awake()
    {
        // _posOfLETSGOOOO = LETSGOOOO.transform.position;
        _letsAnchor = LETSGOOOO.GetComponent<RectTransform>();
        _posOfLETSGOOOO = _letsAnchor.anchoredPosition;
    }
    void Update()
    {
        if (_weAreGoing)
        {
            Vector2 shakingRightNow = new Vector2(Random.Range(-_hype, _hype), Random.Range(-_hype, _hype));
            // LETSGOOOO.transform.position = _posOfLETSGOOOO + shakingRightNow;
            _letsAnchor.anchoredPosition = _posOfLETSGOOOO + shakingRightNow;
        }
    }
    private IEnumerator StopGoing()
    {
        yield return new WaitForSecondsRealtime(3);
        _weAreGoing = false;
        // LETSGOOOO.transform.position = _posOfLETSGOOOO;
        _letsAnchor.anchoredPosition = _posOfLETSGOOOO;
    }
    public void WeGoAgain()
    {
        if (_hype > 60f)
        {
            StopAllCoroutines();
            LETSGOOOO.SetActive(false);
            Boom.SetActive(true);
            Boom.GetComponent<Animator>().SetBool("Bomba", true);
            return;
        }

        StopAllCoroutines();
        _weAreGoing = true;
        _hype += 2f;
        StartCoroutine(StopGoing());
    }

    public void YouWin()
    {
        Time.timeScale = 0f;
        WinConPanel.SetActive(true);
        WeGoAgain();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void GoNext()
    {
        //  Removes "Level" from "LevelX", leaving the level number "X"
        string levelIndexStr = SceneManager.GetActiveScene().name.Substring(5);

        int nextLevelIndex;
        bool gotNextLevel = int.TryParse(levelIndexStr, out nextLevelIndex);

        if (!gotNextLevel)
        {
            Debug.Log("nuh uh");
            return;
        }
        nextLevelIndex += 1;

        int levelExists = SceneUtility.GetBuildIndexByScenePath("Level" + nextLevelIndex);

        if (levelExists == -1)
        {
            Debug.Log("uh oh");
            return;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene("Level" + nextLevelIndex);
    }
}
