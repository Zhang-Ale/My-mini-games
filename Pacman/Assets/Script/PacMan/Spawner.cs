using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float minSpawnTime;
    public float maxSpawnTime;

    void Start()
    {
        Spawn();     
    }

    private void Spawn()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        var spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        Invoke("Spawn", spawnTime);
    }

}
