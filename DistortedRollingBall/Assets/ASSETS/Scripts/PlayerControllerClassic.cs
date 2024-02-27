using UnityEngine;
using System.Collections;

public class PlayerControllerClassic : MonoBehaviour
{
	
	// Create public variables for player speed
	public float MovementSpeed;

	// Create private references to the rigidbody component on the player
	private Rigidbody rb;

	// At the start of the game..
	void Start ()
	{
		// Assign the Rigidbody component to our private rb variable
		rb = GetComponent<Rigidbody>();

	}

	// Each physics step..
	void FixedUpdate ()
	{
		// Set some local float variables equal to the value of our Horizontal and Vertical Inputs
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		// Create a Vector3 variable, and assign X and Z to feature our horizontal and vertical float variables above
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		// Add a physical force to our Player rigidbody using our 'movement' Vector3 above, 
		// multiplying it by 'speed' - our public player speed that appears in the inspector
		rb.AddForce (movement * MovementSpeed);
	}

	// When this game object intersects a collider with 'is trigger' checked, 
	// store a reference to that collider in a variable named 'other'..
	void OnTriggerEnter(Collider other) 
	{
		// ..and if the game object we intersect has the tag 'Flag' assigned to it, you will complete the level
		if (other.gameObject.CompareTag ("Flag"))
		{
            Application.LoadLevel(Application.loadedLevel);
        }

        // ..and if the game object we intersect has the tag 'Kill' assigned to it, the level is gonna restart
        if (other.gameObject.CompareTag("Kill"))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

}