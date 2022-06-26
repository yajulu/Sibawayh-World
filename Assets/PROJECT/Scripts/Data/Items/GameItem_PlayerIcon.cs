using System;
using PROJECT.Scripts.Enums;
using UnityEngine;

namespace PROJECT.Scripts.Data.Items
{
    [Serializable]
    public class GameItem_PlayerIcon : GameItem
    {
      
        public GameItem_PlayerIcon(int _itemID, eItemType type, string displayName,Sprite icon) : base(_itemID, type, displayName, icon)
        {
        }
    }
}

