using System;
using System.Collections.Generic;
using _YajuluSDK._Scripts.Essentials;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PROJECT.Scripts.Game.Controllers
{
    public class GameModeManager : Singleton<GameModeManager>
    {

        public event Action GameModeStarted;
        public event Action<string> GameModeWordChanged;
        public event Action<string, bool> GameModeCheckWord;

        public event Action GameModeCompleted;

        [SerializeField, ReadOnly] private string currentCheckWord;
        [SerializeField, ReadOnly] private string currentReferenceWord;

        private Dictionary<int, string> _lettersDict = new Dictionary<int, string>();

        [SerializeField] private List<string> currentLevelWordsList;
        [SerializeField, ReadOnly] private int currentWordIndex = 0;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void StartGameMode()
        {
            OnGameModeStarted();
            OnGameModeWordChanged(currentReferenceWord);
        }

        public void AddLetter(string letter, int gameLetterIndex)
        {
            _lettersDict.Add(gameLetterIndex, letter);
            UpdateCheckWord();
        }

        public void RemoveLetter(string letter, int gameLetterIndex)
        {
            _lettersDict.Remove(gameLetterIndex);
            UpdateCheckWord();
        }

        private void UpdateCheckWord()
        {
            currentCheckWord = _lettersDict.Values.ToString();
            if(currentCheckWord.Length == currentReferenceWord.Length)
                OnGameModeCheckWord(currentCheckWord, CheckWord());
        }

        private void ShowNextWord()
        {
            if (currentWordIndex == currentLevelWordsList.Count)
            {
                OnGameModeCompleted();
            }
            else
            {
                currentWordIndex++;
                OnGameModeWordChanged(currentLevelWordsList[currentWordIndex]);    
            }
        }

        protected virtual bool CheckWord()
        {
            return currentCheckWord.Equals(currentReferenceWord);
        }

        protected virtual void OnGameModeStarted()
        {
            currentWordIndex = 0;
            currentReferenceWord = currentLevelWordsList[0];
            currentCheckWord = "";
            GameModeStarted?.Invoke();
        }

        protected virtual void OnGameModeWordChanged(string newWord)
        {
            _lettersDict.Clear();
            GameModeWordChanged?.Invoke(newWord);
        }

        protected virtual void OnGameModeCheckWord(string currentWord, bool result)
        {
            GameModeCheckWord?.Invoke(currentWord, result);
            if (result)
            {
                ShowNextWord();
            }
        }

        protected virtual void OnGameModeCompleted()
        {
            GameModeCompleted?.Invoke();
        }
    }
    
}
