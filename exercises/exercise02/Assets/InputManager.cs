using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class InputManager : MonoBehaviour
{
    public static InputManager Singleton;
    //public float TestFloat = 10;
    /*[Header("SteamVR Reference")]
    public Hand LeftHand;
    public Hand RightHand;
    [Space(20)]
    public SteamVR_Action_Boolean ShootAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Shoot");
    //public SteamVR_Action_Boolean GrabAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Grab");*/
    [Header("Input Events")]
    public UnityEvent ShootEvent;
    public UnityEvent GrabEvent;
    public UnityEvent ThrowEvent;

    [Header("Input Bools")]
    public bool GrabBool;

    void OnEnable()
    {
        Singleton = this;
    } 

    void Update()
    { 
        ShootInvoke();
        ThrowInvoke();
    }

    void ShootInvoke()
    {
        if (Input.GetMouseButton(1) /*|| ShootAction.GetStateDown(RightHand.handType)*/)
        {
            ShootEvent.Invoke();
        }
    }
    void ThrowInvoke()
    { 
        if (Input.GetMouseButton(2))
        {
            ThrowEvent.Invoke();
            GrabBool = false;
        }
    }
}
