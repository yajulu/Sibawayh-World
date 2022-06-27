using System;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.GameConfig;
using _YajuluSDK._Scripts.Social;
using _YajuluSDK._Scripts.Tools;
using Project.Scripts.Data;
using PROJECT.Scripts.Data;
using PROJECT.Scripts.Enums;
using Project.Scripts.Inventory;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PROJECT.Scripts.Game.Controllers
{
    public class DataPersistenceManager : Singleton<DataPersistenceManager>
    {
        [SerializeField] private PlayerProgress progress;
        [SerializeField] private ProfileData profileData;
        public PlayerProgress Progress => progress;

        public ProfileData ProfileData => profileData;

        public static event Action<PlayerProgress> OnPlayerProgressUpdated;
        public static event Action<ProfileData> OnProfileDataUpdated;
        
        private void Start()
        {
            PlayfabManager.OnPlayerLoggedInBasic += LoadPlayerData;
        }

        private void LoadPlayerData()
        {
            LoadPlayerProgress();
            LoadProfileData();
        }

        [Button, TitleGroup("ProfileDa")]
        public void LoadProfileData()
        {
            profileData = Instance.LoadObject<ProfileData>(Success);

            void Success(ProfileData data)
            {
                profileData = data;
                OnProfileDataUpdated?.Invoke(data);
            }
        }


        [Button, TitleGroup("Progress")]
        public void LoadPlayerProgress()
        {
            if (SaveUtility.HasKey(nameof(PlayerProgress)))
            {
                progress = SaveUtility.LoadObject<PlayerProgress>(nameof(PlayerProgress));
                PlayfabManager.LoadPlayerData<PlayerProgress>(nameof(PlayerProgress), (progress) => Instance.progress = progress);
            }
            else
            {
                progress = new PlayerProgress();
                progress.levelStates.Add(eLevelState.Unlocked);
                SaveProgress();
            }
        }
        
        [Button, TitleGroup("Progress")]
        public void SaveProgress()
        {
            SaveUtility.SaveObject(nameof(PlayerProgress), progress);
        }

        [Button, TitleGroup("Progress")]
        public void ClearProgress()
        {
            SaveUtility.DeleteObject(nameof(PlayerProgress));
            progress = null;
        }
        
        
        
        public T LoadObject<T>(Action<T> complete) where T : new()
        {
            T obj;
            if (SaveUtility.HasKey(typeof(T).Name))
            {
                obj = SaveUtility.LoadObject<T>(typeof(T).Name);
                PlayfabManager.LoadPlayerData<T>(typeof(T).Name, (res) =>
                {
                    obj = res;
                    complete?.Invoke(obj);
                });
            }
            else
            {
                obj = new T();
                SaveObject(obj);
                
            }
            return obj;
        }
        
        public void SaveObject<T>(T obj)
        {
            SaveUtility.SaveObject(typeof(T).Name, obj);
        }
        
        public void ClearObject<T>(ref T obj)
        {
            SaveUtility.DeleteObject(typeof(T).Name);
            obj = default(T);
        }

        [Button]
        private void ClearObject(string key)
        {
            SaveUtility.DeleteObject(key);
        }
        
    }
}
