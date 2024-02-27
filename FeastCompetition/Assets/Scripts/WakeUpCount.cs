using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class WakeUpCount : MonoBehaviour
{
    public TextMeshProUGUI timer;
    public float timeCount;

    void OnEnable()
    {
        timeCount = 26; 
    }

    void Update()
    {
        DisplayTime(timeCount);

        if (timeCount == 0)
        {
            timeCount = 0; 
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay -= Time.time;
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timer.text = string.Format("{00}", seconds);
    }
}
