using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject prefab;
    GameObject zombie;
    ZombieController ZC;

    void Start()
    {
        zombie = Instantiate(prefab, transform.position, Quaternion.identity);
        ZC = zombie.GetComponent<ZombieController>();
    }

    public IEnumerator Instantiate()
    {
        yield return new WaitForSeconds(10);
        zombie = Instantiate(prefab, transform.position, transform.rotation);
        StopCoroutine("instantiateZombie"); 
    }

    /*private void Update()
    {
        if (ZC._asleep)
        {
            instantiateZombie = StartCoroutine("Instantiate");          
        }
    }*/

    public void LargeSpawn()
    {
        var spawnTime = 5;
        Invoke("Instantiate", spawnTime);
    }

}
