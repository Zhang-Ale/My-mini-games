using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerVictoryConditions : MonoBehaviour
{

    public string NextLevelName;
    public string TriggerTagToWin = "Flag";
    public GameObject ObjectToSpawnOnVictory;
    public float TimeToWaitUntilLoad = 5f;

    public string TriggerTagToDie = "Kill";
    public GameObject ObjectToSpawnOnDeath;
    public float TimeToWaitUntilReload = 5f;

    // Create private references to the rigidbody component on the player
    private Rigidbody rb;

    // At the start of the game..
    void Start()
    {
        // Assign the Rigidbody component to our private rb variable
        rb = GetComponent<Rigidbody>();
    }

    // When this game object intersects a collider with 'is trigger' checked, 
    // store a reference to that collider in a variable named 'other'..
    void OnTriggerEnter (Collider other)
    {
        // ..and if the game object we intersect has the tag 'Flag' assigned to it, you will complete the level
        if (other.gameObject.CompareTag(TriggerTagToWin))
        {
            if (ObjectToSpawnOnVictory != null)
            {
                Instantiate(ObjectToSpawnOnVictory, transform.position, transform.rotation);
            }

            StartCoroutine("LoadLevel");
        }

        // ..and if the game object we intersect has the tag 'Kill' assigned to it, the level is gonna restart
        if (other.gameObject.CompareTag(TriggerTagToDie))
        {
            if (ObjectToSpawnOnDeath != null)
            {
                Instantiate(ObjectToSpawnOnDeath, transform.position, transform.rotation);
            }


            StartCoroutine("ReloadLevel");
        }

    }

    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(TimeToWaitUntilLoad);
        SceneManager.LoadScene(NextLevelName);
    }

    IEnumerator ReloadLevel()
    {
        yield return new WaitForSeconds(TimeToWaitUntilReload);
        Application.LoadLevel(Application.loadedLevel);
    }
}


