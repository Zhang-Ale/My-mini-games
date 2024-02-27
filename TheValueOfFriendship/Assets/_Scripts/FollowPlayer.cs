using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    GameObject player;
    public GameObject stranger; 
    GameObject playerEasyPos;
    public bool ThisIsFriend;
    public bool isThisHard; 
    private void Start()
    {
        player = GameObject.Find("Player");

        playerEasyPos = GameObject.Find("FollowPosEasy");
    }
    void Update()
    {
        if (!ThisIsFriend && !isThisHard)
            transform.position = player.transform.position;

        if(ThisIsFriend && !isThisHard)
        {
            transform.position = playerEasyPos.transform.position;
            StartCoroutine(SelfDestroy());
        }
    }

    IEnumerator SelfDestroy()
    {
        float ran = Random.Range(3f, 8f);
        yield return new WaitForSeconds(ran);
        Vector3 addon = new Vector3(10, 10, 0); 
        Vector3 pos = transform.position + addon;
        Instantiate(stranger, pos, Quaternion.identity); 
        Destroy(this.gameObject);
    }
}
