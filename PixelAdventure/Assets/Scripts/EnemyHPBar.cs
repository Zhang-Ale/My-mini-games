using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    public Slider Slider;
    public Color Low;
    public Color High;
    public Vector3 flippedOffset;
    public Vector3 normalOffset;
    public GameObject EnemyParent;
    TriggerEnemy TE; 

    public void Start()
    {
        TE = EnemyParent.GetComponent<TriggerEnemy>();
    }

    public void SetHealth(float health, float maxHealth)
    {
        //the line below is to activate HP bar if attacked
        //Slider.gameObject.SetActive(health < maxHealth);
        Slider.value = health;
        Slider.maxValue = maxHealth;

        Slider.GetComponentInChildren<Image>().color = Color.Lerp(Low, High, Slider.normalizedValue);
    }

    void Update()
    {
            if (TE.flipped)
            {
                Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + normalOffset);
            }
            else
            {
                Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + flippedOffset);
            }
    }
}
