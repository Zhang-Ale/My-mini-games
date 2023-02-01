using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject BulletPrefab;
    [Space(20)]
    public Transform ShootPoint;
    public float ShootForce = 1000;

    void Awake()
    {
        gameObject.SetActive(true);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1) /*|| ShootAction.GetStateDown(RightHand.handType)*/)
        {
            Shoot();
        }
    }
    public void Shoot()
    {
        Debug.Log("Shoot!"); 
        Rigidbody tempBulletRigidbody = Instantiate(BulletPrefab, ShootPoint.position, ShootPoint.rotation).GetComponent<Rigidbody>();
        tempBulletRigidbody.AddForce(ShootPoint.forward * ShootForce);
    }

    IEnumerator countBullets(GameObject b)
    {
        int x = Bullet.bulletCount;
        //print(b.name + " has shot " + x + " at " + Time.time);
        //yield return new WaitForSeconds(waitTime);
        yield return new WaitForSeconds(5f);
        StartCoroutine(countBullets(gameObject));
    }

}
