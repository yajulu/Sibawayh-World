using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.UI;
using DG.Tweening;
using PROJECT.Scripts.Game.Controllers;
using RTLTMPro;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace PROJECT.Scripts.Game
{
    public class LetterSpawner : UIBehaviour
    {
        [SerializeField, TitleGroup("Refs"), OnValueChanged(nameof(UpdateLettersList))] private Transform parentTarget;
        [SerializeField, TitleGroup("Refs")] private RTLTextMeshPro currentCheckWord;
        [SerializeField, TitleGroup("Refs")] private Image centerPieceImage;
        [SerializeField, TitleGroup("Refs")] private RectTransform centerPieceTransform;
        
        [SerializeField, DisableIf("@this.parentTarget == null"), OnValueChanged(nameof(UpdateCountFromWord)), SuffixLabel("@fixedWord", true)]
        private string word;

        private string fixedWord;

        [SerializeField, PropertyRange(0, nameof(MaxCount)), DisableIf("@this.parentTarget == null"), OnValueChanged(nameof(UpdateSpawner))]
        private int count;

        [SerializeField, OnValueChanged(nameof(UpdateSpawner))] private float radius;

        [SerializeField, ReadOnly] private GameLetter [] letters;
        
        private Transform _dummyTransform;
        private GameLetter _dummyGameLetter;
        private Vector2 _dummyPosition;

        

        private List<GameLetter> _currentSelectedLetters = new List<GameLetter>();

        public event Action OnResetWord;
        
        private Sequence checkingSequence;

        private Sequence _lettersSequence;

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
            ResetCenterPiece();
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            ResetLetters();
            if (Singleton.Quitting) return;
            GameModeManager.Instance.GameModeStarted -= OnGameModeStarted;
            GameModeManager.Instance.GameModeWordUpdated -= UpdateCheckWordUI;
            GameModeManager.Instance.GameModeWordChanged -= UpdateReferenceWord;
            GameModeManager.Instance.GameModeCheckWord -= OnGameModeCheckWord;
        }

        [Button]
        private void UpdateLettersList()
        {
            letters = parentTarget.GetComponentsInChildren<GameLetter>(true);
        }

        [Button]
        private void SetRefs()
        {
            currentCheckWord = transform.FindDeepChild<RTLTextMeshPro>("CheckWord_Text");
            centerPieceImage = transform.FindDeepChild<Image>("CenterPiece_Center");
            centerPieceTransform = transform.FindDeepChild<RectTransform>("CenterPiece");
            // separator = transform.FindDeepChild<Transform>("ClickSeparator");
        }

        private void ResetLetters()
        {
            foreach (var letter in letters)
            {
                letter.transform.localPosition = Vector3.zero;
            }
        }

        private void UpdateSpawner()
        {
            _currentSelectedLetters.Clear();
            var angle = (Mathf.PI * 2) / count;
            
            _lettersSequence = DOTween.Sequence();
            
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
                        var index = numbers.Current;
                        _dummyTransform.gameObject.SetActive(true);
                        _dummyPosition.x = Mathf.Cos((angle * i) + (Mathf.PI * 0.5f)) * radius;
                        _dummyPosition.y = Mathf.Sin((angle * i) + (Mathf.PI * 0.5f)) * radius;
                        _dummyTransform.localPosition = _dummyPosition;
                        _dummyGameLetter.Letter = index < word.Length ? word[index].ToString() : "X";
                        _dummyGameLetter.OnButtonToggled = HandleButtonToggles;
                        _dummyGameLetter.OnButtonUp = OnButtonUpOrDragEnded;
                        if(Application.isPlaying)
                            _lettersSequence.Insert(i * 0.1f, _dummyTransform.DOLocalMove(_dummyPosition, 0.15f).SetEase(Ease.OutBack).From(Vector3.zero));
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
            currentCheckWord.text = checkWord;
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
            checkingSequence = DOTween.Sequence();
            checkingSequence.OnComplete(ResetCenterPiece);
            if (check)
            {
                checkingSequence.Append(centerPieceImage.DOColor(Color.green, 0.1f).SetEase(Ease.Linear));
                checkingSequence.Join(centerPieceTransform.DOPunchScale(Vector3.one * 0.15f, 0.5f, 5).SetEase(Ease.Linear));
                checkingSequence.Append(centerPieceImage.DOColor(Color.cyan, 0.4f));
            }
            else
            {
                checkingSequence.Append(centerPieceImage.DOColor(Color.red, 0.1f).SetEase(Ease.Linear));
                checkingSequence.Join(centerPieceTransform.transform.DOShakePosition(0.5f, 25f));
                checkingSequence.Append(centerPieceImage.DOColor(Color.cyan, 0.4f));
                
                OnResetWord?.Invoke();
                _currentSelectedLetters.Clear();
            }

        }
        
        private void ResetCenterPiece()
        {
            centerPieceImage.color = Color.cyan;
            centerPieceTransform.localScale = Vector3.one;
            centerPieceTransform.localPosition = Vector3.zero;
        }
        
        private void UpdateCountFromWord()
        {
            if (word == null)
                return;
            // fixedWord = RtlString.GetFixedText(word);
            count = Mathf.Min(word.Length, letters.Length);
            UpdateSpawner();
        }
    }
}
