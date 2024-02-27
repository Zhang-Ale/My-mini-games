using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    public bool isLeft = false;
    public GameObject HitBox;
    public Transform LeftSide;
    public Transform RightSide;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && !isLeft)
        {
            isLeft = true;
            HitBox.transform.position = LeftSide.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.D) && isLeft)
        {
            isLeft = false;
            HitBox.transform.position = RightSide.transform.position;
        }

    }
}
