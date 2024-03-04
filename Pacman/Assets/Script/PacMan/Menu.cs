using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Menu : Observable
{
    public GameObject button1, button2;
    public int point;
    public TextMeshProUGUI pointText;
    public bool gameStarted;
    public GameObject winText, loseText; 

    public void StartGame()
    {
        point = 0;
        winText.SetActive(false); 
        loseText.SetActive(false); 
        button1.SetActive(false);
        gameStarted = true;
    }

    public void StopGame()
    {
        gameStarted = false;
        loseText.SetActive(true); 
        button2.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Start()
    {
        SetCountText();
    }

    public void AddPoint()
    {
        point = point + 1;
        SetCountText();
    }

    void SetCountText()
    {
        pointText.text = "Point: " + point.ToString() + "/50";
        if (point >= 50)
        {
            winText.SetActive(true); 
            StopGame(); 
        }
    }

}
