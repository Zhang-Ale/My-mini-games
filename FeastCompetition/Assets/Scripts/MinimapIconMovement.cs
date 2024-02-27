using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIconMovement : MonoBehaviour
{
    public GameObject minimapIcon;
    public GameObject CrossIcon;
    public bool changeIcon; 
    GameObject ownMinimap;
    GameObject crossMinimap; 
    public bool isThisZombie; 

    private void Start()
    {
        if (!isThisZombie)
        {
            GameObject BrainMinimap = Instantiate(minimapIcon, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.Euler(90, 0, 0));
            ownMinimap = BrainMinimap;
            ownMinimap.transform.parent = this.transform;
        }
        
        if(isThisZombie && !changeIcon)
        {
            GameObject ZombieMinimap = Instantiate(minimapIcon, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.Euler(90, 0, 0));
            ownMinimap = ZombieMinimap;
            
            GameObject CrossMinimap = Instantiate(CrossIcon, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.Euler(90, 0, 0));
            crossMinimap = CrossMinimap;
            crossMinimap.SetActive(false);
        }
    }

    void Update()
    {
        ownMinimap.transform.position = new Vector3(transform.position.x, minimapIcon.transform.position.y, transform.position.z);

        if (isThisZombie && changeIcon)
        {
            ownMinimap.SetActive(false);
            crossMinimap.SetActive(true);
            crossMinimap.transform.position = new Vector3(transform.position.x, CrossIcon.transform.position.y, transform.position.z);
        }

        if (isThisZombie && !changeIcon)
        {
            crossMinimap.SetActive(false); 
            ownMinimap.SetActive(true);
        }
    }
}
