using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LindensCamController : MonoBehaviour
{
    public GameObject player;
    private Vector3 camPosition;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        camPosition = new Vector3(player.transform.position.x, player.transform.position.y, -3);
        this.transform.position = camPosition;
    }
}
