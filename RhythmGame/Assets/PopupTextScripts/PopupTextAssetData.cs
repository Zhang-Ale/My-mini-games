using UnityEngine;

namespace UI.PopupText
{
    [CreateAssetMenu(fileName = "New PopupTextData",
        menuName = "PopupText/PopupTextData")]
    public class PopupTextAssetData : ScriptableObject
    {
        public Color fontColor;
        [Range(20, 60)] public float fontSize;
        public AnimationCurve scaleCurve;
        public AnimationCurve verticalCurve;
        public AnimationCurve horizontalCurve;
        public AnimationCurve alphaCurve;
        public Sprite icon;

        public float EndTime
        {
            get
            {
                return 2.0f; 
                //return (horizontalCurve.keys[^1].time + scaleCurve.keys[^1].time)* 100f;
            }
        }

        public float EvaluateScale(float time)
        {
            return scaleCurve.Evaluate(time);
        }

        public float EvaluateVertical(float time)
        {
            return verticalCurve.Evaluate(time);
        }

        public float EvaluateHorizontal(float time)
        {
            return horizontalCurve.Evaluate(time);
        }

        /// <summary>
		/// Fade away starts with the verticalCurve
        /// </summary>
		///

        public float EvaluateAlpha(float time)
        {
            return Mathf.Clamp(alphaCurve.Evaluate(time), 0, 1);
        }
    }
}

