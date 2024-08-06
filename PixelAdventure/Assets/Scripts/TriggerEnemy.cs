using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnemy : MonoBehaviour
{
    #region Public Variables
    public float attackDistance;
    public float moveSpeed;
    public float timer;
    public bool inRange; 
    public Transform player;
    public Transform leftLimit;
    public Transform rightLimit;
    [HideInInspector] public Transform target;
    public GameObject hotZone;
    public GameObject triggerArea;
    public int maxHealth = 50;
    public int currentHealth;
    public GameObject BattleMusic;
    public GameObject BackMusic;
    public Transform attackHitBox;
    public float attackRange = 1f;
    public LayerMask enemyLayers;
    public EnemyHPBar HealthBar;
    public bool flipped = false;
    public bool attackMode;
    public HotZoneCheck HC;
    public PlayerHealth PH;
    public bool _kill; 
    public GameObject leftBloodEffect;
    public GameObject rightBloodEffect; 
    #endregion

    #region Private Variables
    private Animator anim;
    private bool coroutineStarted = false;
    private float distance; 
    public float playerDistance;
    SpriteRenderer SR;
    private Color originalColor;
    [SerializeField]
    bool EnemyIsDamaged;
    [SerializeField]
    bool playerIsDamaged;
    [SerializeField]
    private bool cooling; 
    private float intTimer;
    
    #endregion

    private void Awake()
    {
        SelectTarget();
        intTimer = timer; 
        anim = GetComponent<Animator>();
        SR = GetComponent<SpriteRenderer>();
        originalColor = SR.color;
        currentHealth = maxHealth;
        HealthBar.SetHealth(currentHealth, maxHealth);
        flipped = false;
        _kill = false; 
    }

    void FixedUpdate()
    {
        HealthBar.SetHealth(currentHealth, maxHealth);
        playerDistance = Vector2.Distance(transform.position, player.position);
        if (!EnemyIsDamaged && !attackMode)
        { 
            Move();
            if (!coroutineStarted)
            {
                StartCoroutine(IdleAndMove());
            }
        }
        else
        {
            StopCoroutine(IdleAndMove());
        }

        if (!InsideofLimits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            BattleMusic.SetActive(false);
            BackMusic.SetActive(true);
            SelectTarget();
        }

        if (inRange && !cooling)
        {
            StartCoroutine(EnemyLogic());
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        if (playerDistance >= 35)
        {
            currentHealth = 100;
            playerDistance = 0;
        }
        if(_kill) { _kill = false;  }
    }

    IEnumerator IdleAndMove()
    {        
        if (!inRange)
        {
            moveSpeed = 2f;
            yield return new WaitForSeconds(2);
            coroutineStarted = true;
            yield return new WaitForSeconds(2);
            moveSpeed = 0f;
            yield return new WaitForSeconds(2);
            coroutineStarted = false;           
        }
    }

    IEnumerator ControlMove()
    {
        EnemyIsDamaged = true;
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(2f);
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        cooling = false;
        EnemyIsDamaged = false;
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(ControlMove());
        currentHealth -= damage;
        StartCoroutine(FlashColor(0.2f));
        if (!flipped) 
        {
            Instantiate(rightBloodEffect, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(leftBloodEffect, transform.position, Quaternion.identity);
        }
    }

    IEnumerator FlashColor(float time)
    {
        yield return new WaitForSeconds(0.75f);
        SR.color = Color.red;
        Invoke("ResetColor", time);
        yield return new WaitForSeconds(0.65f);
        SR.color = Color.red;
        Invoke("ResetColor", time);
    }

    private void ResetColor()
    {
        SR.color = originalColor; 
    }

    void Die()
    {
        _kill = true; 
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Destroy(this.gameObject, 2f);
        BattleMusic.SetActive(false);
        BackMusic.SetActive(true); 
    }

    IEnumerator EnemyLogic()
    {
        moveSpeed = 5f;
        distance = Vector2.Distance(transform.position, target.position);

        if (attackDistance > distance && !playerIsDamaged && !cooling && !EnemyIsDamaged && HC._inRange && PH.curHealth>0)
        {
            playerIsDamaged = true;
            Attack();
            yield return new WaitForSeconds(1f);
            playerIsDamaged = false;
        }

        if (cooling)
        {
            Cooldown();
            yield return new WaitForSeconds(0.5f);
            cooling = false;
            attackMode = false;
        }
    }

    void Move()
    {
        if (!coroutineStarted)
        {
            anim.SetBool("Walk", true);
        }
        else 
        {
            anim.SetBool("Walk", false);
        }

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (!flipped)
            {
                Vector2 targetPosition = new Vector2(target.position.x - 1f, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
            else
            {
                Vector2 targetPosition = new Vector2(target.position.x + 1f, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
        }
    }

    void Attack()
    {
        timer = intTimer; 
        attackMode = true; 

        anim.SetBool("Walk", false);
        anim.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackHitBox.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (!cooling && enemy.GetComponent<PlayerHealth>() == true)
            {
                enemy.GetComponent<PlayerHealth>().DamagePlayer(10);
            }
        }
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.5 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    public void TriggerCooling()
    {
        cooling = true;
    }

    private bool InsideofLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    public void SelectTarget()
    {
        float distanceToLeft = Vector3.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector3.Distance(transform.position, rightLimit.position);
        if (distanceToLeft > distanceToRight)
        {
            target = leftLimit;
            flipped = true;
        }
        else
        {
            target = rightLimit;
            flipped = false;
        }

        coroutineStarted = false;

        StartCoroutine(Flipped());
    }

    public void Flip() //In HotZone.cs
    {
        Vector3 rotation = transform.eulerAngles;
        if (!EnemyIsDamaged)
        {
            if (transform.position.x > target.position.x)
            {
                rotation.y = 180;
                flipped = true;
            }
            else
            {
                rotation.y = 0;
                flipped = false;
            }
            transform.eulerAngles = rotation;
        }
    }

    IEnumerator Flipped()
    {
        Vector3 rotation = transform.eulerAngles;
        if (!EnemyIsDamaged)
        {
            if (transform.position.x > target.position.x)
            {
                rotation.y = 180;
                flipped = true;
            }
            else
            {
                rotation.y = 0;
                flipped = false;
            }
            transform.eulerAngles = rotation;
            yield return new WaitForSeconds(2f);
        }
    }

}
