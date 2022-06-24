using System;
using PROJECT.Scripts.Enums;
using UnityEngine;

namespace PROJECT.Scripts.Data.Items
{
    [Serializable]
    public class GameItem
    {
        [SerializeField] private int itemID;
        [SerializeField] private eItemType itemType;
        [SerializeField] private string itemName;
        [SerializeField] private Sprite itemIcon;
        [SerializeField] private bool isItemOwned;

        public int ItemID => itemID;

        public eItemType ItemType => itemType;

        public string ItemName => itemName;

        public Sprite ItemIcon => itemIcon;

        public bool IsItemOwned => isItemOwned;
        
        public GameItem(int _itemID, eItemType _itemType, string _itemName, Sprite _itemIcon)
        {
            itemID = _itemID;
            itemType = _itemType;
            itemName = _itemName;
            itemIcon = _itemIcon;
        }
    }
    
    
 
}
