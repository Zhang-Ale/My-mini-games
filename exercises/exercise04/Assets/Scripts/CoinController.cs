using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class CoinController : MonoBehaviour
{
    public int CoinsNeeded;
    public string CoinsCollisionTag;
    public string ObstaclesCollisionTag;
    //public GameObject ObjectToToggle;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI endText;
    public bool alive; 
    private int count;
    
    void Start()
    {
        count = 0;
        SetCountText();
        countText.text = "Coins collected: " + count.ToString();
        endText.text = " ";
        alive = true; 
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(CoinsCollisionTag))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText ();
        }

        if (other.gameObject.CompareTag(ObstaclesCollisionTag))
        {
            //change other.gameObject.material.color; 
            endText.text = "You Lose!";
            alive = false; 
        }
    }

    void SetCountText()
    {
        countText.text = "Coins Collected: " + count.ToString();
        if (count >= CoinsNeeded)
        {
            endText.text = "You Win!"; 
        }

        /*if (count >= CoinsNeeded)
        {
            ObjectToToggle.SetActive(!ObjectToToggle.activeInHierarchy);
        }*/
    }
}