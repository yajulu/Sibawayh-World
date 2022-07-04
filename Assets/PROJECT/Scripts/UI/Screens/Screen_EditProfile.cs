using System;
using System.Collections.Generic;
using System.Linq;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.UI;
using PlayFab.ClientModels;
using PROJECT.Scripts.Enums;
using PROJECT.Scripts.Game.Controllers;
using Project.Scripts.Inventory;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PROJECT.Scripts.UI.Screens
{
    public class Screen_EditProfile : UIScreenBase
    {
        [SerializeField] private UIInventoryList iconsList;
        [SerializeField] private UIInventoryList bannersList;

        private List<ItemInstance> _inventoryCosmetics;
        
        private void Start()
        {
            DataPersistenceManager.OnPlayerInventoryUpdated += RefreshInventory;
            
        }

        private void OnDestroy()
        {
            DataPersistenceManager.OnPlayerInventoryUpdated -= RefreshInventory;
        }

        private void RefreshInventory()
        {
            _inventoryCosmetics = DataPersistenceManager.Instance.Inventory;
            
            _inventoryCosmetics = DataPersistenceManager.Instance.Inventory.Where(instance => instance.ItemClass.Equals("Cosmetics")).ToList();
            
            iconsList.SetItemList(_inventoryCosmetics.Where(instance => instance.ItemId.Contains("playericon")), eItemType.PlayerIcon);
            bannersList.SetItemList(_inventoryCosmetics.Where(instance => instance.ItemId.Contains("banner")), eItemType.Banner);
        }

        protected override bool OnScreenPreOpen()
        {
            RefreshInventory();
            return true;
        }

        protected override bool OnScreenPreClose()
        {
            DataPersistenceManager.Instance.UpdatePlayerGameProfileData(new ProfileData());
            return base.OnScreenPreClose();
        }

        [Button]
        private void SetRefs()
        {
            iconsList = transform.FindDeepChild<UIInventoryList>("EditIcon_PopUp");
            bannersList = transform.FindDeepChild<UIInventoryList>("EditBanner_PopUp");
        }
    }
}