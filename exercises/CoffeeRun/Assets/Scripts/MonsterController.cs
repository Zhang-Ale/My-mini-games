using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    Rigidbody monsRb;
    public Transform monsTrans, playTrans, initialTrans;
    public int monsSpeed;
    public bool beginCheck;
    public bool stopCheck;
    public GameObject face;
    public GameObject part1, part2; 
    public GameObject thisName;
    public GameObject dialogue;
    public GameObject ambienceMusic;
    public GameObject chaseMusic; 

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

    }
    private void Update()
    {
        if (beginCheck) 
        {
            chaseMusic.SetActive(true);
            monsRb.constraints = RigidbodyConstraints.None;
            monsTrans.LookAt(playTrans); 
        }
        else
        {
            monsRb.constraints = RigidbodyConstraints.FreezeAll;
            if(stopCheck) 
            {
                monsRb.constraints = RigidbodyConstraints.None;
                monsRb.velocity = transform.forward * monsSpeed * Time.deltaTime;
                //monsTrans.LookAt(initialTrans);
                StartCoroutine(Disappear());
            }
        }
    }

    public IEnumerator StopChecking() 
    {        
        yield return new WaitForSeconds(3);
        beginCheck = false;
        yield return new WaitForSeconds(3);
        //dialogue.SetActive(false);
        yield return new WaitForSeconds(1);
        stopCheck = true;
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
    }
}
