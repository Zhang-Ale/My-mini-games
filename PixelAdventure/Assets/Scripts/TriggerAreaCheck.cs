using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAreaCheck : MonoBehaviour
{
    private TriggerEnemy EnemyParent;
    public GameObject BattleMusic;
    public GameObject BackMusic;

    private void Awake()
    {
        EnemyParent = GetComponentInParent<TriggerEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            EnemyParent.target = collision.transform;
            EnemyParent.inRange = true;
            BattleMusic.SetActive(true);
            BackMusic.SetActive(false);
            EnemyParent.hotZone.SetActive(true);
        }
    }
}
