using System;
using System.Collections.Generic;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.Tools;
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

        [SerializeField, ReadOnly] private eLevelState[] levelStates;
        [SerializeField, ReadOnly] private int[] intLevelStates;

        public int CurrentWordIndex => currentWordIndex;
        public string CurrentReferenceWord => currentReferenceWord;
        public LevelData CurrentLevelData => currentLevelData;

        public GameData CurrentGameData => gameData;

        protected override void OnAwake()
        {
            base.OnAwake();
            LoadData();
        }

        public eLevelState GetLevelState(int levelNumber)
        {
            return levelStates[levelNumber - 1];
        }

        [Button, TitleGroup("Progress")]
        private void LoadData()
        {
            if (PlayerPrefs.HasKey(gameData.ProgressKey))
            {
                intLevelStates = SaveUtility.LoadList<int>(gameData.ProgressKey);
                levelStates = Array.ConvertAll(intLevelStates, ConvertIntToEnum);    
                if (levelStates.Length != gameData.GetLevelsCount)
                {
                    ClearProgress();
                    LoadData();
                }
            }
            else
            {
                levelStates = new eLevelState[gameData.GetLevelsCount];
                levelStates[0] = eLevelState.Unlocked;
                SaveProgress();
                // LoadData();
            }

            eLevelState ConvertIntToEnum(int val)
            {
                return (eLevelState)val;
            }
            
        }
        
        [Button, TitleGroup("Progress")]
        private void SaveProgress()
        {
            intLevelStates = Array.ConvertAll(levelStates, ConvertEnumToInt);
            SaveUtility.SaveList(gameData.ProgressKey, intLevelStates);
            
            int ConvertEnumToInt(eLevelState val)
            {
                return (int)val;
            }
        }

        [Button, TitleGroup("Progress")]
        private void ClearProgress()
        {
            PlayerPrefs.DeleteKey(gameData.ProgressKey);
            PlayerPrefs.HasKey(gameData.ProgressKey);
            levelStates = null;
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
            SaveProgress();
        }

        protected virtual void OnGameModeWordUpdated(string checkWord)
        {
            GameModeWordUpdated?.Invoke(checkWord);
        }
    }
    
}
