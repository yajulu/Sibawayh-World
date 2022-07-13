using System;
using System.Collections.Generic;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.GameConfig;
using _YajuluSDK._Scripts.Social;
using _YajuluSDK._Scripts.Tools;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.CloudScriptModels;
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
        [SerializeField] private string playerDisplayName;

        public PlayerProgress Progress => progress;
        public ProfileData ProfileData
        {
            get => profileData;
            private set
            {
                profileData = value;
                OnProfileDataUpdated?.Invoke(value);
            }
        }

        public string PlayerDisplayName
        {
            get => playerDisplayName;
            private set
            {
                playerDisplayName = value;
                OnPlayerDisplayNameUpdated?.Invoke(value);
            }
        }

        public static event Action<PlayerProgress> OnPlayerProgressUpdated;
        public static event Action<ProfileData> OnProfileDataUpdated;
        public static event Action<string> OnPlayerDisplayNameUpdated;
        public static event Action OnPlayerInventoryUpdated;

        private List<ItemInstance> _inventory;
        private Dictionary<string, int> _virtualCurrency;

        public List<ItemInstance> Inventory => _inventory;

        public Dictionary<string, int> VirtualCurrency => _virtualCurrency;

        public bool IsPlayerUpdating => _isPlayerUpdating;

        private bool _isPlayerUpdating = false;

        private void Start()
        {
            PlayfabManager.OnPlayerLoggedInBasic += LoadPlayerData;
        }

        private void LoadPlayerData()
        {
            LoadPlayerProgress();
            LoadProfileData();
            LoadPlayerInventory();
            LoadPlayerDisplayName();
        }

        public void LoadPlayerInventory()
        {
            PlayfabManager.LoadPlayerInventory(Success);

            void Success(GetUserInventoryResult result)
            {
                _inventory = result.Inventory;
                _virtualCurrency = result.VirtualCurrency;
                OnPlayerInventoryUpdated?.Invoke();
            }

            void Failure(PlayFabError error)
            {
                
            }
        }

        private void LoadPlayerDisplayName()
        {
            PlayfabManager.GetPlayerData(null, Success);

            void Success(GetPlayerProfileResult obj)
            {
                PlayerDisplayName = obj.PlayerProfile.DisplayName;
            }
        }


        public void UpdateLocalPlayerData(ProfileData newProfileData)
        {
            OnProfileDataUpdated?.Invoke(newProfileData);
        }

        public void UpdateOnlinePlayerData(ProfileData newProfileData, string newName)
        {
            if (_isPlayerUpdating)
                return;
            _isPlayerUpdating = true;
            UpdatePlayerGameProfileData(newProfileData);
            UpdatePlayerDisplayName(newName);
        }

        private void UpdatePlayerGameProfileData(ProfileData newProfileData)
        {
            PlayfabManager.UpdatePlayerGameProfile(newProfileData, Success, Failure);
            ProfileData = newProfileData;
            void Success(ExecuteFunctionResult result)
            {
                _isPlayerUpdating = false;
                var dict = (Dictionary<string, object>) JsonConvert.DeserializeObject<Dictionary<string, object>>(result.FunctionResult.ToString());
                if ((bool)dict["profileUpdated"])
                {
                    profileData = JsonConvert.DeserializeObject<ProfileData>(dict["currentProfile"].ToString());
                    OnProfileDataUpdated?.Invoke(profileData);
                }
            }
            
            void Failure(PlayFabError error)
            {
                _isPlayerUpdating = false;
            }
        }

        public void UpdateLocalPlayerName(string name)
        {
            PlayerDisplayName = name;
        }


        private void UpdatePlayerDisplayName(string newName)
        {
            PlayfabManager.UpdatePlayerDisplayName(newName, Success);
            PlayerDisplayName = newName;

            void Success(UpdateUserTitleDisplayNameResult obj)
            {
                OnPlayerDisplayNameUpdated?.Invoke(obj.DisplayName);
            }
        }


        [Button, TitleGroup("ProfileDa")]
        public void LoadProfileData()
        {
            PlayfabManager.LoadPlayerReadOnlyData<ProfileData>(nameof(ProfileData), Success);

            void Success(ProfileData data)
            {
                ProfileData = data;
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
                PlayfabManager.LoadPlayerReadOnlyData<T>(typeof(T).Name, (res) =>
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
