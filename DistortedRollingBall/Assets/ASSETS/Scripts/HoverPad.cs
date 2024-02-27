using UnityEngine;
using System.Collections;

public class WindPad : MonoBehaviour
{
    public float WindForce;

    private Rigidbody rb;


    void OnTriggerStay(Collider other)
    {
        rb = other.GetComponent<Rigidbody>();

        rb.AddForce(Vector3.up * WindForce, ForceMode.Acceleration);


    }
}