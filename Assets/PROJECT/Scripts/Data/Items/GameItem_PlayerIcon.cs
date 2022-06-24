using System.Collections;
using System.Collections.Generic;
using PROJECT.Scripts.Data;
using PROJECT.Scripts.Enums;
using UnityEngine;

public class GameItem_PlayerIcon : GameItem
{
    public GameItem_PlayerIcon(int _itemID, eItemType _itemType, string _itemName)
    {
        itemID = _itemID;
        itemType = _itemType;
        itemName = _itemName;
        itemIcon = null;
    }
    
    public GameItem_PlayerIcon(int _itemID, eItemType _itemType, string _itemName,Sprite _itemIcon)
    {
        itemID = _itemID;
        itemType = _itemType;
        itemName = _itemName;
        itemIcon = _itemIcon;
    }
}

