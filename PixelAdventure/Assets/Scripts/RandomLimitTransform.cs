using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLimitTransform : MonoBehaviour
{
    public Transform thisTrans;
    float thisX; 
    public float initialX;
    bool _changed;
    public bool left;

    void Start()
    {
        thisTrans = this.GetComponent<Transform>();
        _changed = false; 
    }

    void Update()
    {
        if (!_changed) 
        {
            if (left) 
            {
                StartCoroutine(RandomLeftPos());
            }
            else 
            {
                StartCoroutine(RandomRightPos());
            }
        }     
        
        thisX = thisTrans.position.x; 
        if (thisX > initialX - 5 || thisX < initialX + 5) 
        {
            thisX = initialX; 
        }
    }
    IEnumerator RandomLeftPos() 
    {
        _changed = true;
        thisTrans.position = new Vector3(Random.Range(initialX - 1f, initialX + 1f), 1.61f, 0);
        yield return new WaitForSeconds(4f);
        _changed = false;
    }

    IEnumerator RandomRightPos() 
    {
        _changed = true;
        yield return new WaitForSeconds(2f);
        thisTrans.position = new Vector3(Random.Range(initialX - 0.5f, initialX + 0.5f), 1.61f, 0);
        yield return new WaitForSeconds(4f);
        _changed = false;
    }
}
