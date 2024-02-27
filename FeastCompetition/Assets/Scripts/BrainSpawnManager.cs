using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainSpawnManager : MonoBehaviour
{
    float xRandomPos,zRandomPos;
    GameObject brain;
    public GameObject brainPrefab;

    void Start()
    {
        xRandomPos = Random.Range(+10, -10);
        zRandomPos = Random.Range(8, -8);

        Vector3 areaVector = new Vector3(this.transform.position.x + xRandomPos, this.transform.position.y, this.transform.position.z + zRandomPos);
        brain = Instantiate(brainPrefab, areaVector, transform.rotation);
    }

    public IEnumerator SpawnOneBrain()
    {
        yield return new WaitForSeconds(5);
        xRandomPos = Random.Range(+10, -10);
        zRandomPos = Random.Range(8, -8);
        Vector3 areaVector = new Vector3(this.transform.position.x + xRandomPos, this.transform.position.y, this.transform.position.z + zRandomPos);
        brain = Instantiate(brainPrefab, areaVector, transform.rotation);
    }
}
