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

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = countDown ? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;

        if(hasLimit && ((countDown && currentTime <= timerLimit) || (!countDown && currentTime >= timerLimit)))
        {
            currentTime = timerLimit;
            SetTimerText();
            timerTxt.color = Color.red;
            enabled = false;
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
}
