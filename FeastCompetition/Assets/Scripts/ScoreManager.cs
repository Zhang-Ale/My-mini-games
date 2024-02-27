using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI playerScore;
    public float playerCount;
    public bool playerScoreAdd; 
    public TextMeshProUGUI enemyScore;
    public float enemyCount;
    public bool enemyScoreAdd;
    private GameObject[] _enemies;
    public ZombieSpawner spawner;

    void Start()
    {
        playerCount = 0;
        enemyCount = 0; 
        playerScore.text = "Player: " + playerCount.ToString() + " brain(s) eaten";
        enemyScore.text = "Enemy: " + enemyCount.ToString() + " brain(s) eaten";
    }

    void Update()
    {
        if (playerCount >= 4 && playerScoreAdd)
        {
            _enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in _enemies)
            {
                NavMeshAgent _agent = enemy.GetComponent<NavMeshAgent>();
                _agent.speed += 0.1f; 
            }
        }

        if (playerCount >= 30 && playerScoreAdd)
        {
            spawner.LargeSpawn();
            _enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in _enemies)
            {
                NavMeshAgent _agent = enemy.GetComponent<NavMeshAgent>();
                _agent.speed = 4f;
            }
        }
    }

    public void AddPointToPlayer()
    {
        playerScoreAdd = true;
        playerCount = playerCount + 1f;
        playerScore.text = "Player: " + playerCount.ToString() + " brain(s) eaten";
        playerScoreAdd = false;
    }

    public void AddPointToEnemy()
    {
        enemyScoreAdd = true;
        enemyCount = enemyCount + 1f;
        enemyScore.text = "Enemy: " + enemyCount.ToString() + " brain(s) eaten";
        enemyScoreAdd = false;
    }
    
}
