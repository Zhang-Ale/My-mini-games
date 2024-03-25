using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour
{
    public float Lifetime;

    void Start()
    {
        Destroy(gameObject, Lifetime);
    }
}
