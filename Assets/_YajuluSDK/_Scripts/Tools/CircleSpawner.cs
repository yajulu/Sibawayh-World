using Sirenix.OdinInspector;
using UnityEngine;

namespace _YajuluSDK._Scripts.Tools
{
    public class CircleSpawner : MonoBehaviour
    {
        [SerializeField, TitleGroup("Refs")] private Transform parentTarget;

        [SerializeField, PropertyRange(0, nameof(MaxCount)), DisableIf("@this.parentTarget == null"), OnValueChanged(nameof(UpdateSpawner))]
        private int count;

        [SerializeField, OnValueChanged(nameof(UpdateSpawner))] private float radius;

        private Transform _dummyTransform;
        private Vector2 _dummyPosition;

        public int Count
        {
            get => count;
            set => count = value;
        }

        private int MaxCount()
        {
            return parentTarget == null ? 0 : parentTarget.childCount;
        }

        private void UpdateSpawner()
        {
            var angle = (Mathf.PI * 2) / count;
            for (var i = 0; i < parentTarget.childCount; i++)
            {
                _dummyTransform = null;
                _dummyTransform = parentTarget.GetChild(i);

                if (i < count)
                {
                    _dummyTransform.gameObject.SetActive(true);
                    _dummyPosition.x = Mathf.Cos((angle * i) + (Mathf.PI * 0.5f)) * radius;
                    _dummyPosition.y = Mathf.Sin((angle * i) + (Mathf.PI * 0.5f)) * radius;
                    _dummyTransform.localPosition = _dummyPosition;
                }
                else
                {
                    _dummyTransform.gameObject.SetActive(false);
                }
            }
        }

    }
}
