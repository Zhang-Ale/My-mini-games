using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinController : MonoBehaviour
{
    public int CoinsNeeded;
    public string CoinsCollisionTag;
    public string ObstaclesCollisionTag;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI endText;
    public bool alive;
    public bool needCoins;
    public bool gameEnd; 
    public AudioSource AS;

    private int count;
    PlayerController PC; 

    void Start()
    {
        PC = GetComponent<PlayerController>();
        count = 0;
        SetCountText();
        countText.text = "Coins collected: " + count.ToString();
        endText.text = " ";
        alive = true; 
    }

    private void Update()
    {
        if (count <= CoinsNeeded)
        {
            needCoins = true; 
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(CoinsCollisionTag))
        {
            other.gameObject.SetActive(false);
            AS.Play();
            count = count + 1;
            SetCountText();
        }

        if (other.gameObject.CompareTag(ObstaclesCollisionTag))
        {
            OnLose();
            alive = false;
        }
    }

    void SetCountText()
    {
        countText.text = "Coins Collected: " + count.ToString();
        if (count >= CoinsNeeded)
        {
            gameEnd = true; 
            endText.text = "You Won!";
            PC.won = true;          
        }
    }

    public void OnLose()
    {
        endText.text = "You Lost!";
        PC.Restart();
    }
}