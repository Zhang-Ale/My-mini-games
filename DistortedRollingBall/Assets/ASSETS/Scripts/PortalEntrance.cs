using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEntrance : MonoBehaviour
{
    public Transform PortalExit;
    public GameObject Player;

    private void OnTriggerEnter(Collider other)
    {
        Player.transform.position = PortalExit.transform.position;
    }
}
