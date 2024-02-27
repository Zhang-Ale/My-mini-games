using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    Slider healthBar;
    PlayerHealth playerHealth;

    private void Start()
    {
        healthBar = GetComponent<Slider>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        healthBar.maxValue = playerHealth.maxHealth;
        healthBar.value = playerHealth.curHealth;
    }

    public void SetHealth(int hp)
    {
        healthBar.value = hp;
    }
}
