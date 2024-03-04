using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Collectable : Subject
{
    public string collectTag, enemyTag;
    public MonsterController MC;
    public ParticleSystem onCollectParticle;
    public GameObject coffeeImage;
    public GameObject countT;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI endText;
    public GameObject restartText;
    private float count;
    public int CoffeeNeeded;
    public bool gameEnd;
    public AudioSource _audioPlayer;
    public AudioClip _sprintingAudioClip;
    public GameObject block; 

    private void Start()
    {
        count = 0;
        countText.text = " collected: " + count.ToString();
        endText.text = " ";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            NotifyObservers(PlayerActions.Sprint);
        }

        if (gameEnd)
        {
            restartText.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FirstCollectable")
        {
            coffeeImage.SetActive(true);
        }

        if(other.gameObject.CompareTag(collectTag))
        {
            NotifyObservers(PlayerActions.Collect);
            countT.SetActive(true);
            count = count + 0.5f;
            SetCountText();
            Instantiate(onCollectParticle, other.gameObject.transform.position, Quaternion.identity);
            Destroy(other.gameObject, 0.5f);
            MC.beginCheck = true;
            MC.monsSpeed += 10; 
        }
        if (other.gameObject.CompareTag(enemyTag) && MC.beginCheck)
        {
            StartCoroutine(OnPlayerDestroy()); 
        }

        if(other.gameObject.name == "EscapeTrigger")
        {
            gameEnd = true;
            endText.text = "YOU ESCAPED";
            MC.StartCoroutine("StopChecking");
        }
    }

    IEnumerator OnPlayerDestroy()
    {
        _audioPlayer.clip = _sprintingAudioClip;
        _audioPlayer.Play();
        endText.text = "Oh no...";
        yield return new WaitForSeconds(1f);
        endText.text = "YOU ARE CAPTURED";
        gameEnd = true;
        Destroy(this.gameObject, 3f);
    }

    void SetCountText()
    {
        countText.text = " collected: " + count.ToString();
        if (count >= CoffeeNeeded)
        {
            block.SetActive(false);
            endText.text = "Quick! Escape from the window!";
        }
    }
}
