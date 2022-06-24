using System;
using PROJECT.Scripts.Enums;
using UnityEngine;

namespace PROJECT.Scripts.Data.Items
{
    [Serializable]
    public class GameItem_PlayerIcon : GameItem
    {
      
        public GameItem_PlayerIcon(int _itemID, eItemType _itemType, string _itemName,Sprite _itemIcon) : base(_itemID, _itemType, _itemName, _itemIcon)
        {
        }
    }
}

