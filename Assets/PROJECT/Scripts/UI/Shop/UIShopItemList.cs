using System.Collections.Generic;
using System.Linq;
using PlayFab.ClientModels;
using PROJECT.Scripts.Enums;
using PROJECT.Scripts.Shop;
using Project.Scripts.UI.Shop;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace PROJECT.Scripts.UI.Shop
{
    public class UIShopItemList : MonoBehaviour
    {
        [SerializeField] private eItemType[] listTypes;
        [SerializeField] private List<UIShopItem> shopItemList;

        public eItemType[] ListTypes => listTypes;


        public void RefreshShopList()
        {
            var index = 0;
            foreach (var type in listTypes)
            {
                var catalogItems = ShopManager.Instance.CatalogItemTypeDictionary[type];
                foreach (var item in catalogItems)
                {
                    if (index >= shopItemList.Count)
                    {
                        Debug.LogWarning($"{this.name} is full, instantiating an new element.");
                        shopItemList.Add(Instantiate(shopItemList[0], shopItemList[0].transform.parent));
                    }
                    shopItemList[index].ShopCatalogItem = item;
                    index++;
                }
            }

            for (var i = index; i < shopItemList.Count; i++)
            {
                shopItemList[i].gameObject.SetActive(false);
            }
        }

        [Button]
        private void SetRefs()
        {
            shopItemList = GetComponentsInChildren<UIShopItem>().ToList();
        }
    }
}