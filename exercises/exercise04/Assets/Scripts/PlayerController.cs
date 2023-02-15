using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float rotateSpeed;
    public float forwardSpeed;
    public ParticleSystem leftSmoke, rightSmoke;
    public ParticleSystem explosion;
    public Transform spawnPoint;
    public GameObject restartText;
    public bool won; 
    CoinController CC; 
    float forwardForce = 500;
    float sidewaysForce = 100;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        CC = GetComponent<CoinController>();
        spawnPoint.position = transform.position;
        restartText.SetActive(false);
        won = false;
    }

    void Update()
    {
        if (CC.alive)
        {
            FlyControl();
        }
        else
        {
            OnDeath();
        }

        if (won)
        {
            Restart();
        }
    }

    void FlyControl()
    {
        if (Input.GetKey("w"))
        {
            transform.Translate(transform.forward * -forwardSpeed * Time.deltaTime, Space.World);
            //rb.AddForce(0, 0, forwardForce * Time.deltaTime);
            leftSmoke.Play();
            rightSmoke.Play(); 
        }

        if (Input.GetKey("d"))
        {
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.Self);
            //rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            leftSmoke.Play();
        }

        if (Input.GetKey("a"))
        {
            transform.Rotate(0, -rotateSpeed * Time.deltaTime, 0, Space.Self);
            //rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            rightSmoke.Play();
        }

        if (Input.GetKey(KeyCode.Space)) 
        { 
            rb.AddForce(0, 25, 0);
        }

        if (Input.GetKey("s")) 
        { 
            rb.AddForce(0, -10, 0); 
        }
    }

    void OnDeath()
    {
        rb.AddForce(0, 0, forwardForce * Time.deltaTime);
        rb.AddForce(0, -5, 0);
        explosion.Play();
        Restart(); 
    }

    public void Restart()
    {
        restartText.SetActive(true);
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}