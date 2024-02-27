using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{
    public Image img;
    public float time;
    public Color flashColor;
    CanvasGroup CG;
    private Color defaultColor;

    void Start()
    {
        CG = GetComponent<CanvasGroup>();
        defaultColor = img.color;
    }

    public void FlashScreen()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        yield return new WaitForSeconds(0.1f);
        img.color = flashColor;
        CG.alpha = 1;
        yield return new WaitForSeconds(time);
        img.color = defaultColor;
        CG.alpha = 0;
        this.gameObject.SetActive(false);
    }
}
