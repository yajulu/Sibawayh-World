using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PlayFab.ClientModels;
using Project.Scripts.Inventory;
using UnityEngine;

namespace PROJECT.Scripts.UI.Friends
{
    public class UIFriendList : MonoBehaviour
    {
        [SerializeField] private List<FriendInfo> currentFriendList;
        [SerializeField] private List<UIFriendCard> friendCardList;

        private ProfileData _dummyProfileData;

        public List<UIFriendCard> FriendCardList
        {
            get => friendCardList;
            set
            {
                friendCardList = value;
                UpdateFriendListUI();
            }
        }

        private void UpdateFriendListUI()
        {
            var index = 0;

            foreach (var friend in currentFriendList)
            {
                if (index >= friendCardList.Count)
                {
                    Debug.LogWarning($"{this.name} is full, instantiating an new element.");
                    friendCardList.Add(Instantiate(friendCardList[0], friendCardList[0].transform.parent));
                }

                try
                {
                    _dummyProfileData = JsonConvert.DeserializeObject<ProfileData>(friend.Profile.AvatarUrl);
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.ToString());
                }
                finally
                {
                    _dummyProfileData = new ProfileData();
                }
                
                friendCardList[index].SetPlayerData(friend.TitleDisplayName, 1,  _dummyProfileData);

                index++;
            }
            
            for (var i = index; i < friendCardList.Count; i++)
            {
                friendCardList[i].gameObject.SetActive(false);
            }
        }
    }
}
