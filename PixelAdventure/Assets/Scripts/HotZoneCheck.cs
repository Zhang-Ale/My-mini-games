using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotZoneCheck : MonoBehaviour
{
    private TriggerEnemy EnemyParent;
    public bool _inRange;
    private Animator anim;
    public GameObject text;
    public Vector3 flippedOffset;
    public Vector3 normalOffset;
    public bool showGuide = false;
    public GameObject BattleMusic;
    public GameObject BackMusic; 

    private void Awake()
    {
        EnemyParent = GetComponentInParent<TriggerEnemy>();
        anim = GetComponentInParent<Animator>();
        text.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (EnemyParent.inRange)
        {
            text.SetActive(true);
            if (EnemyParent.flipped)
            {
                text.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + flippedOffset);
            }
            else
            {
                text.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + normalOffset);
            }
        }
        
        if (_inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("CanAttack"))
        {
            EnemyParent.Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            _inRange = true;
            showGuide = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            _inRange = false;
            EnemyParent.attackMode = false; 
            BattleMusic.SetActive(false);
            BackMusic.SetActive(true);
            showGuide = false;
            text.SetActive(false);
            this.gameObject.SetActive(false);
            EnemyParent.triggerArea.SetActive(true);
            EnemyParent.inRange = false;
            EnemyParent.SelectTarget();
        }
    }
}
