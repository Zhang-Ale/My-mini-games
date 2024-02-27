using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private Vector3 vector;
    public GameObject lifeXPObject;
    public GameObject strangers;
    public GameObject objects;
    public GameObject wall1;
    public GameObject wall;
    public BackgroundColor BC; 
    public const int numObjects = 500;
    public const int numStrangers = 1000;
    public float timer = 0f;
    public float timerDuration = 360f;
    public float XP = 0f;
    public GameObject video;
    public GameObject gameOver, shadeScreen;
    public bool _gameOver;

    public PlayerController player;
    public UI ui;
    public EventObserver eo;

    public TMP_Text scoreText;
    public TMP_Text timerText;

    public GameObject mainMusicObject;
    public AudioSource mainMusic;

    public float totalMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        timer = timerDuration;
        totalMultiplier = 1.0f;

        for (int i = 0; i < numObjects; ++i)
        {
            vector = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0);
            objects = Instantiate(lifeXPObject, vector, Quaternion.identity);
            wall = Instantiate(wall1, -vector, Quaternion.identity);
        }

        for (int i = 0; i < numStrangers; ++i)
        {
            vector = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0);
            objects = Instantiate(strangers, vector, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;

        if (timer < 30)
        {
            BC.changeCol = false;
            video.SetActive(true);
        }

        if(timer == 0)
        {
            gameOver.SetActive(true);
            shadeScreen.SetActive(true); 
            _gameOver = true; 
        }
    }
}