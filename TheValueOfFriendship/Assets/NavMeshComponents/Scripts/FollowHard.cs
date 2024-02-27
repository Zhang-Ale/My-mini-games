using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHard : MonoBehaviour
{
    GameObject playerHardPos;
    public GameObject stranger;

    void Start()
    {
        playerHardPos = GameObject.Find("FollowPosHard");
    }

    void Update()
    {
        transform.position = playerHardPos.transform.position;
    }

    IEnumerator SelfDestroy()
    {
        float ran = Random.Range(10f, 15f);
        yield return new WaitForSeconds(ran);
        Vector3 addon = new Vector3(10, 10, 0);
        Vector3 pos = transform.position + addon;
        Instantiate(stranger, pos, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
