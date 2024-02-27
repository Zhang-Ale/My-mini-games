using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinController : MonoBehaviour
{
    public int CoinsNeeded;
    public string CoinsCollisionTag;
    public GameObject ObjectToToggle;
    public Text countText;
    public Text winText;
    public bool canWin; 
    [SerializeField]
    private int count;

    void Start()
    {
        count = 0;
        SetCountText();
        winText.text = ""; 
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText ();
        }
        if (other.gameObject.tag == "Flag" && canWin)
        {
            winText.text = "You win!";
        }

    }

    void SetCountText()
    {
        countText.text = "Coins collected: " + count.ToString();

        if (count >= CoinsNeeded)
        {
            winText.text = "You've collected all the coins!";
            ObjectToToggle.SetActive(!ObjectToToggle.activeInHierarchy);
            canWin = true; 
        }
    }
}