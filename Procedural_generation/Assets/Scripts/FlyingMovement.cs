using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMovement : MonoBehaviour
{
    private float gravity;
    private Rigidbody2D rb;
    private Vector2 startPos;
    float ang = 45;
    public float _speed;
    public float flyForce; 
    public bool dead;
    public GameObject EndUI; 
    private GameplayManager gameplayManager;

    public static FlyingMovement Instance { get; private set; }
    
    public ParticleSystem ps;
    private ParticleSystem.EmissionModule emission;
    public ParticleSystem flames;
    private ParticleSystem.EmissionModule flamesEmission;
    public AudioSource AudioManager;
    
    void Start()
    {
        Instance = this;
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        gravity = rb.gravityScale;
        emission = ps.emission;
        flamesEmission = flames.emission;
        AudioManager = GetComponent<AudioSource>();
        gameplayManager = GameObject.FindObjectOfType<GameplayManager>();
    }

    void Update()
    {
        if (dead) return;
        Vector2 vel = rb.velocity;
        float anf = Mathf.Atan2(vel.y, x: 1250) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(x: 0, y: 0, z: ang-135));
        rb.AddForce(Vector3.right * _speed);
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * gravity * Time.deltaTime * flyForce);
            flamesEmission.enabled = true;
            
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            flamesEmission.enabled = false;
        }
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Death")
        {
            AudioManager.Play();
            dead = true;
            rb.AddForce(Vector2.right * 1800);
            rb.AddTorque(400);
            emission.enabled = false;
            EndUI.SetActive(true);
            gameplayManager.gameOver = true; 
        }  
    }
    public void Restart()
    {
        transform.position = startPos;
        dead = false;
    }
}
