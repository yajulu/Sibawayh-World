using System.Collections.Generic;
using PlayFab.ClientModels;
using PROJECT.Scripts.Data.Items;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _YajuluSDK._Scripts.Shop
{
    public class ShopCreator 
    {
        [SerializeField] private CatalogInstance _catalogInstance;
        [SerializeField, TextArea] private string Json;

        [SerializeField] private List<GameItem> items;
        
        [Button]
        private void CreateShopJson()
        {
            
            
        }
    }

    public class CatalogInstance
    {
        public List<ItemInstance> Catalog;
    }
}
