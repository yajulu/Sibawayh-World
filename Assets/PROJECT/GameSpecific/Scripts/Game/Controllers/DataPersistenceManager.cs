using Project.GameSpecific.Scripts.Data;
using PROJECT.Scripts.Enums;
using Project.YajuluSDK.Scripts.Essentials;
using Project.YajuluSDK.Scripts.Tools;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.GameSpecific.Scripts.Game.Controllers
{
    public class DataPersistenceManager : Singleton<DataPersistenceManager>
    {
        [SerializeField] private string playerDataKey;
        [SerializeField] private PlayerData playerData;

        public PlayerData PlayerData => playerData;

        [Button, TitleGroup("Progress")]
        public void LoadData()
        {
            if (PlayerPrefs.HasKey(playerDataKey))
            {
                playerData = SaveUtility.LoadObject<PlayerData>(playerDataKey);
            }
            else
            {
                playerData = new PlayerData();
                playerData.levelStates.Add(eLevelState.Unlocked);
                SaveProgress();
            }
        }
        
        [Button, TitleGroup("Progress")]
        public void SaveProgress()
        {
            SaveUtility.SaveObject(playerDataKey, playerData);
        }

        [Button, TitleGroup("Progress")]
        public void ClearProgress()
        {
            PlayerPrefs.DeleteKey(playerDataKey);
            PlayerPrefs.Save();
            playerData = null;
        }

    }
}
