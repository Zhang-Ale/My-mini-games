using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerScript : MonoBehaviour
{
    [Header("Controller")] 
    public float walkSpeed;
    public float jumpStrength;
    [SerializeField]
    bool isGrounded;
    private bool canJump;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    Rigidbody2D rb;
    public SpriteRenderer spriteRen;
    public Animator anim;
    public AudioSource AS1;
    public AudioSource AS2; 
    [Header("GameObjects")]
    public Vector3 respawnPoint;
    public GameObject fallDetector;
    public GameObject dust;
    public Transform dustPos;
    bool isDustCreated;
    public GameObject RegenerationPart;
    [SerializeField] private int facingDirection = 1;
    private bool isFacingRight = true;
    [Header("DASH")]
    public float dashSpeed;
    public float dashTime;
    [SerializeField] private bool _isDashing;
    public float distanceBetweenImages;
    public float dashCooldown;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;
    public bool flipped = false;
    public PlayerHealth PH;
    bool fallOnce;
    public MenuLoad ML;
    public GameObject BS;
    [Header("Events")]
    [Space]
    public UnityEvent OnLandEvent;

    private void Awake()
    {
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    void Start()
    {
        spriteRen = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        respawnPoint = transform.position;
        fallOnce = false; 
    }

    void Update()
    {     
        CheckDash();
        CheckIfCanJump();
        Jump();
        
        if (Input.GetKey(KeyCode.D))
        {
            if (isGrounded ) 
            {
                anim.SetBool("Walk", true);
            }
            else
            {
                anim.SetBool("Walk", false);
            }
            transform.Translate(walkSpeed * Time.deltaTime, 0, 0);
            spriteRen.flipX = false;
            flipped = false;
            isFacingRight = true; 
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("Walk", false);
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (isGrounded )
            {
                anim.SetBool("Walk", true);
            }
            else
            {
                anim.SetBool("Walk", false);
            }
            transform.Translate(-walkSpeed * Time.deltaTime, 0, 0);
            spriteRen.flipX = true;
            flipped = true;
            isFacingRight = false; 
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetBool("Walk", false);
        }

        if (Input.GetButtonDown("Dash"))
        {
            if (Time.time >= (lastDash + dashCooldown))
            {
                AttemptToDash();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack");
        }

        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }
    private void FixedUpdate()
    {
        CheckJump();
        SpawnDust();
    }

    void Jump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.AddForce(new Vector2(0, rb.velocity.y + jumpStrength));
            Instantiate(dust, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            anim.SetBool("Jump", true);
            anim.SetTrigger("Landed");
        }
    }
    private void CheckIfCanJump()
    {
        if ((isGrounded && rb.velocity.y == 0))
        {
            canJump = true;
        }       
    }

    public void OnLanding() 
    {
        anim.SetBool("Jump", false);
    }

    private void CheckDash()
    {
        if (_isDashing)
        {
            if (dashTimeLeft > 0)
            {
                if (flipped)
                {
                    transform.Translate(new Vector2(dashSpeed * facingDirection * -1, 0.0f));
                    dashTimeLeft -= Time.deltaTime;
                }
                else
                {
                    transform.Translate(new Vector2(dashSpeed * facingDirection, 0.0f));
                    dashTimeLeft -= Time.deltaTime;
                }

                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }
            if (dashTimeLeft <= 0)
            {
                _isDashing = false;
            }
        }
    }
    private void AttemptToDash()
    {
        _isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }
    private void SpawnDust()
    {
        if (isGrounded && !isDustCreated)
        {
            Instantiate(dust, new Vector2(dustPos.transform.position.x-0.3f, dustPos.transform.position.y -0.3f), dust.transform.rotation);
            isDustCreated = true;
        }
        if (!isGrounded)
        {
            isDustCreated = false;
        }
    }

    private void CheckJump()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;

        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }

    void FirstAttack()
    {
        AS1.Play();
    }
    void SecondAttack() 
    {
        AS2.Play(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "FallDetector")
        {
            transform.position = respawnPoint;
        }
        else if (collision.tag == "Checkpoint")
        {
            var HpParticle = Instantiate(RegenerationPart, rb.transform.position, Quaternion.identity);
            HpParticle.transform.parent = gameObject.transform;
            respawnPoint = transform.position;
            PH.HealBack();
        }

        if(collision.tag == "HitGround" && !fallOnce) 
        {
            PH.DamagePlayer(800);
            fallOnce = true; 
        }

        if(collision.name == "End") 
        {
            StartCoroutine(Ascension()); 
        }
    }

    IEnumerator Ascension() 
    {
        BS.SetActive(true);
        BS.GetComponent<Animator>().SetTrigger("End");
        yield return new WaitForSeconds(5); 
        ML.LoadSceneMenu();
    }
}
