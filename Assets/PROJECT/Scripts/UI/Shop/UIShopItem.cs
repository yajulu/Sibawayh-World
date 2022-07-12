using System;
using System.Linq;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.GameConfig;
using _YajuluSDK._Scripts.UI;
using PlayFab.ClientModels;
using PROJECT.Scripts.Shop;
using PROJECT.Scripts.UI.Shop;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Shop
{
    public class UIShopItem : UIElementBase
    {
        [SerializeField] private Image icon;
        [SerializeField] private Button button;
        [SerializeField] private RectTransform pricePanel;
        [SerializeField] private UIPricePanel [] currencyPanelList;

        [SerializeField, ReadOnly] private CatalogItem shopCatalogItem;

        private void OnEnable()
        {
            button.onClick.AddListener(PurchaseCurrentItem);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(PurchaseCurrentItem);
        }

        public CatalogItem ShopCatalogItem
        {
            get => shopCatalogItem;
            set
            {
                RefreshShopItemCard(value);
                shopCatalogItem = value;
            }
        }

        private void RefreshShopItemCard(CatalogItem item)
        {
            var index = 0;

            if (item.Tags.Contains("Owned"))
            {
                button.interactable = false;
                pricePanel.gameObject.SetActive(false);
            }
            else
            {
                button.interactable = true;
                pricePanel.gameObject.SetActive(true);
            }

            foreach (var currency in GameConfig.Instance.Shop.VirtualCurrenciesDict.Keys)
            {
                
                if (item.VirtualCurrencyPrices.ContainsKey(currency))
                {
                    currencyPanelList[index].gameObject.SetActive(true);
                    currencyPanelList[index].SetCurrencyPrice(currency, item.VirtualCurrencyPrices[currency]);
                }
                else
                {
                    currencyPanelList[index].gameObject.SetActive(false);
                }
                index++;
            }

            icon.sprite = GameConfig.Instance.Shop.ShopItemIDDictionary[item.ItemId];
        }

        private void PurchaseCurrentItem()
        {
            ShopManager.Instance.PurchaseItem(shopCatalogItem.ItemId, (int) shopCatalogItem.VirtualCurrencyPrices["YC"], "YC");
        }

        [Button]
        private void SetRefs()
        {
            button = GetComponentInChildren<Button>();
            pricePanel = transform.FindDeepChild<RectTransform>("ShopItemPrice_Panel");
            currencyPanelList = pricePanel.GetComponentsInChildren<UIPricePanel>();
            icon = transform.FindDeepChild<Image>("ShopItemIcon_Image");
        }
        
    }
}
