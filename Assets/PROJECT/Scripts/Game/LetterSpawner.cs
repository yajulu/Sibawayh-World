using System;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.UI;
using PROJECT.Scripts.Game.Controllers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace PROJECT.Scripts.Game
{
    public class LetterSpawner : UIBehaviour
    {
        [SerializeField, TitleGroup("Refs"), OnValueChanged(nameof(UpdateLettersList))] private Transform parentTarget;
        [SerializeField, TitleGroup("Refs")] private TextMeshProUGUI currentCheckWord;
        [SerializeField, TitleGroup("Refs")] private Transform separator;
        
        [SerializeField, DisableIf("@this.parentTarget == null"), OnValueChanged(nameof(UpdateCountFromWord))]
        private string word;

        [SerializeField, PropertyRange(0, nameof(MaxCount)), DisableIf("@this.parentTarget == null"), OnValueChanged(nameof(UpdateSpawner))]
        private int count;

        [SerializeField, OnValueChanged(nameof(UpdateSpawner))] private float radius;

        [SerializeField, ReadOnly] private GameLetter [] letters;
        
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
            return parentTarget == null ? 0 : letters.Length;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            GameModeManager.Instance.GameModeStarted += OnGameModeStarted;
            GameModeManager.Instance.GameModeWordUpdated += UpdateCheckWord;
            GameModeManager.Instance.GameModeWordChanged += UpdateReferenceWord;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            if (Singleton.Quitting) return;
            GameModeManager.Instance.GameModeStarted -= OnGameModeStarted;
            GameModeManager.Instance.GameModeWordUpdated -= UpdateCheckWord;
            GameModeManager.Instance.GameModeWordChanged -= UpdateReferenceWord;
        }

        [Button]
        private void UpdateLettersList()
        {
            letters = parentTarget.GetComponentsInChildren<GameLetter>(true);
        }

        [Button]
        private void SetRefs()
        {
            currentCheckWord = transform.FindDeepChild<TextMeshProUGUI>("CheckWord_Text");
            separator = transform.FindDeepChild<Transform>("ClickSeparator");
        }

        private void UpdateSpawner()
        {
            var angle = (Mathf.PI * 2) / count;
            for (var i = 0; i < letters.Length; i++)
            {
                _dummyGameLetter = null;
                _dummyGameLetter = letters[i];
                
                _dummyTransform = null;
                _dummyTransform = _dummyGameLetter.transform;

                if (i < count)
                {
                    _dummyTransform.gameObject.SetActive(true);
                    _dummyPosition.x = Mathf.Cos((angle * i) + (Mathf.PI * 0.5f)) * radius;
                    _dummyPosition.y = Mathf.Sin((angle * i) + (Mathf.PI * 0.5f)) * radius;
                    _dummyTransform.localPosition = _dummyPosition;
                    _dummyGameLetter.Letter = i < word.Length ? word[i].ToString() : "X";
                    _dummyGameLetter.OnButtonToggled = HandleButtonToggles;
                    _dummyGameLetter.OnButtonUpOrDragEnded = OnButtonUpOrDragEnded;
                }
                else
                {
                    _dummyTransform.gameObject.SetActive(false);
                }
            }

            void OnButtonUpOrDragEnded()
            {
                GameModeManager.Instance.TestCurrentCheckWord();    
            }
            
            void HandleButtonToggles(string letter, int buttonIndex, bool selected)
            {
                if (selected)
                {
                    GameModeManager.Instance.AddLetter(letter, buttonIndex);
                }
                else
                {
                    GameModeManager.Instance.RemoveLetter(letter, buttonIndex);
                }
            
            }
        }
        
        private void OnGameModeStarted()
        {
            UpdateCheckWord("");
        }

        private void UpdateCheckWord(string checkWord)
        {
            currentCheckWord.SetText(checkWord);
        }

        private void UpdateReferenceWord(string refWord)
        {
            // if (Input.GetMouseButton(0))
            // {
            //     _separator.gameObject.SetActive(true);    
            // }
            Word = refWord;
        }

        

        private void UpdateCountFromWord()
        {
            if (word == null)
                return;
            count = Mathf.Min(word.Length, letters.Length);
            UpdateSpawner();
        }
    }
}
