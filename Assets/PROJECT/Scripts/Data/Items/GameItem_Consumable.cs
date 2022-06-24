using System;
using PROJECT.Scripts.Enums;
using UnityEngine;

namespace PROJECT.Scripts.Data.Items
{
    [Serializable]
    public class GameItem_Consumable : GameItem
    {
        [SerializeField] public Sprite consumableSprite;
        [SerializeField] public int consumableCount;

        public GameItem_Consumable(int _itemID, eItemType _itemType, string _itemName,Sprite _itemIcon, Sprite _consumableSprite)
        {
            itemID = _itemID;
            itemType = _itemType;
            itemName = _itemName;
            itemIcon = _itemIcon;
            consumableSprite = _consumableSprite;
        }
    }
}
