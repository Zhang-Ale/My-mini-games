using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private float timeRemaining = 600;
    bool timeIsRunning;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI playerNameText; 
    private void Start()
    {
        SetName("Hero"); 
    }
    void Update()
    {
        StartCoroutine(DisplayTime(timeRemaining));
        TimeTicking();
    }

    void SetName (string playerName)
    {
        playerNameText.text = playerName; 
    }
    
    IEnumerator DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeRemaining / 60);
        float seconds = Mathf.FloorToInt(timeRemaining % 60);
        //62 % 60 = 1min2sec; 125 & 60 = 2min5sec; 46 % 60 = 46sec
        //float milliSeconds = (timeToDisplay % 1) * 1000;

        yield return new WaitForSeconds(0.1f);
        timeIsRunning = true;
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimeTicking()
    {
        if (timeIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                timeIsRunning = false;
                Application.Quit();
            }
        }
    }
}
