using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public GameObject FinalScore;
    TextMeshProUGUI finalScoreText; 
    public TextMeshProUGUI _scoreText;
    public int _count;
    public AudioSource gameMusic;
    public float timeRemaining;
    bool timeIsRunning;
    public TextMeshProUGUI timeText;
    public bool gameStart; 
    public bool gameOver;
    public TextMeshProUGUI CountThree;
    public TextMeshProUGUI CountTwo;
    public TextMeshProUGUI CountOne;

    void Start()
    {
        timeIsRunning = false; 
        FinalScore.SetActive(false);
        StartCoroutine(CountTime(1f));
        finalScoreText = FinalScore.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        StartCoroutine(DisplayTime(timeRemaining));
        End();
    }

    public void Scored()
    {
        _count += 1;
        _scoreText.text = "Score: " + _count;
    }

    IEnumerator CountTime(float _wait)
    {
        CountThree.alpha = 255f;
        yield return new WaitForSeconds(_wait);
        CountThree.alpha = 0f;
        CountTwo.alpha = 255f;
        yield return new WaitForSeconds(_wait);
        CountTwo.alpha = 0f;
        CountOne.alpha = 255f;
        yield return new WaitForSeconds(_wait);
        CountOne.alpha = 0f;
        yield return null;
        gameMusic.Play();
        gameStart = true; 
    }

    IEnumerator DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 0;
        float minutes = Mathf.FloorToInt(timeRemaining / 60);
        float seconds = Mathf.FloorToInt(timeRemaining % 60);
        //62 % 60 = 1min2sec; 125 & 60 = 2min5sec; 46 % 60 = 46sec
        //float milliSeconds = (timeToDisplay % 1) * 1000;

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        yield return new WaitForSeconds(4f);
        timeIsRunning = true;
    }

    void End()
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
                gameOver = true;
                finalScoreText.text = _scoreText.text;
                FinalScore.SetActive(true);
                gameMusic.Stop(); 
                timeIsRunning = false;
            }
        }
    }
}
