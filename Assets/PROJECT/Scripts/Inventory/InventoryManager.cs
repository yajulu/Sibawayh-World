using System;
using _YajuluSDK._Scripts.Essentials;
using PROJECT.Scripts.Enums;
using PROJECT.Scripts.Game.Controllers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [SerializeField] private ProfileData currentProfile;

        private void Start()
        {
            DataPersistenceManager.OnProfileDataUpdated += OnProfileUpdated;
        }

        private void OnDestroy()
        {
            DataPersistenceManager.OnProfileDataUpdated -= OnProfileUpdated;
        }

        private void OnProfileUpdated(ProfileData profile)
        {
            currentProfile = profile;
        }
    }

    [Serializable]
    public class ProfileData
    {
        [SerializeField] private ProfileItem banner;
        [SerializeField] private ProfileItem icon;
        [SerializeField] private ProfileItem companion; 
        

        public ProfileData()
        {
            banner = new ProfileItem(eItemType.Banner);
            icon = new ProfileItem(eItemType.PlayerIcon);
            companion = new ProfileItem(eItemType.Companion);
        }

        public ProfileItem Banner => banner;

        public ProfileItem Icon => icon;

        public ProfileItem Companion => companion;

        public ProfileItem GetProfileItemWithType(eItemType type)
        {
            ProfileItem item;
            switch (type)
            {
                case eItemType.PlayerIcon:
                    item = icon;
                    break;
                case eItemType.Banner:
                    item = banner;
                    break;
                case eItemType.Companion:
                    item = companion;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return item;
        }
    }

    [Serializable]
    public class ProfileItem
    {
        [SerializeField, ReadOnly] private eItemType _type;
        [SerializeField] private string _itemID;
        [SerializeField, ReadOnly]private int _index;
        
        public eItemType Type => _type;
        public int Index => _index;
        
        public string ItemID
        {
            get => _itemID;
            set
            {
                _itemID = value;
                _index = int.Parse(value.Split("_")[1]);
            }
        }
        
        public ProfileItem(eItemType type)
        {
            _type = type;
            _itemID = string.Concat(type.ToString().ToLower(),"_0");
            _index = 0;
        }

    }
}
