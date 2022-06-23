using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PROJECT.Scripts.Game
{
    [RequireComponent(typeof(RectTransform))]
    public class CurveSpawnerLayout : UIBehaviour
    {
        [SerializeField, OnValueChanged(nameof(Spawn))]
        private RectOffset padding;
        [SerializeField, OnValueChanged(nameof(Spawn))] private AnimationCurve spawnCurve;
        [SerializeField, OnValueChanged(nameof(Spawn))] private float spawnSpacing;
        [SerializeField, OnValueChanged(nameof(Spawn))] private float samplingAmplitude;
        [SerializeField, Range(0,1), OnValueChanged(nameof(Spawn))] private float spawnSampling;
        
        [SerializeField, TitleGroup("Refs")] private RectTransform parent;
        private float maxChildWidth;
        private float maxChildHeight;
        
        [System.NonSerialized] private RectTransform m_Rect;
        protected RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                    m_Rect = GetComponent<RectTransform>();
                return m_Rect;
            }
        }

        public CurveSpawnerLayout()
        {
            padding = new RectOffset();
        }
        
        private RectTransform topElement;
        [Button]
        private void Spawn()
        {
            // var samplePerIteration = 1 / spawnSampling;
            var paddingV = 0f;

            // var alignmentOnAxisShift = 1 - GetAlignmentOnAxis(0) * 2;
            parent.pivot = new Vector2(0.5f, 0);
            maxChildWidth = float.MinValue;
            maxChildHeight = float.MinValue;
            
            
            for (int i = 0; i < parent.childCount; i++)
            {
                RectTransform child = (RectTransform)parent.GetChild(i);

                maxChildHeight = child.rect.height > maxChildHeight ? child.rect.height : maxChildHeight;
                maxChildWidth = child.rect.width > maxChildWidth ? child.rect.width : maxChildWidth;
                
                if (i == 0)
                {
                    paddingV = child.rect.height * 0.5f;
                }
                var newPosition = child.localPosition;
                var evaluatedValue = spawnCurve.Evaluate(i * spawnSampling);
                newPosition.y = i * spawnSpacing + paddingV + padding.bottom;
                newPosition.x = samplingAmplitude * evaluatedValue;
                child.localPosition = newPosition;
                // if (i == (parent.childCount - 1))
                //     topElement = child;
                
                // parent.
                var totalWidth = (samplingAmplitude * 2) + (maxChildWidth) + padding.horizontal;
                var totalHeight = (spawnSpacing * (parent.childCount - 1)) + padding.vertical;
            }
        }
        //
        // public override void CalculateLayoutInputVertical()
        // {
        //     
        // }
        //
        // public override void CalculateLayoutInputHorizontal()
        // {
        //     base.CalculateLayoutInputHorizontal();
        //     // Spawn();
        //     var totalWidth = (samplingAmplitude * 2) + (maxChildWidth) + padding.horizontal;
        //     var totalHeight = (spawnSpacing * (rectChildren.Count - 1)) + topElement.rect.height + padding.vertical;
        //     SetLayoutInputForAxis(totalWidth ,0,0,0);
        //     SetLayoutInputForAxis(totalHeight,0,0,1);
        // }
        //
        // public override void SetLayoutHorizontal()
        // {
        //     // throw new System.NotImplementedException();
        // }
        
        public void SetLayoutHorizontal()
        {
            throw new System.NotImplementedException();
        }
        
    }
}
