using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObjectOnTriggerExit : MonoBehaviour
{

    public GameObject ObjectToToggle;
    public string TriggerTag;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnTriggerExit(Collider other)
    {
        // If the game object we collide with has the tag we choose assigned to it..
        if (other.gameObject.CompareTag(TriggerTag))
        {
            // Toggle the other game object 
            ObjectToToggle.SetActive(!ObjectToToggle.activeInHierarchy);

        }
    }
}
