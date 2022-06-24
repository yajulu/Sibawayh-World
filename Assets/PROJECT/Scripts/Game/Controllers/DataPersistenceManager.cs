using System;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.GameConfig;
using _YajuluSDK._Scripts.Tools;
using Project.Scripts.Data;
using PROJECT.Scripts.Data;
using PROJECT.Scripts.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PROJECT.Scripts.Game.Controllers
{
    public class DataPersistenceManager : Singleton<DataPersistenceManager>
    {
        [SerializeField] private string playerDataKey;
        [SerializeField] private PlayerProgress _playerProgress;

        public PlayerProgress PlayerProgress => _playerProgress;

        [Button, TitleGroup("Progress")]
        public void LoadPlayerProgress()
        {
            if (PlayerPrefs.HasKey(playerDataKey))
            {
                _playerProgress = SaveUtility.LoadObject<PlayerProgress>(playerDataKey);
            }
            else
            {
                _playerProgress = new PlayerProgress();
                _playerProgress.levelStates.Add(eLevelState.Unlocked);
                SaveProgress();
            }
        }
        
        [Button, TitleGroup("Progress")]
        public void SaveProgress()
        {
            SaveUtility.SaveObject(playerDataKey, _playerProgress);
        }

        [Button, TitleGroup("Progress")]
        public void ClearProgress()
        {
            PlayerPrefs.DeleteKey(playerDataKey);
            PlayerPrefs.Save();
            _playerProgress = null;
        }

    }
}
