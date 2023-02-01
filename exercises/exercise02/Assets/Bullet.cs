
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{  
    public float BulletExistingTime = 5f;
    GameObject obj;
    public static int bulletCount;

    private void Awake()
    {
        obj = GameObject.FindGameObjectWithTag("ShootPoint");
    }
    void Update()
    {
        if (obj != null) 
        {
            if (this.transform.position != obj.transform.position)
            {
                Destroy(this.gameObject, BulletExistingTime);
            }
        }
        bulletCount++;
    }
}
