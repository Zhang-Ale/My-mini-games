using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int currentScore;

    public string player_string = "10";
    public int player_int = 10;
    public GameObject addScoreNotify;
    public bool gameOver;
    private float _maxTime = 1;
    private float _timer;

    private void FixedUpdate()
    {
        if (!gameOver && _timer > _maxTime)
        {
            currentScore += 1;
            _timer = 0;
        }
        _timer += Time.deltaTime;
        
        scoreText.text = "Score: " + currentScore.ToString();
    }

    public void UpdateScore(int score)
    {
        currentScore += score;
        scoreText.text = "Score: " + currentScore.ToString();
        StartCoroutine(Appear());
    }
    public void StringToInt()
    {
        //convert a string to int: player_string="10" -> player_string=10
        //use: int.TryParse(string, out integer);
        int.TryParse(player_string, out int guess);
        if (guess == player_int)
        {
            Debug.Log("You win " + guess + " points.");
        }
    }

    IEnumerator Appear()
    {
        addScoreNotify.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        addScoreNotify.SetActive(false);
    }
}
