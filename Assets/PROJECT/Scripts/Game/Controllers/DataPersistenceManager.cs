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
        [SerializeField] private PlayerProgress _playerProgress;

        public PlayerProgress PlayerProgress => _playerProgress;

        [Button, TitleGroup("Progress")]
        public void LoadPlayerProgress()
        {
            if (SaveUtility.HasKey(nameof(PlayerProgress)))
            {
                _playerProgress = SaveUtility.LoadObject<PlayerProgress>(nameof(PlayerProgress));
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
            SaveUtility.SaveObject(nameof(PlayerProgress), _playerProgress);
        }

        [Button, TitleGroup("Progress")]
        public void ClearProgress()
        {
            SaveUtility.DeleteObject(nameof(PlayerProgress));
            _playerProgress = null;
        }
        
    }
}
