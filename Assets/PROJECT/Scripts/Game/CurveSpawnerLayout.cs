using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PROJECT.Scripts.Game
{
    public class CurveSpawnerLayout : LayoutGroup
    {
        [SerializeField, OnValueChanged(nameof(Spawn))] private AnimationCurve spawnCurve;
        [SerializeField, OnValueChanged(nameof(Spawn))] private float spawnSpacing;
        [SerializeField, OnValueChanged(nameof(Spawn))] private float samplingAmplitude;
        [SerializeField, Range(0,1), OnValueChanged(nameof(Spawn))] private float spawnSampling;

        private float maxChildWidth;
        private float maxChildHeight;
        
        private RectTransform topElement;
        [Button]
        private void Spawn()
        {
            // var samplePerIteration = 1 / spawnSampling;
            var paddingV = 0f;

            var alignmentOnAxisShift = 1 - GetAlignmentOnAxis(0) * 2;
            rectTransform.pivot = new Vector2(GetAlignmentOnAxis(0), 0);
            maxChildWidth = float.MinValue;
            maxChildHeight = float.MinValue;
            
            
            for (int i = 0; i < rectChildren.Count; i++)
            {
                RectTransform child = rectChildren[i];

                maxChildHeight = child.rect.height > maxChildHeight ? child.rect.height : maxChildHeight;
                maxChildWidth = child.rect.width > maxChildWidth ? child.rect.width : maxChildWidth;
                
                if (i == 0)
                {
                    paddingV = child.rect.height * 0.5f;
                }
                var newPosition = child.localPosition;
                var evaluatedValue = spawnCurve.Evaluate(i * spawnSampling) + alignmentOnAxisShift;
                newPosition.y = i * spawnSpacing + paddingV + m_Padding.bottom;
                newPosition.x = samplingAmplitude * evaluatedValue;
                child.localPosition = newPosition;
                if (i == rectChildren.Count - 1)
                    topElement = child;
            }
        }

        public override void CalculateLayoutInputVertical()
        {
            
        }

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            Spawn();
            var totalWidth = (samplingAmplitude * 2) + (maxChildWidth) + padding.horizontal;
            var totalHeight = (spawnSpacing * (rectChildren.Count - 1)) + topElement.rect.height + padding.vertical;
            SetLayoutInputForAxis(totalWidth ,0,0,0);
            SetLayoutInputForAxis(totalHeight,0,0,1);
        }

        public override void SetLayoutHorizontal()
        {
            // throw new System.NotImplementedException();
        }
        
        public override void SetLayoutVertical()
        {
            // throw new System.NotImplementedException();
        }
    }
}
