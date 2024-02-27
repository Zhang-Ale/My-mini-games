using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollectionNotifier : MonoBehaviour
{
    public static event Action OnXPCollected;

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LifeXPSphere" && OnXPCollected != null) // check if anyone subscribed to this event
        {
            OnXPCollected(); // run the event on all subscribers
        }

        Destroy(this.gameObject);
        Debug.Log("Object collected.");
    }
}