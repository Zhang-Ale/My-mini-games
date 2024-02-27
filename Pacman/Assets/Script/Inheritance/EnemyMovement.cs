using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class EnemyMovement : Subject
{
    GameObject player;
    NavMeshAgent enemy;
    public GameObject part, PUparticle;
    EnemyHealth EH;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        poweredUp = false;
        enemy = gameObject.GetComponent<NavMeshAgent>();
        menu = GameObject.FindGameObjectWithTag("Menu").GetComponent<Menu>();
        pus = GameObject.FindGameObjectWithTag("PUS").GetComponent<PUSpawner>();
        EH = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (menu.gameStarted && !EH.notified)
        {
            enemy.speed = 30; 
            enemy.SetDestination(player.transform.position);
        }
        else
        {
            enemy.speed = 0;
            enemy.SetDestination(this.gameObject.transform.position);
        }

        
        if(GetComponent<Rigidbody>().velocity.magnitude != 0)
        {
            InstantiateParticle(part);
        }

        if (poweredUp)
        {
            StartCoroutine(PowerUpTime());
        }
        else
        {
            StopCoroutine(PowerUpTime());
        }   
    }

    IEnumerator PowerUpTime()
    {
        InstantiateParticle(PUparticle);
        yield return new WaitForSeconds(powerUpDuration);
        poweredUp = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PowerUp")
        {
            poweredUp = true;
            Destroy(other.gameObject);
            pus.Invoke("Spawn", 5); 
        }
    }
}
