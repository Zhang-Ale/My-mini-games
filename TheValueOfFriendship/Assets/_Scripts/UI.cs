using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public GameManager gm;
    public EventObserver eo;
    public TMP_Text timerDisplay;
    public TMP_Text XPDisplay;
    public TMP_Text winDisplay;
    private int intTimer;
    private int minutes;
    private int seconds;
    private string secondsString;

    void Update()
    {
        intTimer = (int)gm.timer;
        minutes = intTimer / 60;
        seconds = intTimer % 60;

        if (seconds > 9)
            secondsString = seconds.ToString("0");
        else
            secondsString = '0' + seconds.ToString("0");

        timerDisplay.text = minutes.ToString("0") + ':' + secondsString;
        XPDisplay.text = "XP: " + gm.XP.ToString("F");

        //if (gm.timer <= 0)
        {
            //The game is over, maybe display something on winDisplay
        }
    }
}
