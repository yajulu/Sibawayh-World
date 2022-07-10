using System.Linq;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.GameConfig;
using _YajuluSDK._Scripts.UI;
using PlayFab.ClientModels;
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
