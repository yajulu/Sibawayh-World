using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace _YajuluSDK._Scripts.UI
{
    public class UIFlexGridLayout : LayoutGroup
    {
        [SerializeField] private GridLayoutGroup.Constraint constraint;
        [SerializeField, MinValue(0)] private int rows;
        [SerializeField, MinValue(0)] private int columns;
        [SerializeField, MinValue(0)] private Vector2 cellSize;
        [SerializeField, MinValue(0)] private Vector2 spacing;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            

            switch (constraint)
            {
                case GridLayoutGroup.Constraint.Flexible:
                    var sqrt = Mathf.Sqrt(transform.childCount);
                    rows = Mathf.CeilToInt(sqrt);
                    columns = Mathf.CeilToInt(sqrt);        
                    break;
                case GridLayoutGroup.Constraint.FixedColumnCount:
                    rows = Mathf.CeilToInt((float)transform.childCount / columns);
                    break;
                case GridLayoutGroup.Constraint.FixedRowCount:
                    columns = Mathf.CeilToInt((float)transform.childCount / rows);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            var rect = rectTransform.rect;

            var cellWidth = (rect.width - (spacing.x * 2) - padding.horizontal) / (float)columns;
            var cellHeight = (rect.height - (spacing.y * 2) - padding.vertical) / (float)rows;

            cellSize.x = cellWidth;
            cellSize.y = cellHeight;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                var rowCount = Mathf.FloorToInt((float)i / columns);
                var columnCount = i % columns;

                var child = rectChildren[i];

                var xPosition = (cellSize.x + spacing.x) * columnCount + padding.left;
                var yPosition = (cellSize.y + spacing.y) * rowCount + padding.top;
                
                SetChildAlongAxis(child, 0, xPosition, cellSize.x);
                SetChildAlongAxis(child, 1, yPosition, cellSize.y);
            }

        }

        public override void CalculateLayoutInputVertical()
        {
            // throw new System.NotImplementedException();
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
