using UnityEngine;

public class HardFriend : People
{
    public static float stayTimeValueMin = 25.0f;
    public static float stayTimeValueMax = 40.0f;
    public static float multiplier = 1.2f;

    HardFriend(Vector3 strangerLocation, float randomValue)
    {
        this.location = strangerLocation;
        this.stayTimeValue = randomValue;
    }

    HardFriend(float randomValue)
    {
        this.stayTimeValue = randomValue;
    }

    HardFriend()
    {
        this.stayTimeValue = 30.0f; //Temporary solution
    }

    private void Update()
    {
        Transform playerTrans = GameObject.Find("FollowPosHard").transform;
        //SetParent(playerTrans); 
    }
}