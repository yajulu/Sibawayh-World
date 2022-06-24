using System;
using PROJECT.Scripts.Enums;
using UnityEngine;

namespace PROJECT.Scripts.Data.Items
{
    [Serializable]
    public class GameItem_Companion : GameItem
    {
        
        [SerializeField] public Sprite companionSprite;
        [SerializeField] public GameObject companionPrefab;

        public GameItem_Companion(int _itemID, eItemType _itemType, string _itemName,Sprite _itemIcon, Sprite _companionSprite, GameObject _companionPrefab)
        {
            itemID = _itemID;
            itemType = _itemType;
            itemName = _itemName;
            itemIcon = _itemIcon;
            companionSprite = _companionSprite;
            companionPrefab = _companionPrefab;
        }
    }
}
