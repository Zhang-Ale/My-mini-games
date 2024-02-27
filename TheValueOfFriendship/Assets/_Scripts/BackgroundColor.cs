using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColor : MonoBehaviour
{
    public SpriteRenderer SR;
    public Color[] colors; 
    private float targetPoint;
    private int curColInt = 0;
    private int newColInt = 1;
    public bool changeCol;
    float timer = 0.0f; 

    void FixedUpdate()
    {
        if (changeCol)
        {
            //Transition(); 
            SmoothTransition(); 
        }
    }

    void Transition()
    {
        timer += Time.deltaTime;
        if (timer >= 3.0f)
        {
            Color col = new Color(Random.value, Random.value, Random.value, 1.0f);
            SR.color = col;
            timer = 0;
        }
    }

    void SmoothTransition()
    {
        targetPoint += Time.deltaTime;
        SR.color = Color.Lerp(colors[curColInt], colors[newColInt], targetPoint);
        if(targetPoint >= 3f)
        {
            targetPoint = 0f;
            curColInt = newColInt;
            newColInt++;
            if (newColInt == colors.Length)
                newColInt = 0; 
        }
    }
}
