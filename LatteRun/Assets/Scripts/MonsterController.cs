using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MonsterController : MonoBehaviour
{
    Rigidbody monsRb;
    public Transform monsTrans, playTrans;
    public int monsSpeed;
    public bool beginCheck;
    public bool stopCheck;
    public GameObject face;
    public GameObject part1, part2; 
    public GameObject thisName;
    public TextMeshProUGUI dialogue, epilogue;
    public GameObject ambienceMusic;
    public GameObject chaseMusic;
    public Collectable C;
    float counter = 5; 

    void Start()
    {
        ambienceMusic.SetActive(true);
        beginCheck = false;

        monsRb = this.GetComponent<Rigidbody>();
        stopCheck = false; 
    }

    void FixedUpdate()
    {
        if (beginCheck)
        {   
            monsRb.velocity = transform.forward * monsSpeed * Time.deltaTime;
            part1.SetActive(true);
            part2.SetActive(true);
            MeshRenderer MR = GetComponent<MeshRenderer>();
            MR.enabled = true;
            CapsuleCollider CapsuleC = GetComponent<CapsuleCollider>();
            CapsuleC.enabled = true; 
            face.SetActive(true);
            thisName.SetActive(true);
        }

        if (C.gameEnd)
        {
            if (Input.GetKeyDown("r"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
    private void Update()
    {
        if (beginCheck) 
        {
            chaseMusic.SetActive(true);
            monsRb.constraints = RigidbodyConstraints.None;
            monsTrans.LookAt(playTrans);
            epilogue.text = ""; 
            dialogue.text = "Starbucks King: 'How dare you take my Latte! You will die!'";
            counter -= Time.deltaTime; 
            if(counter <= 0f)
            {
                dialogue.text = ""; 
            }
        }
        else
        {
            monsRb.constraints = RigidbodyConstraints.FreezeAll;
            if(stopCheck) 
            {
                monsRb.constraints = RigidbodyConstraints.None;
                monsRb.velocity = transform.forward * monsSpeed * Time.deltaTime;
                StartCoroutine(Disappear());
            }
        }
    }

    public IEnumerator StopChecking() 
    {        
        yield return new WaitForSeconds(1);
        beginCheck = false;
        stopCheck = true;
        yield return new WaitForSeconds(2);
        dialogue.text = "Starbucks King: 'You will never escape from the Starbucks King... one day... I'll get you...'";
        yield return new WaitForSeconds(3);
        dialogue.text = ""; 
    }
    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(5); 
        MeshRenderer MR = GetComponent<MeshRenderer>();
        MR.enabled = false;
        CapsuleCollider CapsuleC = GetComponent<CapsuleCollider>();
        CapsuleC.enabled = false;
        face.SetActive(false);
        thisName.SetActive(false);
        part1.SetActive(false);
        part2.SetActive(false);
    }
}
