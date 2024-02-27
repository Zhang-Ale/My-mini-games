using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBox : MonoBehaviour
{
    [SerializeField] float distance = 1f;
    [SerializeField] LayerMask boxMask;
    GameObject box;
    public PlayerScript PS;
    private void Start()
    {
        PS = GetComponent<PlayerScript>(); 
    }
    void Update()
    {
        if (!PS.flipped)
        {
            CastRightRay();
        }
        else
        {
            CastLeftRay(); 
        }       
    }

    void CastLeftRay()
    { 
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left * transform.localScale.x, distance, boxMask);
        if (hit.collider != null && hit.collider.gameObject.tag == "Pushable" && Input.GetKey(KeyCode.E))
        {
            box = hit.collider.gameObject;
            box.transform.parent = this.transform; 
            box.GetComponent<FixedJoint2D>().enabled = true;
            box.GetComponent<BoxPull>().beingPushed = true;
            box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
        }
        else if (hit.collider != null && Input.GetKeyUp(KeyCode.E))
        {
            box.GetComponent<FixedJoint2D>().enabled = false;
            box.transform.parent = null;
            box.GetComponent<BoxPull>().beingPushed = false;
        }
    }

    void CastRightRay()
    { 
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance, boxMask);
        if (hit.collider != null && hit.collider.gameObject.tag == "Pushable" && Input.GetKey(KeyCode.E))
        {
            box = hit.collider.gameObject;
            box.transform.parent = this.transform;
            box.GetComponent<FixedJoint2D>().enabled = true;
            box.GetComponent<BoxPull>().beingPushed = true;
            box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
        }
        else if (hit.collider != null && Input.GetKeyUp(KeyCode.E))
        {
            box.GetComponent<FixedJoint2D>().enabled = false;
            box.transform.parent = null;
            box.GetComponent<BoxPull>().beingPushed = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (!PS.flipped)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * distance);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.left * transform.localScale.x * distance);
        }
    }
}
