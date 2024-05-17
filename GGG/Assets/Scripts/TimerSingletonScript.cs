using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerSingletonScript : MonoBehaviour
{
    public float StartTimer = 60;
    public GameObject TimerObject;
    public Image TimerPassedRadial;
    public TextMeshProUGUI TimerText;
    public float RemainingTimer;
    public bool TimerActive = false;
    private static TimerSingletonScript _singleton;

    void Awake()
    {
        _singleton = this;
        RemainingTimer = StartTimer;
        DisplayTime(RemainingTimer);
    }
    public static TimerSingletonScript Get()
    {
        return _singleton;
    }

    void FixedUpdate()
    {
        if (TimerActive)
        {
            RemainingTimer -= Time.deltaTime;
            DisplayTime(RemainingTimer);

            if (RemainingTimer <= 0)
            {
                GameOverScript gos = FindObjectOfType<GameOverScript>();
                if (gos)
                {
                    Time.timeScale = 0f;

                    gos.GameOverStart();
                    TimerActive = false;
                }
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        TimerPassedRadial.fillAmount = 1 - timeToDisplay / StartTimer;
        
        timeToDisplay += 0.95f;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}