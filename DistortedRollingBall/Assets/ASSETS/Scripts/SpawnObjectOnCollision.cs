using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectOnCollision : MonoBehaviour
{
    public GameObject GameObjectToSpawn;
    public string CollisionTag;
    public float MinimumSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.relativeVelocity.magnitude > MinimumSpeed)
        {
            // If the game object we collide with has the tag we choose assigned to it..
            if (other.gameObject.CompareTag(CollisionTag))
            {

                Instantiate(GameObjectToSpawn, transform.position, transform.rotation);

            }
        }
    }
}
