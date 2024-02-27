using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpeedOnTriggerStay : MonoBehaviour
{
    public GameObject Player;
    public string PlayerTag;
    public float SpeedInsideTrigger;
    private PlayerControllerClassic Controller;
    private float startSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Controller = Player.GetComponent<PlayerControllerClassic>();
        startSpeed = Controller.MovementSpeed;
    }


void OnTriggerStay(Collider other)
    {
        // If the game object we collide with has the tag we choose assigned to it..
        if (other.gameObject.CompareTag(PlayerTag))
        {
        Controller.MovementSpeed = SpeedInsideTrigger;

        }
    }


void OnTriggerExit(Collider other)
{
    // If the game object we collide with has the tag we choose assigned to it..
    if (other.gameObject.CompareTag(PlayerTag))
    {
        Controller.MovementSpeed = startSpeed;

    }
}
}