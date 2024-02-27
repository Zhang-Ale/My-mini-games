using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class TextFlickering : MonoBehaviour
{
    TextMeshProUGUI restartText;

    void Start()
    {
        restartText = GetComponent<TextMeshProUGUI>(); 
        StartCoroutine(Flickering());
    }

    IEnumerator Flickering()
    {
        while (true)
        {
            restartText.alpha = 255f;
            yield return new WaitForSeconds(0.5f);
            restartText.alpha = 0f;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
