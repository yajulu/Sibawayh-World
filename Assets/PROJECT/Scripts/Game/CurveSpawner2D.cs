using Sirenix.OdinInspector;
using UnityEngine;

namespace PROJECT.Scripts.Game
{
    public class CurveSpawner2D : MonoBehaviour
    {
        
        [SerializeField, OnValueChanged(nameof(Spawn))] private AnimationCurve spawnCurve;
        [SerializeField, OnValueChanged(nameof(Spawn))] private float spawnSpacing;
        [SerializeField, OnValueChanged(nameof(Spawn))] private float samplingAmplitude;
        [SerializeField, Range(0,1), OnValueChanged(nameof(Spawn))] private float spawnSampling;
        
        [SerializeField, TitleGroup("Refs")] private Transform parent;

        // private RectTransform topElement;
        [Button]
        private void Spawn()
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                
                var newPosition = child.localPosition;
                var evaluatedValue = spawnCurve.Evaluate(i * spawnSampling);
                newPosition.y = i * spawnSpacing;
                newPosition.x = samplingAmplitude * evaluatedValue;
                child.localPosition = newPosition;

                // var totalWidth = (samplingAmplitude * 2) + (maxChildWidth);
                // var totalHeight = (spawnSpacing * (parent.childCount - 1));
            }
        }
        
    }
}
