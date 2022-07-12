using System;
using System.Collections.Generic;
using System.Linq;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.Social;
using PlayFab;
using PlayFab.ClientModels;
using PROJECT.Scripts.Enums;
using PROJECT.Scripts.Game.Controllers;
using UnityEngine;

namespace PROJECT.Scripts.Shop
{
    public class ShopManager : Singleton<ShopManager>
    {

        [SerializeField] private List<CatalogItem> currentCatalog;
        
        public List<CatalogItem> CurrentCatalog => currentCatalog;

        public Dictionary<eItemType, IEnumerable<CatalogItem>> CatalogItemTypeDictionary => catalogItemTypeDictionary;

        public static event Action OnCatalogLoadCompleted;

        public static event Action OnCatalogLoadFailed;

        private Dictionary<eItemType, IEnumerable<CatalogItem>> catalogItemTypeDictionary;

        private bool isLoadingCatalog = false;


        private void Start()
        {
            InitializeCatalogDictionary();
            DataPersistenceManager.OnPlayerInventoryUpdated += OnPlayerInventoryUpdated;
        }

        private void OnDestroy()
        {
            DataPersistenceManager.OnPlayerInventoryUpdated -= OnPlayerInventoryUpdated;
        }

        private void OnPlayerInventoryUpdated()
        {
            if (isLoadingCatalog)
            {
                isLoadingCatalog = false;
                SortCatalogItems();
                OnCatalogLoadCompleted?.Invoke();
            }            
        }

        public void LoadCatalog()
        {
            PlayfabManager.LoadCatalogData(Success, Failure);            

            void Success(List<CatalogItem> result)
            {
                currentCatalog = result;
                isLoadingCatalog = true;
                DataPersistenceManager.Instance.LoadPlayerInventory();
            }

            void Failure(PlayFabError error)
            {
                OnCatalogLoadFailed?.Invoke();
            }
        }

        private void SortCatalogItems()
        {
            var inventoryListIDs = DataPersistenceManager.Instance.Inventory
                .Where(item => item.ItemClass.Equals("Cosmetics"))
                .Select(item => item.ItemId);            

            foreach( var item in currentCatalog)
            {
                if (inventoryListIDs.Contains(item.ItemId))
                {
                    item.Tags.Add("Owned");
                }
            }

            foreach (var itemType in Enum.GetNames(typeof(eItemType)))
            {
                var type = Enum.Parse<eItemType>(itemType);
                CatalogItemTypeDictionary[type] = currentCatalog.Where(item => item.Tags[0].Equals(itemType));
            }
        }

        private void InitializeCatalogDictionary()
        {
            catalogItemTypeDictionary  = new Dictionary<eItemType, IEnumerable<CatalogItem>>();
            foreach (eItemType itemType in Enum.GetValues(typeof(eItemType)))
            {
                CatalogItemTypeDictionary.Add(itemType, new List<CatalogItem>());
            }
        }

        public void PurchaseItem(string itemID, int price, string virtualCurrency)
        {
            PlayfabManager.PurchaseItem(itemID, price, virtualCurrency, Success, Failure);
            
            void Success(PurchaseItemResult result)
            {
                Debug.Log("Success");
            }

            void Failure(PlayFabError error)
            {
                Debug.Log("Failed");
            }

        }

        
    }
}