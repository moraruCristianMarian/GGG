using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinConScript : MonoBehaviour
{
    public GameObject LETSGOOOO;
    public GameObject Boom;
    public GameObject WinConPanel;
    public ShopMenuScript TheShopMenuScript;
    public TextMeshProUGUI TimeRemainingText;
    public TextMeshProUGUI MoneyRemainingText;
    public TextMeshProUGUI GoblinsRemainingText;
    private Vector2 _posOfLETSGOOOO;
    private bool _weAreGoing = false;
    private float _hype = 6f;
    private RectTransform _letsAnchor;
    private CanvasGroup _myCanvasGroup;

    void Awake()
    {
        _letsAnchor = LETSGOOOO.GetComponent<RectTransform>();
        _posOfLETSGOOOO = _letsAnchor.anchoredPosition;
        _myCanvasGroup = gameObject.GetComponent<CanvasGroup>();
    }
    void Update()
    {
        if (_weAreGoing)
        {
            Vector2 shakingRightNow = new Vector2(Random.Range(-_hype, _hype), Random.Range(-_hype, _hype));
            _letsAnchor.anchoredPosition = _posOfLETSGOOOO + shakingRightNow;
        }
    }
    private IEnumerator StopGoing()
    {
        yield return new WaitForSecondsRealtime(3);
        _weAreGoing = false;
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
        PauseSingletonScript.Get().CanPause = false;

        float remainingSeconds = TimerSingletonScript.Get().StartTimer - TimerSingletonScript.Get().RemainingTimer;
        float minutes = Mathf.FloorToInt(remainingSeconds / 60);
        float seconds = Mathf.FloorToInt(remainingSeconds % 60);
        TimeRemainingText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        MoneyRemainingText.text = string.Format("${0}", TheShopMenuScript.Money);

        GoblinsRemainingText.text = FindObjectsOfType<FramePieceScript>().Length.ToString();

        Time.timeScale = 0f;
        WinConPanel.SetActive(true);
        WeGoAgain();
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        _myCanvasGroup.alpha = 0;

        //  Fade in
        for (int i = 0; i < 50; i++)
        {
            _myCanvasGroup.alpha += 0.02f;
            yield return new WaitForSecondsRealtime(0.01f);
        }

        _myCanvasGroup.interactable = true;
    }

    public void GoToMainMenu()
    {
        PauseSingletonScript.Get().CanPause = true;
        
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

        PauseSingletonScript.Get().CanPause = true;

        Time.timeScale = 1f;
        SceneManager.LoadScene("Level" + nextLevelIndex);
    }
}
