using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float launchForce;
    Rigidbody rb;
    private float lastRotateDirectionSwitchTime = 0;
    float rotateSpeed = 0.5f;
    bool canRotate = true; 

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>(); 
    }

    void Update()
    {
        /*Vector3 newPosition = new Vector3(gameObject.transform.position.x, 
            gameObject.transform.position.y, 
            gameObject.transform.position.z);
        gameObject.transform.position = newPosition; */

        if (canRotate)
        {
            gameObject.transform.Rotate(0, rotateSpeed, 0);
        }

        if (Time.time - lastRotateDirectionSwitchTime > 1)
        {
            rotateSpeed = rotateSpeed * -1; 
            lastRotateDirectionSwitchTime = Time.time; 
        } 

        if (Input.GetKey(KeyCode.Space))
        {
            canRotate = false; 
            rb.useGravity = true; 
            transform.Translate(transform.forward * 0.001f); 
            rb.AddForce(transform.forward * launchForce);
        }      
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Ground")
        {
            col.gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
