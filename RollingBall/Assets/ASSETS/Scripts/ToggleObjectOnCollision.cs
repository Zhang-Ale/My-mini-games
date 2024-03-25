using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObjectOnCollision : MonoBehaviour
{

    public GameObject ObjectToToggle;
    public string CollisionTag;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    void OnCollisionEnter(Collision other)
    {
        // If the game object we collide with has the tag we choose assigned to it..
        if (other.gameObject.CompareTag(CollisionTag))
        {
            // Toggle the other game object 
            ObjectToToggle.SetActive(!ObjectToToggle.activeInHierarchy);

        }
    }
}