using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectStars : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    public GameObject OnCollectEffect;
    Score score; 

    void Update()
    {
        score = GameObject.Find("Canvas").GetComponent<Score>(); 
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Button"))
        {
            Instantiate(OnCollectEffect, transform.position, Quaternion.identity);
            score.Scored(); 
            _speed = 0; 
            Destroy(this.gameObject, 0.2f);
        }
    }
}
