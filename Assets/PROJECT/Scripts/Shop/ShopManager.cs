using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.GameConfig;
using _YajuluSDK._Scripts.Social;
using _YajuluSDK._Scripts.UI;
using PlayFab;
using PlayFab.ClientModels;
using PROJECT.Scripts.Enums;
using PROJECT.Scripts.Game.Controllers;
using UnityEngine;
using static UIPopupController;

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
        private bool purchaseLock = false;

        private CatalogItem selectedItem;

        public CatalogItem SelectedItem
        {
            get => selectedItem;
            set => selectedItem = value;
        }

        private PopupRequest popupRequest;

        private void Start()
        {
            InitializeCatalogDictionary();
            InitializePopUpRequest();
            DataPersistenceManager.OnPlayerInventoryUpdated += OnPlayerInventoryUpdated;
            PlayfabManager.OnPlayerLoggedInBasic += PlayfabManager_OnPlayerLoggedIn;
        }

        private void PlayfabManager_OnPlayerLoggedIn()
        {
            LoadCatalog();
        }

        private void OnDestroy()
        {
            DataPersistenceManager.OnPlayerInventoryUpdated -= OnPlayerInventoryUpdated;
            PlayfabManager.OnPlayerLoggedInBasic -= PlayfabManager_OnPlayerLoggedIn;
        }

        private void OnPlayerInventoryUpdated()
        {
            if (isLoadingCatalog)
            {
                isLoadingCatalog = false;
                SortCatalogItems();
                purchaseLock = false;
                OnCatalogLoadCompleted?.Invoke();
            }            
        }

        public void LoadCatalog()
        {
            isLoadingCatalog = true;
            PlayfabManager.LoadCatalogData(Success, Failure);            

            void Success(List<CatalogItem> result)
            {
                currentCatalog = result;
                DataPersistenceManager.Instance.LoadPlayerInventory();
            }

            void Failure(PlayFabError error)
            {
                isLoadingCatalog = false;
                purchaseLock = false;
                OnCatalogLoadFailed?.Invoke();
            }
        }

        private void SortCatalogItems()
        {
            Task.Run(Sorting);

            void Sorting()
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
            
        }

        private void InitializeCatalogDictionary()
        {
            catalogItemTypeDictionary  = new Dictionary<eItemType, IEnumerable<CatalogItem>>();
            foreach (eItemType itemType in Enum.GetValues(typeof(eItemType)))
            {
                CatalogItemTypeDictionary.Add(itemType, new List<CatalogItem>());
            }
        }

        private void InitializePopUpRequest()
        {
            popupRequest = new PopupRequest
            {
                IconType = PopupIconType.ItemIconNormal,
                CancelAction = (() => 
                { 
                    selectedItem = null;
                    purchaseLock = false;
                })
            };
        }

        public void PurchaseItem(string itemID, int price, string virtualCurrency)
        {
            PlayfabManager.PurchaseItem(itemID, price, virtualCurrency, Success, Failure);
            
            void Success(PurchaseItemResult result)
            {
                ShowPurchaseCompletePopup(result.Items[0]);
                LoadCatalog();
                Debug.Log("Success");
            }

            void Failure(PlayFabError error)
            {
                ShopPurchaseFailedPopup(error);
                purchaseLock = false;
                Debug.Log("Failed");
            }

        }

        public void ShowConfirmPurchasePopup()
        {
            if (purchaseLock)
                return;
            purchaseLock = true;
            InitializePopUpRequest();
            popupRequest.Icon = GameConfig.Instance.Shop.ShopItemIDDictionary[selectedItem.ItemId];
            popupRequest.Msg = selectedItem.DisplayName;
            popupRequest.ButtonsConfig = new List<PopupButtonAction>();
            PopupButtonAction buttonAction;

            foreach (var virtualCurrency in selectedItem.VirtualCurrencyPrices)
            {
                buttonAction = new PopupButtonAction
                {
                    buttonText = virtualCurrency.Value + " " + virtualCurrency.Key,
                    buttonAction = (() => { PurchaseItem(selectedItem.ItemId, (int)virtualCurrency.Value, virtualCurrency.Key); })
                };
                popupRequest.ButtonsConfig.Add(buttonAction);
            }

            PopUpManager.Instance.RequestPopUp(popupRequest);
        }

        public void ShowPurchaseCompletePopup(ItemInstance item)
        {
            var request = new PopupRequest
            {
                IconType = PopupIconType.ItemIconHighlighted,
                Icon = GameConfig.Instance.Shop.ShopItemIDDictionary[item.ItemId],
                Msg = selectedItem.DisplayName,
                ButtonsConfig = new List<PopupButtonAction>(),
            };            
  
            PopUpManager.Instance.RequestPopUp(request);
        }

        public void ShopPurchaseFailedPopup(PlayFabError error)
        {
            var msg = "";

            switch (error.Error)
            {
                case PlayFabErrorCode.InsufficientFunds:
                    msg = "لا يوجد لديك نقوة كافية";
                    break;
            }

            var request = new PopupRequest
            {
                IconType = PopupIconType.smallIcon,
                //Icon = GameConfig.Instance.Shop.ShopItemIDDictionary[selectedItem.ItemId],
                Msg = msg,
                ButtonsConfig = new List<PopupButtonAction>()
            };

            PopUpManager.Instance.RequestPopUp(request);
        }
    }
}