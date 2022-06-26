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

        public GameItem_Consumable(int _itemID, eItemType type, string displayName,Sprite icon, Sprite _consumableSprite) : base(_itemID, type, displayName, icon)
        {
            consumableSprite = _consumableSprite;
        }
    }
}
