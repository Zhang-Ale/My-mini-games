using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;
    public int CurrentHealth;

    private void Start()
    {
        enemyData = Resources.Load<EnemyData>("CubeEnemy");
        CurrentHealth = enemyData.MaxHealth;
        if (enemyData.EnemyPrefab != null)
        {
            Instantiate(enemyData.EnemyPrefab, transform.position, transform.rotation);
        }
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        // Check if enemy is dead
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Handle enemy death
        Destroy(gameObject);
    }
}