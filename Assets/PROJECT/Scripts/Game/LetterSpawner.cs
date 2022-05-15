using System;
using System.Collections.Generic;
using System.Linq;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.UI;
using PROJECT.Scripts.Game.Controllers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

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

        private List<GameLetter> _currentSelectedLetters = new List<GameLetter>();

        public event Action OnResetWord;

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
            GameModeManager.Instance.GameModeWordUpdated += UpdateCheckWordUI;
            GameModeManager.Instance.GameModeWordChanged += UpdateReferenceWord;
            GameModeManager.Instance.GameModeCheckWord += OnGameModeCheckWord;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            if (Singleton.Quitting) return;
            GameModeManager.Instance.GameModeStarted -= OnGameModeStarted;
            GameModeManager.Instance.GameModeWordUpdated -= UpdateCheckWordUI;
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
            _currentSelectedLetters.Clear();
            var angle = (Mathf.PI * 2) / count;
            using var numbers = Enumerable.Range(0, count).OrderBy(x => Random.Range(0, count)).GetEnumerator();
            {
                for (var i = 0; i < letters.Length; i++)
                {
                    _dummyGameLetter = null;
                    _dummyGameLetter = letters[i];
                
                    _dummyTransform = null;
                    _dummyTransform = _dummyGameLetter.transform;
               
                    if (i < count)
                    {
                        numbers.MoveNext();
                        var angleMultiplier = numbers.Current;
                        _dummyTransform.gameObject.SetActive(true);
                        _dummyPosition.x = Mathf.Cos((angle * angleMultiplier) + (Mathf.PI * 0.5f)) * radius;
                        _dummyPosition.y = Mathf.Sin((angle * angleMultiplier) + (Mathf.PI * 0.5f)) * radius;
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
            }

            void OnButtonUpOrDragEnded()
            {
                GameModeManager.Instance.TestCurrentCheckWord();    
            }
            
            void HandleButtonToggles(GameLetter letter, bool selected)
            {
                if (selected)
                {
                    _currentSelectedLetters.Add(letter);
                }
                else
                {
                    _currentSelectedLetters.Remove(letter);
                }

                var newCheckWord = _currentSelectedLetters.Aggregate("", (current, selectedLetter) => current + selectedLetter.Letter);
                GameModeManager.Instance.UpdateCheckWord(newCheckWord);
            }
        }
        
        private void OnGameModeStarted()
        {
            UpdateCheckWordUI("");
        }

        private void UpdateCheckWordUI(string checkWord)
        {
            currentCheckWord.SetText(checkWord);
        }

        private void UpdateReferenceWord(string refWord)
        {
            // if (Input.GetMouseButton(0))
            // {
            //     _separator.gameObject.SetActive(true);    
            // }
            // if (refWord.Equals(word))
            // {
            //     OnResetWord?.Invoke();
            // }
            // else
            // {
            Word = refWord;    
            // }
        }
        
        private void OnGameModeCheckWord(string checkWord, bool check)
        {
            if (!check)
            {
                OnResetWord?.Invoke();
                _currentSelectedLetters.Clear();
            }
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
