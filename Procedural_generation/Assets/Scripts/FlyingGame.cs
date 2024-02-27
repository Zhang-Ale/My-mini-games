using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingGame : MonoBehaviour
{
    public GameObject obstacleUp;
    public GameObject obstacleDown;
    public GameObject paddle;
    public float startOffset;
    public static FlyingGame Instance { get; private set; }
    [SerializeField] private float _maxTime;
    private float _timer;
    private GameObject player;
    public float destroyTime; 
    
    void Start()
    {
        //RestartGame();
        SpawnObstacle();
    }

    void Update()
    {
        if(_timer > _maxTime)
        {
            SpawnObstacle();
            _timer = 0; 
        }
        _timer += Time.deltaTime; 
    }

    public void RestartGame()
    {
        Destroy(paddle);
        paddle = Instantiate(obstacleDown, new Vector2(x: startOffset, y: 0 ), Quaternion.identity); 
    }

    private void SpawnObstacle()
    {
        player = GameObject.Find("Player"); 
        Vector3 spawnPosUp = transform.position + new Vector3(player.transform.position.x + Random.Range(60, 100), Random.Range(-20, -25), 0);
        Vector3 spawnPosDown = transform.position + new Vector3(player.transform.position.x + Random.Range(60, 100), Random.Range(20, 25), 0);
        Vector3 paddlePos = transform.position + new Vector3(player.transform.position.x + Random.Range(60, 100), Random.Range(-10, 10), 0);
        GameObject ObstacleUp = Instantiate(obstacleUp, spawnPosUp, Quaternion.identity);
        GameObject ObstacleDown = Instantiate(obstacleDown, spawnPosDown, Quaternion.Euler(0, 0, 180));
        GameObject Paddle = Instantiate(paddle, paddlePos, Quaternion.identity);
        Destroy(ObstacleDown, destroyTime);
        Destroy(ObstacleUp, destroyTime);
        Destroy(Paddle, destroyTime);
    }
}
