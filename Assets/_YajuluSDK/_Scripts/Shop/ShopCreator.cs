using System.Collections.Generic;
using PlayFab.ClientModels;
using PROJECT.Scripts.Data.Items;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _YajuluSDK._Scripts.Shop
{
    [CreateAssetMenu()]
    public class ShopCreator : ScriptableObject
    {
        [SerializeField] private CatalogInstance _catalogInstance;
        [SerializeField, TextArea] private string Json;

        [SerializeField] private List<GameItem> items;
        
        
        
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        [Button]
        private void CreateShopJSON()
        {
            var list = new List<CatalogItem>();
            
        }
    }

    public class CatalogInstance
    {
        public List<ItemInstance> Catalog;
    }
}
