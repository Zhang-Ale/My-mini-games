using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    private float yRotate = 1f; 
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            transform.Rotate(0, yRotate, 0, Space.Self);
        }
    }
}
