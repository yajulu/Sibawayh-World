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
        
        public GameItem_Banner(int itemID, eItemType type, string displayName, Sprite icon, Sprite bannerSprite) : base(itemID, type, displayName, icon)
        {
            this.bannerSprite = bannerSprite;
        }
    }
}
