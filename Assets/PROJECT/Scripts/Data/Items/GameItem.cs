using System;
using PROJECT.Scripts.Enums;
using UnityEngine;

namespace PROJECT.Scripts.Data.Items
{
    [Serializable]
    public class GameItem
    {
        [SerializeField] public int itemID;
        [SerializeField] public eItemType itemType;
        [SerializeField] public string itemName;
        [SerializeField] public Sprite itemIcon;
        [SerializeField] public bool isItemOwned;


        public GameItem()
        {
            itemID = 0;
            itemType = eItemType.Consumable;
            itemName = "";
            itemIcon = null;

        }
        
        public GameItem(int _itemID, eItemType _itemType, string _itemName, Sprite _itemIcon)
        {
            itemID = _itemID;
            itemType = _itemType;
            itemName = _itemName;
            itemIcon = _itemIcon;


        }
    }
    
    
 
}
