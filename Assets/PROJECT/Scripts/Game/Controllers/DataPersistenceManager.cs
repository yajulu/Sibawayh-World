using System;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.GameConfig;
using _YajuluSDK._Scripts.Social;
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
        [SerializeField] private PlayerProgress _progress;

        public PlayerProgress Progress => _progress;

        [Button, TitleGroup("Progress")]
        public void LoadPlayerProgress()
        {
            if (SaveUtility.HasKey(nameof(PlayerProgress)))
            {
                _progress = SaveUtility.LoadObject<PlayerProgress>(nameof(PlayerProgress));
                PlayfabManager.LoadPlayerData<PlayerProgress>(nameof(PlayerProgress), (progress) => Instance._progress = progress);
            }
            else
            {
                _progress = new PlayerProgress();
                _progress.levelStates.Add(eLevelState.Unlocked);
                SaveProgress();
            }
        }
        
        [Button, TitleGroup("Progress")]
        public void SaveProgress()
        {
            SaveUtility.SaveObject(nameof(PlayerProgress), _progress);
        }

        [Button, TitleGroup("Progress")]
        public void ClearProgress()
        {
            SaveUtility.DeleteObject(nameof(PlayerProgress));
            _progress = null;
        }
        
    }
}
