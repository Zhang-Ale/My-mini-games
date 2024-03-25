using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject col1, col2, col3, col4;
    BoxCollider2D bc1, bc2, bc3, bc4; 
    private void Start()
    {
        bc1 = col1.GetComponent<BoxCollider2D>();
        bc2 = col2.GetComponent<BoxCollider2D>();
        bc3 = col3.GetComponent<BoxCollider2D>(); 
        bc4 = col4.GetComponent<BoxCollider2D>();
    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.S))
        {
            bc1.enabled = true;
            bc1.isTrigger = true;
        }
        else
        {
            bc1.enabled = false;
            bc1.isTrigger = false;
        }

        if (Input.GetKey(KeyCode.D))
        {
            bc2.enabled = true;
            bc2.isTrigger = true;
        }
        else
        {
            bc2.enabled = false;
            bc2.isTrigger = false;
        }

        if (Input.GetKey(KeyCode.J))
        {
            bc3.enabled = true;
            bc3.isTrigger = true;
        }
        else
        {
            bc3.enabled = false;
            bc3.isTrigger = false;
        }

        if (Input.GetKey(KeyCode.K))
        {
            bc4.enabled = true;
            bc4.isTrigger = true;
        }
        else
        {
            bc4.enabled = false;
            bc4.isTrigger = false;
        }
    }
}
