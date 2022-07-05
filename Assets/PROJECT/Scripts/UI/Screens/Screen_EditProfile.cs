using System;
using System.Collections.Generic;
using System.Linq;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.GameConfig;
using _YajuluSDK._Scripts.UI;
using PlayFab.ClientModels;
using PROJECT.Scripts.Enums;
using PROJECT.Scripts.Game.Controllers;
using Project.Scripts.Inventory;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace PROJECT.Scripts.UI.Screens
{
    public class Screen_EditProfile : UIScreenBase
    {
        [SerializeField] private UIInventoryList iconsList;
        [SerializeField] private UIInventoryList bannersList;

        [SerializeField] private Image playerIconImage;
        [SerializeField] private Image bannerImage;

        private List<ItemInstance> _inventoryCosmetics;
        private ProfileData _currentProfile;
        private void Start()
        {
            DataPersistenceManager.OnPlayerInventoryUpdated += RefreshInventory;
            DataPersistenceManager.OnProfileDataUpdated += OnProfileDataUpdated;
            iconsList.OnValueChanged += OnInventoryListSelectionChanged;
            bannersList.OnValueChanged += OnInventoryListSelectionChanged;
        }
        
        private void OnDestroy()
        {
            DataPersistenceManager.OnPlayerInventoryUpdated -= RefreshInventory;
            iconsList.OnValueChanged -= OnInventoryListSelectionChanged;
            bannersList.OnValueChanged -= OnInventoryListSelectionChanged;
        }

        private void RefreshInventory()
        {
            _inventoryCosmetics = DataPersistenceManager.Instance.Inventory;
            
            _inventoryCosmetics = DataPersistenceManager.Instance.Inventory.Where(instance => instance.ItemClass.Equals("Cosmetics")).ToList();
            
            iconsList.SetItemList(_inventoryCosmetics.Where(instance => instance.ItemId.Contains("playericon")), eItemType.PlayerIcon);
            bannersList.SetItemList(_inventoryCosmetics.Where(instance => instance.ItemId.Contains("banner")), eItemType.Banner);
        }

        private void OnInventoryListSelectionChanged(eItemType type, string itemId)
        {
            switch (type)
            {
                case eItemType.PlayerIcon:
                    _currentProfile.Icon.ItemID = itemId;
                    break;
                case eItemType.Banner:
                    _currentProfile.Banner.ItemID = itemId;
                    break;
                case eItemType.Companion:
                    _currentProfile.Companion.ItemID = itemId;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            DataPersistenceManager.Instance.UpdateLocalPlayerData(_currentProfile);
        }

        protected override void OnScreenOpenStarted()
        {
            _currentProfile = DataPersistenceManager.Instance.ProfileData;
            RefreshInventory();
            OnProfileDataUpdated(_currentProfile);
            base.OnScreenOpenStarted();
        }

        protected override void OnScreenCloseStarted()
        {
            DataPersistenceManager.Instance.UpdatePlayerGameProfileData(_currentProfile);
            base.OnScreenCloseStarted();
        }
        
        private void OnProfileDataUpdated(ProfileData newProfile)
        {
            playerIconImage.sprite = GameConfig.Instance.Shop.ShopDictionary[eItemType.PlayerIcon]
                .spriteList[newProfile.Icon.Index];
            bannerImage.sprite = GameConfig.Instance.Shop.ShopDictionary[eItemType.Banner]
                .spriteList[newProfile.Banner.Index];
        }

        [Button]
        private void SetRefs()
        {
            iconsList = transform.FindDeepChild<UIInventoryList>("EditIcon_PopUp");
            bannersList = transform.FindDeepChild<UIInventoryList>("EditBanner_PopUp");
            bannerImage = transform.FindDeepChild<Image>("Banner_Image");
            playerIconImage = transform.FindDeepChild<Image>("PlayerIcon_Image");
        }
    }
}