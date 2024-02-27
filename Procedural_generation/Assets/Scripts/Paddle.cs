using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public int score = 10;
    private GameplayManager gameplayManager;

    private void Awake()
    {
        gameplayManager = GameObject.FindObjectOfType <GameplayManager>();

    }
    void OnCollisionEnter2D (Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            gameplayManager.UpdateScore(score);
            Destroy(this.gameObject);
        }
    }
}
