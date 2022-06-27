using System;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.GameConfig;
using _YajuluSDK._Scripts.Social;
using _YajuluSDK._Scripts.UI;
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
            DataPersistenceManager.OnProfileDataUpdated += OnProfileFataUpdated;
        }

        private void OnDestroy()
        {
            DataPersistenceManager.OnProfileDataUpdated -= OnProfileFataUpdated;
        }

        private void OnProfileFataUpdated(ProfileData profileData)
        {
            playerDisplayCard.SetPlayerProfileItems(
                GameConfig.Instance.Shop.ShopDictionary[profileData.Banner.Type].spriteList[profileData.Banner.Index],
                GameConfig.Instance.Shop.ShopDictionary[profileData.Icon.Type].spriteList[profileData.Icon.Index]);
        }

        protected override void OnScreenOpenStarted()
        {
            playerDisplayCard.UpdatePlayerDisplayName(PlayFabHandler.Instance.CachedPlayer.DisplayName);
            OnProfileFataUpdated(DataPersistenceManager.Instance.ProfileData);
            base.OnScreenOpenStarted();
        }

        [Button]
        private void SetRefs()
        {
            playerDisplayCard = transform.FindDeepChild<PlayerCardController>("Profile_Panel");
        }
    }
}
