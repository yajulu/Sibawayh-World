using System;
using System.Collections.Generic;
using _YajuluSDK._Scripts.Essentials;
using PROJECT.Scripts.Data;
using PROJECT.Scripts.Enums;
using PROJECT.Scripts.ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PROJECT.Scripts.Game.Controllers
{
    public class GameModeManager : Singleton<GameModeManager>
    {

        public event Action GameModeStarted;
        public event Action<string> GameModeWordChanged;
        public event Action<string> GameModeWordUpdated; 
        public event Action<string, bool> GameModeCheckWord;
        public event Action GameModeCompleted;

        [SerializeField, ReadOnly] private string currentCheckWord;
        [SerializeField, ReadOnly] private string currentReferenceWord;

        private Dictionary<int, string> _lettersDict = new Dictionary<int, string>();

        [SerializeField] private int currentLevel = 1;
        [SerializeField] private GameData gameData;
        [SerializeField, ReadOnly] private string []currentLevelWordsList;
        [SerializeField, ReadOnly] private int currentWordIndex = 0;
        [SerializeField, ReadOnly] private LevelData currentLevelData;

        public int CurrentWordIndex => currentWordIndex;

        public string CurrentReferenceWord => currentReferenceWord;

        public LevelData CurrentLevelData => currentLevelData;

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

        // public void AddLetter(string letter, int gameLetterIndex)
        // {
        //     _lettersDict.Add(gameLetterIndex, letter);
        //     UpdateCheckWord();
        // }
        //
        // public void RemoveLetter(string letter, int gameLetterIndex)
        // {
        //     _lettersDict.Remove(gameLetterIndex);
        //     UpdateCheckWord();
        // }

        public void TestCurrentCheckWord()
        {
            if(currentCheckWord.Length == currentReferenceWord.Length)
                OnGameModeCheckWord(currentCheckWord, CheckWord());
        }

        public void UpdateCheckWord(string newCheckWord)
        {
            // currentCheckWord = "";
            // foreach (var letter in _lettersDict.Values)
            // {
            //     currentCheckWord += letter;
            // }
            currentCheckWord = newCheckWord;
            OnGameModeWordUpdated(currentCheckWord);
        }

        private void ShowNextWord()
        {
            if (currentWordIndex + 1 == currentLevelWordsList.Length)
            {
                OnGameModeCompleted();
            }
            else
            {
                currentWordIndex++;
                OnGameModeWordChanged(currentLevelWordsList[currentWordIndex]);    
            }
        }

        public string GetCurrentLevelTypeName()
        {
            return gameData.GetLevelTypeName(currentLevelData.LevelType);
        }

        protected virtual bool CheckWord()
        {
            return currentCheckWord.Equals(currentReferenceWord);
        }

        protected virtual void OnGameModeStarted()
        {
            currentWordIndex = 0;
            currentLevelData = gameData.GetLevelData(currentLevel);
            currentLevelWordsList = currentLevelData.Words;
            currentReferenceWord = currentLevelWordsList[0];
            currentCheckWord = "";
            GameModeStarted?.Invoke();
        }

        protected virtual void OnGameModeWordChanged(string newWord)
        {
            currentReferenceWord = newWord;
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
            // else
            // {
            //     OnGameModeWordChanged(currentReferenceWord);
            // }
        }

        protected virtual void OnGameModeCompleted()
        {
            GameModeCompleted?.Invoke();
        }

        protected virtual void OnGameModeWordUpdated(string checkWord)
        {
            GameModeWordUpdated?.Invoke(checkWord);
        }
    }
    
}
