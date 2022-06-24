using System;
using PROJECT.Scripts.Enums;
using UnityEngine;

namespace PROJECT.Scripts.Data.Items
{
    [Serializable]
    public class GameItem_Banner : GameItem
    {
        [SerializeField] public Sprite bannerSprite;

        public GameItem_Banner(int _itemID, eItemType _itemType, string _itemName,Sprite _itemIcon, Sprite _bannerSprite)
        {
            itemID = _itemID;
            itemType = _itemType;
            itemName = _itemName;
            itemIcon = _itemIcon;
            bannerSprite = _bannerSprite;
        }
    }
}
