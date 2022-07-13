using System;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.GameConfig;
using _YajuluSDK._Scripts.Social;
using _YajuluSDK._Scripts.UI;
using PROJECT.Scripts.Enums;
using PROJECT.Scripts.Game.Controllers;
using Project.Scripts.Inventory;
using RTLTMPro;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PROJECT.Scripts.UI.Screens
{
    public class Screen_Profile : UIScreenBase
    {

        [SerializeField] private PlayerCardController playerDisplayCard;

        protected override void Awake()
        {
            base.Awake();
            DataPersistenceManager.OnProfileDataUpdated += OnProfileDataUpdated;
            DataPersistenceManager.OnPlayerDisplayNameUpdated += DataPersistenceManager_OnPlayerDisplayNameUpdated;
        }

        private void OnDestroy()
        {
            DataPersistenceManager.OnProfileDataUpdated -= OnProfileDataUpdated;
            DataPersistenceManager.OnPlayerDisplayNameUpdated -= DataPersistenceManager_OnPlayerDisplayNameUpdated;
        }

        private void OnProfileDataUpdated(ProfileData profileData)
        {
            playerDisplayCard.SetPlayerProfileItems(
                GameConfig.Instance.Shop.ShopDictionary[eItemType.Banner].spriteList[profileData.Banner.Index],
                GameConfig.Instance.Shop.ShopDictionary[eItemType.PlayerIcon].spriteList[profileData.Icon.Index]);
        }


        private void DataPersistenceManager_OnPlayerDisplayNameUpdated(string newName)
        {
            playerDisplayCard.UpdatePlayerDisplayName(newName);
        }


        protected override void OnScreenOpenStarted()
        {
            if (!DataPersistenceManager.Instance.IsPlayerUpdating)
                DataPersistenceManager.Instance.LoadProfileData();
            playerDisplayCard.UpdatePlayerDisplayName(DataPersistenceManager.Instance.PlayerDisplayName);
            OnProfileDataUpdated(DataPersistenceManager.Instance.ProfileData);
            DataPersistenceManager.Instance.LoadPlayerInventory();
            base.OnScreenOpenStarted();
        }

        [Button]
        private void SetRefs()
        {
            playerDisplayCard = transform.FindDeepChild<PlayerCardController>("Profile_Panel");
        }
    }
}
