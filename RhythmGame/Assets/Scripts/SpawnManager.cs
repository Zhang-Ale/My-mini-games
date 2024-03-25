using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Score score;
    public GameObject _note;
    public GameObject _container;
    int _case = 4;
    int randomTime; 

    void Start()
    {
        randomTime = Random.Range(3, 6); 
    }

    void Update()
    {
        if(score.gameStart)
        {
            StartCoroutine(SwitchCase()); 
        }

        if (score.gameOver)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator SwitchCase()
    {      
        //_case = (int)Mathf.Floor(Random.Range(1, 5));
        _case = Random.Range(1, 5); 
        yield return new WaitForSeconds(randomTime);
        Spawn();
    }

    void SpawnLineOne()
    {
        Vector3 posOneToSpawn = new Vector3(0.2f, 3.6f, 0);
        GameObject NoteOne = Instantiate(_note, posOneToSpawn, Quaternion.identity);
        NoteOne.transform.parent = _container.transform;
    }
    void SpawnLineTwo()
    {
        Vector3 posOneToSpawn = new Vector3(3.08f, 3.6f, 0);
        GameObject NoteOne = Instantiate(_note, posOneToSpawn, Quaternion.identity);
        NoteOne.transform.parent = _container.transform;
    }
    void SpawnLineThree()
    {
        Vector3 posOneToSpawn = new Vector3(6.16f, 3.6f, 0);
        GameObject NoteOne = Instantiate(_note, posOneToSpawn, Quaternion.identity);
        NoteOne.transform.parent = _container.transform;
    }
    void SpawnLineFour()
    {
        Vector3 posOneToSpawn = new Vector3(8.8f, 3.6f, 0);
        GameObject NoteOne = Instantiate(_note, posOneToSpawn, Quaternion.identity);
        NoteOne.transform.parent = _container.transform;
    }

    void Spawn()
    {
        switch (_case)
        {
            case 4:
                SpawnLineOne(); 
                break;
            case 3:
                SpawnLineTwo(); 
                break;
            case 2:
                SpawnLineThree();
                break;
           case 1:
                SpawnLineFour();
                break;                   
        }
        
    }
}
