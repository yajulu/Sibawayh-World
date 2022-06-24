using System;
using PROJECT.Scripts.Enums;
using UnityEngine;

namespace PROJECT.Scripts.Data.Items
{
    [Serializable]
    public class GameItem_Banner : GameItem
    {
        [SerializeField] private Sprite bannerSprite;
        
        public Sprite BannerSprite => bannerSprite;
        
        public GameItem_Banner(int _itemID, eItemType _itemType, string _itemName,Sprite _itemIcon, Sprite _bannerSprite) : base(_itemID, _itemType, _itemName, _itemIcon)
        {
            bannerSprite = _bannerSprite;
        }
    }
}
