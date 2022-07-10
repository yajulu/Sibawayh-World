using System;
using System.Collections.Generic;
using System.Linq;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.UI;
using DG.Tweening;
using PlayFab.ClientModels;
using PROJECT.Scripts.Shop;
using PROJECT.Scripts.UI.Shop;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace PROJECT.Scripts.UI.Screens
{
    public class Panel_Shop : UIPanelBase
    {
        [SerializeField] private UIShopItemList [] itemList;
        
        private void Start()
        {
            ShopManager.OnCatalogLoadCompleted += ShopManagerOnCatalogLoadCompleted;
        }

        private void OnDestroy()
        {
            ShopManager.OnCatalogLoadCompleted += ShopManagerOnCatalogLoadCompleted;
        }

        private void ShopManagerOnCatalogLoadCompleted()
        {
            foreach (var list in itemList)
            {
                list.RefreshShopList();
            }
        }

        protected override void OnScreenOpenStarted()
        {
            ShopManager.Instance.LoadCatalog();
            base.OnScreenOpenStarted();
        }


        [Button]
        protected override void SetRefs()
        {
            base.SetRefs();
            itemList = GetComponentsInChildren<UIShopItemList>();
        }
    }
}
