using System;
using PROJECT.Scripts.Enums;
using UnityEngine;

namespace PROJECT.Scripts.Data.Items
{
    [Serializable]
    public class GameItem_Consumable : GameItem
    {
        [SerializeField] private Sprite consumableSprite;
        [SerializeField] private int consumableCount;

        public Sprite ConsumableSprite => consumableSprite;

        public int ConsumableCount => consumableCount;

        public GameItem_Consumable(int _itemID, eItemType _itemType, string _itemName,Sprite _itemIcon, Sprite _consumableSprite) : base(_itemID, _itemType, _itemName, _itemIcon)
        {
            consumableSprite = _consumableSprite;
        }
    }
}
