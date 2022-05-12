using Sirenix.OdinInspector;
using UnityEngine;

namespace PROJECT.Scripts.Game
{
    public class LetterSpawner : MonoBehaviour
    {
        [SerializeField, TitleGroup("Refs"), OnValueChanged(nameof(UpdateLettersList))] private Transform parentTarget;

        [SerializeField, DisableIf("@this.parentTarget == null"), OnValueChanged(nameof(UpdateCountFromWord))]
        private string word;

        [SerializeField, PropertyRange(0, nameof(MaxCount)), DisableIf("@this.parentTarget == null"), OnValueChanged(nameof(UpdateSpawner))]
        private int count;

        [SerializeField, OnValueChanged(nameof(UpdateSpawner))] private float radius;

        private GameLetter [] _letters;
        private Transform _dummyTransform;
        private GameLetter _dummyGameLetter;
        private Vector2 _dummyPosition;
        
        public int Count
        {
            get => count;
            set
            {
                count = value;
                UpdateSpawner();
            }
        }

        public string Word
        {
            get => word;
            set 
            {
                word = value; 
                UpdateCountFromWord();
            }
        }

        private int MaxCount()
        {
            return parentTarget == null ? 0 : _letters.Length;
        }
        
        private void UpdateLettersList()
        {
            _letters = parentTarget.GetComponentsInChildren<GameLetter>();
        }

        private void UpdateSpawner()
        {
            var angle = (Mathf.PI * 2) / count;
            for (var i = 0; i < _letters.Length; i++)
            {
                _dummyGameLetter = null;
                _dummyGameLetter = _letters[i];
                
                _dummyTransform = null;
                _dummyTransform = _dummyGameLetter.transform;

                if (i < count)
                {
                    _dummyTransform.gameObject.SetActive(true);
                    _dummyPosition.x = Mathf.Cos((angle * i) + (Mathf.PI * 0.5f)) * radius;
                    _dummyPosition.y = Mathf.Sin((angle * i) + (Mathf.PI * 0.5f)) * radius;
                    _dummyTransform.localPosition = _dummyPosition;
                    _dummyGameLetter.Letter = i < word.Length ? word[i].ToString() : "X";
                }
                else
                {
                    _dummyTransform.gameObject.SetActive(false);
                }
            }
        }

        private void UpdateCountFromWord()
        {
            if (word == null)
                return;
            count = Mathf.Min(word.Length, _letters.Length);
            UpdateSpawner();
        }
    }
}
