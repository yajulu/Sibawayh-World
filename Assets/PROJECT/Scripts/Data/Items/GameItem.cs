using System;
using System.Collections;
using System.Collections.Generic;
using PROJECT.Scripts.Enums;
using UnityEngine;

namespace PROJECT.Scripts.Data
{
    [Serializable]
    public class GameItem
    {
        [SerializeField] public int itemID;
        [SerializeField] public eItemType itemType;
        [SerializeField] public string itemName;
        [SerializeField] public Sprite itemIcon;


        public GameItem()
        {
            itemID = 0;
            itemType = eItemType.Consumable;
            itemName = "";
            itemIcon = null;

        }
        
        public GameItem(int _itemID, eItemType _itemType, string _itemName)
        {
            _itemID = itemID;
            _itemType = itemType;
            _itemName = itemName;
            itemIcon = null;


        }
    }
    
    
 
}
