using System;
using System.Collections.Generic;
using System.Linq;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.Social;
using PlayFab;
using PlayFab.ClientModels;
using PROJECT.Scripts.Enums;
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


        private void Start()
        {
            InitializeCatalogDictionary();
        }

        public void LoadCatalog()
        {
            PlayfabManager.LoadCatalogData(Success, Failure);

            void Success(List<CatalogItem> result)
            {
                currentCatalog = result;
                SortCatalogItems();
                OnCatalogLoadCompleted?.Invoke();
            }

            void Failure(PlayFabError error)
            {
                OnCatalogLoadFailed?.Invoke();
            }
        }

        private void SortCatalogItems()
        {
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
    }
}