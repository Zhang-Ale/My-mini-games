using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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
    public GameObject W, A, S, D, Space; 

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        CC = GetComponent<CoinController>();
        spawnPoint.position = transform.position;
        restartText.SetActive(false);
        won = false;
    }

    void FixedUpdate()
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
            transform.Translate(transform.forward * -forwardSpeed * Time.deltaTime, UnityEngine.Space.World);
            //rb.AddForce(0, 0, forwardForce * Time.deltaTime);
            leftSmoke.Play();
            rightSmoke.Play();
            W.GetComponent<CanvasGroup>().alpha = 1f;
        }
        else
        {
            W.GetComponent<CanvasGroup>().alpha = 0.7f;
        }

        if (Input.GetKey("d"))
        {
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0, UnityEngine.Space.Self);
            //rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            leftSmoke.Play();
            D.GetComponent<CanvasGroup>().alpha = 1f;
        }
        else
        {
            D.GetComponent<CanvasGroup>().alpha = 0.7f;
        }

        if (Input.GetKey("a"))
        {
            transform.Rotate(0, -rotateSpeed * Time.deltaTime, 0, UnityEngine.Space.Self);
            //rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            rightSmoke.Play();
            A.GetComponent<CanvasGroup>().alpha = 1f;
        }
        else
        {
            A.GetComponent<CanvasGroup>().alpha = 0.7f;
        }

        if (Input.GetKey(KeyCode.Space)) 
        { 
            rb.AddForce(0, 30, 0);
            Space.GetComponent<CanvasGroup>().alpha = 1f;
        }
        else
        {
            Space.GetComponent<CanvasGroup>().alpha = 0.7f;
        }

        if (Input.GetKey("s")) 
        { 
            rb.AddForce(0, -10, 0);
            S.GetComponent<CanvasGroup>().alpha = 1f;
        }
        else
        {
            S.GetComponent<CanvasGroup>().alpha = 0.7f;
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