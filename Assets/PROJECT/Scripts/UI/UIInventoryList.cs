using System;
using System.Collections.Generic;
using System.Linq;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.GameConfig;
using PlayFab.ClientModels;
using PROJECT.Scripts.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PROJECT.Scripts.UI
{
    public class UIInventoryList : UIBehaviour
    {
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private RectTransform toggleParent;
        [SerializeField, ReadOnly] private List<ItemInstance> itemList;
        [SerializeField, ReadOnly] private List<Toggle> toggleList;
        
        private IEnumerable<Tuple<Toggle, ItemInstance>> _zippedList;
        private eItemType _listType;
        private int _dummySpriteIndex;

        private int _currentToggleIndex;

        public event Action<eItemType, string> OnValueChanged;
         
        public void SetItemList(IEnumerable<ItemInstance> _itemList, eItemType type)
        {
            itemList = _itemList.ToList();
            _listType = type;
            for (int i = itemList.Count(); i < toggleParent.childCount; i++)
            {
                toggleList[i].gameObject.SetActive(false);
            }
            
            _zippedList = toggleList.Zip(itemList, Tuple.Create);

            for (int i = 0; i < itemList.Count; i++)
            {
                _dummySpriteIndex = int.Parse(itemList[i].ItemId.Split("_")[1]);
                ((Image)(toggleList[i].targetGraphic)).sprite =
                    GameConfig.Instance.Shop.ShopDictionary[type].spriteList[_dummySpriteIndex];
                toggleList[i].gameObject.SetActive(true);
            }
        }

        public void TriggerOnValueChanged()
        {
            if (!gameObject.activeSelf)
                return;
            _currentToggleIndex = toggleGroup.GetFirstActiveToggle().transform.GetSiblingIndex();
            OnValueChanged?.Invoke(_listType, itemList[_currentToggleIndex].ItemId);
            gameObject.SetActive(false);
        }
            

        [Button]
        private void SetRefs()
        {
            toggleGroup = GetComponentInChildren<ToggleGroup>();
            toggleParent = transform.FindDeepChild<RectTransform>("TogglesParent");
            toggleList = toggleParent.GetComponentsInChildren<Toggle>().ToList();
        }
    }
}
