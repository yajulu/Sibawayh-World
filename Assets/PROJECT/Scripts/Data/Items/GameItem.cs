using System;
using PROJECT.Scripts.Enums;
using UnityEngine;

namespace PROJECT.Scripts.Data.Items
{
    [Serializable]
    public class GameItem
    {
        [SerializeField] private int itemID;
        [SerializeField] private eItemType type;
        [SerializeField] private string displayName;
        [SerializeField] private Sprite icon;
        [SerializeField] private bool isItemOwned;

        public int ItemID => itemID;

        public eItemType Type => type;

        public string DisplayName => displayName;

        public Sprite Icon => icon;

        public bool IsItemOwned => isItemOwned;
        
        public GameItem(int itemID, eItemType type, string displayName, Sprite icon)
        {
            this.itemID = itemID;
            this.type = type;
            this.displayName = displayName;
            this.icon = icon;
        }

    }
    
    
 
}
