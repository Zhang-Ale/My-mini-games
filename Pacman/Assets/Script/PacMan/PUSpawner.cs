using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUSpawner : MonoBehaviour
{
    public GameObject puPrefab, spuPrefab;
    private GameObject _spawnLocation;
    private GameObject[] _spawnLocations;
    private Transform spawnTrans;
    void Start()
    {
        _spawnLocations = GameObject.FindGameObjectsWithTag("Destination");
    }

    private void SetNextDestination()
    {
        int index = Random.Range(0, _spawnLocations.Length);
        _spawnLocation = _spawnLocations[index];
        spawnTrans = _spawnLocation.transform;
    }
    public void PUSpawn()
    {
        SetNextDestination(); 
        Instantiate(puPrefab, spawnTrans.position, Quaternion.identity);
        Debug.Log("A new power up has been spawned.");
    }
    public void SPUSpawn()
    {
        SetNextDestination();
        Instantiate(spuPrefab, spawnTrans.position, Quaternion.identity);
        Debug.Log("A new shotgun power up has been spawned.");
    }
}
