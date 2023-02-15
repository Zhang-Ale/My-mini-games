using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerTxt;

    public float currentTime;
    public bool countDown;

    public bool hasLimit;
    public float timerLimit;
    public bool timeOver; 

    public CoinController CC;

    private void Start()
    {
        timeOver = false; 
    }

    void Update()
    {
        if (!timeOver)
        {
            currentTime = countDown ? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;
        }

        if(hasLimit && ((countDown && currentTime <= timerLimit) || (!countDown && currentTime >= timerLimit)))
        {
            currentTime = timerLimit;
            SetTimerText();
            timerTxt.color = Color.red;
            enabled = false;
        }

        if(currentTime <= 0 && CC.needCoins || !CC.alive)
        {
            CC.OnLose();
            StopCounting(); 
        }

        if (CC.gameEnd)
        {
            StopCounting();
        }

        SetTimerText();
    }
    private void SetTimerText()
    {
        if(currentTime > 0)
        {
            timerTxt.text = currentTime.ToString("Time Remaining: " + "0");
        }
    }

    void StopCounting()
    {
        timeOver = true;
    }
}
