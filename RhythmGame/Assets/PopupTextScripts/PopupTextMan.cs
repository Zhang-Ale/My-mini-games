using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UI.PopupText
{
    public static class PopupTextMan 
    {
        private static Dictionary<PopupTextType, PopupTextAssetData> popupTextDataMap;
        private static PopupTextController currentPopupText;

        static PopupTextMan()
        {
            // Initialize the dictionary with the scriptable object variants loaded from Resources
            popupTextDataMap = new Dictionary<PopupTextType, PopupTextAssetData>
            {
                { PopupTextType.CriticalDamage, Resources.Load<PopupTextAssetData>("CriticalDamageText") },
                { PopupTextType.Damage, Resources.Load<PopupTextAssetData>("DamageText") },
                { PopupTextType.Healing, Resources.Load<PopupTextAssetData>("HealingText") }
            };
        }

        public static void ShowPopupText(PopupTextType type, Vector3 position, int damage)
        {
            if (popupTextDataMap.TryGetValue(type, out PopupTextAssetData data))
            {
                if (currentPopupText == null)
                {
                    // Find the existing PopupTextController GameObject in the scene
                    currentPopupText = GameObject.FindObjectOfType<PopupTextController>();
                    if (currentPopupText == null)
                    {
                        Debug.LogError("PopupTextController component not found in the scene.");
                        return;
                    }
                }
                else
                { 
                    currentPopupText.transform.position = position;
                    currentPopupText.ShowPopup(data, damage);
                }
            }
            else
            {
                Debug.LogError($"PopupTextData for type {type} not found!");
            }
        }
    }
    
}
