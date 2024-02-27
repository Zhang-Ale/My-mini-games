using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHealth : MonoBehaviour
{
    public int curHealth = 0;
    public int maxHealth = 100;
    PlayerHealthBar healthBar;
    public PlayerScript PC;
    public int blinks;
    public float flashTime;
    private SpriteRenderer sr;
    public GameObject deathParticles;
    bool isDead; 
    public Shake shake;
    public ScreenFlash sf;
    Animator anim; 
    void Start()
    {
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<PlayerHealthBar>();
        curHealth = 1000;
        anim = GetComponent<Animator>(); 
        sr = GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        Death();
    }

    public void DamagePlayer(int damage)
    {
        sf.FlashScreen();

        curHealth -= damage;
        healthBar.SetHealth(curHealth);

        BlinkPlayer(blinks, flashTime);
        shake.StartCoroutine("DamagedShaking");
    }
    public void HealBack() 
    {
        curHealth = maxHealth;
        healthBar.SetHealth(curHealth);
    }

    void BlinkPlayer(int numBlinks, float seconds)
    {
        if (!isDead) 
        {
            StartCoroutine(DoBlinks(numBlinks, seconds));
        }
    }

    IEnumerator DoBlinks(int numBlinks, float seconds)
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < numBlinks * 2; i++)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(seconds);
            sr.enabled = true;
        }
    }

    public void Death()
    {
        if (curHealth == 0 || curHealth <= 0)
        {
            if (0 == 0)
            {
                isDead = true;
                anim.SetTrigger("Dead"); 
                StartCoroutine(Respawn());
            }
        }
    }
    IEnumerator Respawn()
    {
        BoxCollider2D BX = GetComponent<BoxCollider2D>();
        BX.enabled = false;
        Instantiate(deathParticles, transform.position, transform.rotation);
        yield return new WaitForSeconds(2); 
        transform.position = PC.respawnPoint;
        curHealth = maxHealth;
        healthBar.SetHealth(1000);
        isDead = false; 
        BX.enabled = true; 
    }

}
