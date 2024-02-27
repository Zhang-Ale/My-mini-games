using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowDist : MonoBehaviour
{
    public bool _chase;
    public GameObject enemy; 
    private void Start()
    {
        _chase = false; 
    }
    private void Update()
    {
        transform.position = enemy.GetComponent<Rigidbody2D>().transform.position;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            _chase = true; 
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            _chase = false;
        }
    }
}
