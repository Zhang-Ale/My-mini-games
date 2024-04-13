using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.PopupText
{
    public class TriggerPopupText : MonoBehaviour
    {
        public PopupTextType[] possiblePopupTypes; // Array to hold possible PopupTextTypes
        private int consecutiveDamageCount = 0;
        private bool forceCriticalDamage = false;
        Enemy enemy; 

        private void Start()
        {
            enemy = gameObject.GetComponent<Enemy>(); // Try to get existing Enemy component
            if (enemy == null)
            {
                enemy = gameObject.AddComponent<Enemy>(); // Add Enemy component if not already present
            }
        }

        private void Update()
        {
            // Check consecutive damage count and set forceCriticalDamage flag
            if (consecutiveDamageCount >= 3)
            {
                forceCriticalDamage = true;
                consecutiveDamageCount = 0; // Reset the counter
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            int damageValue;
            if (collision.gameObject.CompareTag("Bullet"))
            {
                PopupTextType selectedPopupType;
                if (forceCriticalDamage)
                {
                    selectedPopupType = PopupTextType.CriticalDamage;
                    forceCriticalDamage = false; // Reset the flag
                    damageValue = enemy.enemyData.CriticalDamage;
                    enemy.TakeDamage(damageValue); // Take damage from the collision
                    PopupTextMan.ShowPopupText(selectedPopupType, transform.position, damageValue); // Show popup text with damage value
                }
                else
                {
                    float randomValue = Random.value; // Random value between 0.0 and 1.0

                    // Adjust the probabilities for "Damage" and "CriticalDamage"
                    if (randomValue < 0.7f) // 70% chance for "Damage"
                    {
                        selectedPopupType = PopupTextType.Damage;
                        consecutiveDamageCount++;
                        damageValue = enemy.enemyData.Damage;
                        enemy.TakeDamage(damageValue); // Take damage from the collision
                        PopupTextMan.ShowPopupText(selectedPopupType, transform.position, damageValue); // Show popup text with damage value
                    }
                    else // 30% chance for "CriticalDamage"
                    {
                        selectedPopupType = PopupTextType.CriticalDamage;
                        consecutiveDamageCount = 0;
                        damageValue = enemy.enemyData.CriticalDamage;
                        enemy.TakeDamage(damageValue); // Take damage from the collision
                        PopupTextMan.ShowPopupText(selectedPopupType, transform.position, damageValue); // Show popup text with damage value
                    }
                }

                
            }
        }
    }
}