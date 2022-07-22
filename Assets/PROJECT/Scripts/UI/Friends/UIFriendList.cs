using System;
using System.Collections.Generic;
using System.Linq;
using _YajuluSDK._Scripts.Social;
using _YajuluSDK._Scripts.UI;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using Project.Scripts.Inventory;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PROJECT.Scripts.UI.Friends
{
    public class UIFriendList : MonoBehaviour
    {
        [SerializeField] private List<FriendInfo> currentFriendList;
        [SerializeField] private List<UIFriendCard> friendCardList;

        private ProfileData _dummyProfileData;

        public List<FriendInfo> CurrentFriendList
        {
            get => currentFriendList;
            set
            {
                currentFriendList = value;
                UpdateFriendListUI();
            }
        }
        
        public void LoadFriends()
        {
            PlayfabManager.GetFriendsList(Success, Failure);

            void Success (List<FriendInfo> list)
            {
                CurrentFriendList = list;
            }

            void Failure(PlayFabError error)
            {
                //TODO: request a popup
                Debug.Log("Loading Friends Failed.");
            }
        }

        private void UpdateFriendListUI()
        {
            var index = 0;

            for (; index < currentFriendList.Count; index++)
            {
                var friend = currentFriendList[index];
                if (index >= friendCardList.Count)
                {
                    Debug.LogWarning($"{this.name} is full, instantiating an new element.");
                    friendCardList.Add(Instantiate(friendCardList[0], friendCardList[0].transform.parent));
                }

                try
                {
                    var list = friend.Profile.AvatarUrl.Split("/");
                    _dummyProfileData = new ProfileData
                    {
                        Banner = { ItemID = list[1] },
                        Icon = { ItemID = list[2] },
                        Companion = { ItemID = list[3] }
                    };
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.ToString());
                    _dummyProfileData = new ProfileData();
                }

                friendCardList[index].SetPlayerData(friend.TitleDisplayName, 1, _dummyProfileData);
            }

            for (var i = index; i < friendCardList.Count; i++)
            {
                friendCardList[i].gameObject.SetActive(false);
            }
        }

        [Button]
        protected void SetRefs()
        {
            friendCardList = GetComponentsInChildren<UIFriendCard>().ToList();
        }
    }
}
