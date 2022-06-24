using System;
using PROJECT.Scripts.Enums;
using UnityEngine;

namespace PROJECT.Scripts.Data.Items
{
    [Serializable]
    public class GameItem_Companion : GameItem
    {
        
        [SerializeField] private Sprite companionSprite;
        [SerializeField] private GameObject companionPrefab;

        public Sprite CompanionSprite => companionSprite;

        public GameObject CompanionPrefab => companionPrefab;

        public GameItem_Companion(int _itemID, eItemType _itemType, string _itemName,Sprite _itemIcon, Sprite _companionSprite, GameObject _companionPrefab) : base(_itemID, _itemType, _itemName, _itemIcon)
        {
            companionSprite = _companionSprite;
            companionPrefab = _companionPrefab;
        }
    }
}
