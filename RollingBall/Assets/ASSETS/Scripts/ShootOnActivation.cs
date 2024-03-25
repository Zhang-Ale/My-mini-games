using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootOnActivation : MonoBehaviour
{
    public Rigidbody Projectile;

    public float ProjectileSpeed = 20;



    // Use this for initialization
    void Start()
    {
        Rigidbody instantiatedProjectile = Instantiate(Projectile,
                                                      transform.position,
                                                      transform.rotation)
           as Rigidbody;

        instantiatedProjectile.velocity = transform.TransformDirection(new Vector3(0, 0, ProjectileSpeed));

    }

}
