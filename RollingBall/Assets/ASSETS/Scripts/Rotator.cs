using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rotator : MonoBehaviour
{
    public float RotationOnX = 0f;
    public float RotationOnY = 0f;
    public float RotationOnZ = 0f;


    // Before rendering each frame..
    void Update()
    {
        // Rotate the game object that this script is attached to by RotationOnX in the X axis,
        // RotationOnY in the Y axis and RotationOnZ in the Z axis, multiplied by deltaTime in order to make it per second
        // rather than per frame.
        transform.Rotate(new Vector3(RotationOnX, RotationOnY, RotationOnZ) * Time.deltaTime);
    }
}