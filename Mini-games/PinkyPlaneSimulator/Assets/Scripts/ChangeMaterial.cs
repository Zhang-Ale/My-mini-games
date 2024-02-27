using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    public Material newMat;
    public GameObject Coin; 

    void Update()
    {
        if (!Coin.activeInHierarchy)
        {
            Renderer[] obstacleRend = gameObject.GetComponentsInChildren<MeshRenderer>(); 
            foreach(Renderer rend in obstacleRend)
            {
                rend.material = newMat; 
            }

            Transform[] obstacle = gameObject.GetComponentsInChildren<Transform>();
            foreach (Transform obs in obstacle)
            {
                obs.tag = "Untagged";
            }
        }

    }
}
