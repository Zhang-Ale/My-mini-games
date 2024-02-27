using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] float startTime;
    ScoreManager sm;
    float currentTime;
    bool timerStarted = false;
    [SerializeField] TMP_Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = startTime;
        timerText.text = "Time Left: " + currentTime.ToString("f1");
        timerStarted = true;
        sm = GameObject.Find("Canvas").GetComponent<ScoreManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (timerStarted)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                timerStarted = false;
                currentTime = 0;

                if (sm.playerCount > sm.enemyCount)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                else
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
                }

            }
            timerText.text = "Time Left: " + currentTime.ToString("f1");
        }
    }
}
