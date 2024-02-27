using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject : MonoBehaviour
{
    protected int moveSpeed = 30;
    [SerializeField]protected bool poweredUp;
    protected float powerUpDuration = 5; 
    protected Menu menu; 
    protected PUSpawner pus;

    protected void InstantiateParticle(GameObject part) 
    {
        Instantiate(part, transform.position, Quaternion.identity); 
    }

}
