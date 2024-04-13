using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

namespace UI.PopupText
{
    public class PopupTextController : MonoBehaviour
    {
        public TextMeshProUGUI popupText;
        public Image iconImage;

        private PopupTextAssetData currentData;
        private float startTime;
        float elapsedTime;

        public void ShowPopup(PopupTextAssetData data, int damage)
        {
            currentData = data;
            startTime = Time.time;
            elapsedTime = 0.0f;
            // Set text color, size, and content
            popupText.color = data.fontColor;
            popupText.fontSize = data.fontSize;
            popupText.text = damage.ToString();

            // Set icon
            iconImage.sprite = data.icon;
            iconImage.enabled = true;
        }

        public void HidePopupText()
        {
            // Reset transform position
            transform.localPosition = Vector3.zero;

            // Reset popupTextAsset (if needed)
            // currentData = null;

            //popupText.text = ""; 
            iconImage.enabled = false;

            // Scale down the Popup Text
            transform.localScale = Vector3.zero;

            Debug.Log("Hidden"); 
            
        }

        private void Update()
        {
            if (currentData == null) return;

            elapsedTime = Time.time - startTime;

            // Update position based on curves
            transform.localPosition = new Vector3(
                currentData.EvaluateHorizontal(elapsedTime),
                currentData.EvaluateVertical(elapsedTime),
                0);

            // Update scale based on curve
            transform.localScale = Vector3.one * currentData.EvaluateScale(elapsedTime);

            // Update alpha based on curve
            Color textColor = popupText.color;
            textColor.a = currentData.EvaluateAlpha(elapsedTime);
            popupText.color = textColor;

            // Deactivate after end time
            if (elapsedTime >= currentData.EndTime)
            {
                Debug.Log(elapsedTime.ToString() + " + " + currentData.EndTime.ToString());
                HidePopupText();
            }
        }
    }
}

