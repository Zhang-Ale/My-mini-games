using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ObjectPool.Player; 

public class PlayerMovement : Observable
{
    [Header("General Setup Settings")]
    public GameObject particle, PUparticle, SPUparticle; 
    public bool playerDead;
    GameObject[] Enemies;
    GameObject[] PowerUps;
    public BulletGenerator BG;
    public ShootSystem SS;
    public BulletPool bulletPool;

    private void Start()
    {
        SetUp(); 
    }
    private void Awake()
    {
        IObserver gm = GameObject.FindObjectOfType<PlayerActions>();
        AddObserver(gm);
    }
    private void OnDisable()
    {
        IObserver gm = GameObject.FindObjectOfType<PlayerActions>();
        RemoveObserver(gm);
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

        if (shotgunPoweredUp)
        {
            SS.enabled = false; 
            BG.enabled = true; 
            StartCoroutine(ShotgunPowerUpTime());
        }
        else
        {
            for (int i = 0; i < 20; i++)
            {
                bulletPool.ReturnBullet();
            }
            BG.enabled = false;
            SS.enabled = true; 
            StopCoroutine(ShotgunPowerUpTime()); 
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

    IEnumerator ShotgunPowerUpTime()
    {
        InstantiateParticle(SPUparticle);
        yield return new WaitForSeconds(powerUpDuration);
        shotgunPoweredUp = false;
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
            Notify(this.gameObject, Action.OnPowerUpCollect);
            pus.Invoke("PUSpawn", 5);
            Destroy(other.gameObject); 
        }

        if (other.tag == "ShotgunPowerUp")
        {
            shotgunPoweredUp = true;
            Notify(this.gameObject, Action.OnShotgunPowerUpCollect);
            pus.Invoke("SPUSpawn", 5);   
            Destroy(other.gameObject);
        }
    }
}
