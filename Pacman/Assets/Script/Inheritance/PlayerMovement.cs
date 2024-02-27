using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : Subject
{
    [Header("General Setup Settings")]
    private CharacterController CC;
    Rigidbody RB; 
    public GameObject particle, PUparticle; 
    public GameObject spawner;
    public bool playerDead;
    GameObject[] Enemies;
    GameObject[] PowerUps; 

    private void Start()
    {
        poweredUp = false; 
        menu = GameObject.FindGameObjectWithTag("Menu").GetComponent<Menu>();
        pus = GameObject.FindGameObjectWithTag("PUS").GetComponent<PUSpawner>();
        CC = GetComponent<CharacterController>();
        RB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (menu.gameStarted)
        {
            spawner.GetComponent<Spawner>().enabled = true;
            RB.constraints = RigidbodyConstraints.None;
            RB.constraints = RigidbodyConstraints.FreezePositionY;
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            Vector3 motion = move * Time.deltaTime * moveSpeed;
            CC.Move(motion);
        }

        if (GetComponent<Rigidbody>().velocity.magnitude != 0 && !playerDead)
        {
            InstantiateParticle(particle);
        }

        if (poweredUp)
        {
           StartCoroutine(PowerUpTime()); 
        }
        else
        {
            StopCoroutine(PowerUpTime());
        }

        if (playerDead)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            
            Enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in Enemies)
            {
                enemy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                enemy.GetComponent<NavMeshAgent>().speed = 0; 
            }

            PowerUps = GameObject.FindGameObjectsWithTag("PowerUp");
            foreach (GameObject powerUp in PowerUps)
            {
                powerUp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                powerUp.GetComponent<NavMeshAgent>().speed = 0;
            }
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
        if(other.tag == "Enemy")
        {
            if (!poweredUp)
            {
                playerDead = true;
                menu.gameStarted = false;
                Destroy(spawner);
                menu.StopGame(); 
            }
            else
            {
                playerDead = false;
                Destroy(other.gameObject);
                menu.AddPoint(); 
            }
        }

        if(other.tag == "PowerUp" && !playerDead)
        {
            poweredUp = true;
            pus.Invoke("Spawn", 5);
        }
    }
}
