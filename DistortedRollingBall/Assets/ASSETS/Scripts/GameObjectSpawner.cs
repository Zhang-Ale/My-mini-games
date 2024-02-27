using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSpawner : MonoBehaviour
{

    public GameObject GameObjectToSpawn;
    public Transform SpawnLocation;
    

    public int InitialSpawnTime = 10;
    public int SpawnTime = 5;


    // Use this for initialization
    void Start()
    {
        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        InvokeRepeating("Spawn", InitialSpawnTime, SpawnTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Spawn()
    {
        // Create an instance of the gameobject the selected spawn point's position and rotation.
        Instantiate(GameObjectToSpawn, SpawnLocation.position, SpawnLocation.rotation);
    }

}