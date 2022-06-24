using System;
using PROJECT.Scripts.Enums;
using UnityEngine;

namespace PROJECT.Scripts.Data.Items
{
    [Serializable]
    public class GameItem_PlayerIcon : GameItem
    {
      
        public GameItem_PlayerIcon(int _itemID, eItemType _itemType, string _itemName,Sprite _itemIcon)
        {
            itemID = _itemID;
            itemType = _itemType;
            itemName = _itemName;
            itemIcon = _itemIcon;
        }
    }
}

