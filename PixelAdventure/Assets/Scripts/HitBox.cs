using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HitBox : MonoBehaviour
{
    public bool isLeft = false;
    public GameObject _hitBox;
    public Transform LeftSide;
    public Transform RightSide;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && !isLeft)
        {
            isLeft = true;
            _hitBox.transform.position = LeftSide.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.D) && isLeft)
        {
            isLeft = false;
            _hitBox.transform.position = RightSide.transform.position;
        }

    }
}
