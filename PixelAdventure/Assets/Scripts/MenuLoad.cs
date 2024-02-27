using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLoad : MonoBehaviour
{
    public GameObject PauseMenu;
    public bool paused = false;
    string sceneName;
    public GameObject menuMusic; 
    public void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }
    public void Update()
    {
        if (sceneName == "Game" && Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (paused == true)
            {
                menuMusic.SetActive(true); 
                Time.timeScale = 1.0f;
                PauseMenu.SetActive(false);
                paused = false;
            }
            else 
            {
                menuMusic.SetActive(false); 
                Time.timeScale = 0.0f;
                PauseMenu.SetActive(true);
                paused = true;
            }
        }
    }
    public void Resume()
    {
        Time.timeScale = 1.0f;
        PauseMenu.SetActive(false);
        paused = false; 
    }

    public void LoadSceneMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadSceneGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Exitgame()
    {
        Debug.Log("exitgame");
        Application.Quit();
    }
}
