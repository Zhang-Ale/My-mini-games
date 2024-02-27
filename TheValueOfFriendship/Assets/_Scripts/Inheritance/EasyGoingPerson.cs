using UnityEngine;

public class EasyGoingPerson : People
{
    public static float stayTimeValueMin = 10.0f;
    public static float stayTimeValueMax = 30.0f;
    public static float multiplier = 1.1f;

    EasyGoingPerson(Vector3 strangerLocation, float randomValue)
    {
        this.location = strangerLocation;
        this.stayTimeValue = randomValue;
    }

    EasyGoingPerson(float randomValue)
    {
        this.stayTimeValue = randomValue;
    }

    EasyGoingPerson()
    {
        this.stayTimeValue = 20.0f; //Temporary solution
    }

    private void Update()
    {
        Transform playerTrans = GameObject.Find("FollowPosEasy").transform;
        //SetParent(playerTrans);
    }
}