using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimatorIntegerOnActivation : MonoBehaviour
{

    public Animator AnimatorToActivate;
    public string NameOfTheParameter;
    public int NumberToChange;
    // Start is called before the first frame update
    void Start()
    {
        AnimatorToActivate.SetInteger(NameOfTheParameter, NumberToChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
