using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public Vector3 pos;
    private void Start()
    {
        
    }
    void Update()
    {
        transform.LookAt(player.transform);
        Vector3 targetPosition = player.transform.position + pos;
        transform.position = targetPosition;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
