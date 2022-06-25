using System;
using System.Collections.Generic;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.GameConfig;
using _YajuluSDK._Scripts.Social;
using _YajuluSDK._Scripts.Tools;
using _YajuluSDK._Scripts.UI;
using Project.Scripts.Data;
using PROJECT.Scripts.Data;
using PROJECT.Scripts.Enums;
using PROJECT.Scripts.Game.Map;
using PROJECT.Scripts.ScriptableObjects;
using PROJECT.Scripts.UI.Screens;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PROJECT.Scripts.Game.Controllers
{
    public class GameModeManager : Singleton<GameModeManager>
    {
        public event Action GameModeLoaded;
        public event Action GameModeStarted;
        public event Action<string> GameModeWordChanged;
        public event Action<string> GameModeWordUpdated; 
        public event Action<string, bool> GameModeCheckWord;
        public event Action GameModeCompleted;
        public event Action GameModeStopped;

        [SerializeField, ReadOnly] private string currentCheckWord;
        [SerializeField, ReadOnly] private string currentReferenceWord;

        private Dictionary<int, string> _lettersDict = new Dictionary<int, string>();

        [SerializeField] private int currentLevel = 1;
        // [SerializeField] private GameData gameData;
        [SerializeField, ReadOnly] private int currentWordIndex = 0;
        [SerializeField, ReadOnly] private LevelData currentLevelData;
        [SerializeField, ReadOnly] private eGameModeState currentGameModeState;
        
        public int CurrentWordIndex => currentWordIndex;
        public string CurrentReferenceWord => currentReferenceWord;
        public LevelData CurrentLevelData => currentLevelData;
        
        private eLevelState _dummyProgress;
        public eGameModeState CurrentGameModeState => currentGameModeState;
        private PlayerProgress _playerProgress => DataPersistenceManager.Instance.Progress;
        
        public int CurrentLevel
        {
            get => currentLevel;
            set
            {
                if (currentGameModeState != eGameModeState.Unloaded)
                    return;
                currentLevel = value;
                SetCurrentLevelData(value);
            }
        }

        private void Start()
        {
            PlayfabManager.OnPlayerLoggedInBasic += LoadPlayerProgressData;
        }

        private void LoadPlayerProgressData()
        {
            DataPersistenceManager.Instance.LoadPlayerProgress();
        }
        
        private void SetCurrentLevelData(int levelNumber)
        {
            currentLevelData = GameConfig.Instance.Levels.GetLevelData(levelNumber);
        }
        
        public void UnloadGameMode()
        {
            currentGameModeState = eGameModeState.Unloaded;
        }
        public void LoadGameMode()
        {
            currentGameModeState = eGameModeState.Loaded;
            GameModeLoaded?.Invoke();
            UIScreenManager.Instance.NavigateTo(nameof(Screen_GameMode), true);
        }
        
        public void StartGameMode()
        {
            OnGameModeStarted();
            OnGameModeWordChanged(currentReferenceWord);
        }

        public void StopGameMode()
        {
            OnGameModeStopped();
        }

        public eLevelState CalculateLevelProgress()
        {
            _dummyProgress = eLevelState.Unlocked;
            foreach (var star in CurrentLevelData.StarsCounter)
            {
                if (currentWordIndex > star)
                    _dummyProgress++;
                else
                    break;
            }

            return _dummyProgress;
        }

        private void UpdatePlayerProgress()
        {
            _dummyProgress = CalculateLevelProgress();
            
            //Check Unlocking of next Level
            if (_dummyProgress > _playerProgress.GetLevelState(currentLevel))
            {
                _playerProgress.UpdateLevelState(currentLevel + 1, eLevelState.Unlocked);
            }

            _playerProgress.UpdateLevelState(currentLevel, _dummyProgress);
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
            currentWordIndex++;
            if (currentWordIndex == currentLevelData.Words.Length)
            {
                OnGameModeCompleted();
            }
            else
            {
                OnGameModeWordChanged(CurrentLevelData.Words[currentWordIndex]);    
            }
        }

        public string GetCurrentLevelTypeName()
        {
            return GameConfig.Instance.Levels.GetLevelTypeName(currentLevelData.LevelType);
        }

        protected virtual bool CheckWord()
        {
            return currentCheckWord.Equals(currentReferenceWord);
        }

        protected virtual void OnGameModeStarted()
        {
            currentWordIndex = 0;
            SetCurrentLevelData(currentLevel);
            currentReferenceWord = currentLevelData.Words[0];
            currentCheckWord = "";
            currentGameModeState = eGameModeState.Started;
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
        
        protected virtual void OnGameModeStopped()
        {
            currentGameModeState = eGameModeState.Stopped;
            UpdatePlayerProgress();
            GameModeStopped?.Invoke();
            DataPersistenceManager.Instance.SaveProgress();
        }

        protected virtual void OnGameModeCompleted()
        {
            currentGameModeState = eGameModeState.Completed;
            UpdatePlayerProgress();
            GameModeCompleted?.Invoke();
            UIScreenManager.Instance.NavigateTo(nameof(Screen_Map), true);
            DataPersistenceManager.Instance.SaveProgress();
        }

        protected virtual void OnGameModeWordUpdated(string checkWord)
        {
            GameModeWordUpdated?.Invoke(checkWord);
        }
    }
    
}
