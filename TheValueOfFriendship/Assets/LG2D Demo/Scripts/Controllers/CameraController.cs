using UnityEngine;

/// <summary>
/// A controller for the camera.  This is for demo purposes and can be extended/replaced.
/// </summary>
public class CameraController : MonoBehaviour {
    private GameObject player;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        if (!player)
        {
            Debug.LogError("Camera could not find player!");
        }
    }

    void Update()
    {
        FollowPlayer();
    }

    /// <summary>
    /// Follows the player
    /// </summary>
    private void FollowPlayer()
    {
        var playerPosition = player.transform.position;
        transform.position = new Vector3(playerPosition.x, playerPosition.y, transform.position.z);
    }
}
