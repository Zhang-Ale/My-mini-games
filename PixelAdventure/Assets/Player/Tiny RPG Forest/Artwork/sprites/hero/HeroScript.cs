using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroScript : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer spriteRen;
    public float walkSpeed;
    public float jumpStrength;

    void Start()
    {
        spriteRen = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow)) 
        {
            spriteRen.flipX = false;
            transform.Translate(walkSpeed * Time.deltaTime, 0, 0);
            anim.SetBool("facingSide", true);
            anim.SetBool("facingBack", false);
            anim.SetBool("facingFront", false);
            anim.SetBool("Walking", true);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            anim.SetBool("Walking", false);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            spriteRen.flipX = true;
            transform.Translate(-walkSpeed * Time.deltaTime, 0, 0);
            anim.SetBool("facingSide", true);
            anim.SetBool("facingBack", false);
            anim.SetBool("facingFront", false);
            anim.SetBool("Walking", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            anim.SetBool("Walking", false);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0, jumpStrength * Time.deltaTime, 0);
            anim.SetBool("facingBack", true);
            anim.SetBool("facingSide", false);
            anim.SetBool("facingFront", false);
            anim.SetBool("Walking", true);
            spriteRen.flipX = false;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            anim.SetBool("Walking", false);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0, -walkSpeed * Time.deltaTime, 0);
            anim.SetBool("facingFront", true);
            anim.SetBool("facingBack", false);
            anim.SetBool("facingSide", false);
            anim.SetBool("Walking", true);
            spriteRen.flipX = false;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            anim.SetBool("Walking", false);
        }

        if (Input.GetMouseButtonDown(0)) 
        {
            anim.SetTrigger("Attacking");
        }
    }
}
