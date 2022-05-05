using System;
using System.Collections.Generic;
using System.Linq;
using _YajuluSDK._Scripts.Essentials;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace _YajuluSDK._Scripts.UI
{
    public class UITabGroup : ToggleGroup
    {
        [SerializeField] private Transform tabsParent;
        [SerializeField] private Transform tabsContentParent;

        private Dictionary<Toggle, Transform> _tabGroupDictionary = new Dictionary<Toggle, Transform>();
        protected override void Start()
        {
            base.Start();
            // PrepareTabs();
        }



        private void OnTabChanged(bool isOn, int index)
        {
            
        }

        public new void RegisterToggle(Toggle toggle)
        {
            base.RegisterToggle(toggle);
            if (_tabGroupDictionary.ContainsKey(toggle)) return;
            var contentIndex = toggle.transform.GetSiblingIndex();

            var tabContent = Instantiate(tabsContentParent.GetChild(0), tabsContentParent, false);
            tabContent.SetSiblingIndex(contentIndex);
            
        }

        public new void UnregisterToggle(Toggle toggle)
        {
            base.UnregisterToggle(toggle);
            if (_tabGroupDictionary.TryGetValue(toggle, out var val))
            {
                DestroyImmediate(val);
            }
        }
        
        protected override void Reset()
        {
            base.Reset();
            SetRefs();
            ResetTabs();
        }
        
        [Button]
        private void SetRefs()
        {
            tabsParent = transform.FindDeepChild<Transform>("Tabs");
            tabsContentParent = transform.FindDeepChild<Transform>("TabsContent");
        }
        
        [Button]
        private void ResetTabs()
        {
            Toggle toggle = null;

            if (tabsParent.childCount > tabsContentParent.childCount)
            {
                // Adding new TabContents
                var delta = tabsParent.childCount - tabsContentParent.childCount;
                for (var i = 0; i < delta; i++)
                {
                    Instantiate(tabsContentParent.GetChild(0), tabsContentParent, false);
                }
            }
            else if (tabsParent.childCount < tabsContentParent.childCount)
            {
                // Disabling not used tabsContent panels
                for (var i = tabsParent.childCount; i < tabsContentParent.childCount; i++)
                {
                    var content = tabsContentParent.GetChild(i);
                    content.SetSiblingIndex(i);
                    content.name = "Unused";
                }
            }
            
            _tabGroupDictionary.Clear();
            
            for (var i = 0; i < tabsParent.childCount; i++)
            {
                var tabTransform = tabsParent.GetChild(i);
                if (tabTransform.TryGetComponent(out toggle))
                {
                    var tabContent = tabsContentParent.GetChild(i).gameObject;
                    _tabGroupDictionary.TryAdd(toggle, tabContent.transform);
                    
                    toggle.onValueChanged.RemoveAllListeners();
                    var toggle1 = toggle;
                    toggle.onValueChanged.AddListener((isOn) =>
                    {
                        _tabGroupDictionary[toggle1].gameObject.SetActive(isOn);
                    });
                    toggle.group = this;
                }
                else
                {
                    Debug.LogError($"{tabsParent.GetChild(i).name} does not have a Toggle component");
                }
            }
            
            RefreshTabs();
        }

        [Button]
        private void RefreshTabs()
        {
            //Update TabsContent as the tabs
            for (var i = 0; i < tabsParent.childCount; i++)
            {
                tabsParent.GetChild(i);
                var tabTransform = tabsParent.GetChild(i);
                if (tabTransform.TryGetComponent(out Toggle toggle))
                {
                    if (!_tabGroupDictionary.TryGetValue(toggle, out Transform tabContent) || tabContent == null)
                    {
                        tabContent = Instantiate(tabsContentParent.GetChild(0), tabsContentParent, false);
                        if (!_tabGroupDictionary.TryAdd(toggle, tabContent.transform))
                        {
                            _tabGroupDictionary.Remove(toggle);
                            _tabGroupDictionary.Add(toggle, tabContent.transform);
                        }
                    
                        toggle.onValueChanged.RemoveAllListeners();
                        var toggle1 = toggle;
                        toggle.onValueChanged.AddListener((isOn) =>
                        {
                            _tabGroupDictionary[toggle1].gameObject.SetActive(isOn);
                        });
                        toggle.group = this;
                    }
                    tabContent.gameObject.SetActive(toggle.isOn);
                    tabContent.name = tabTransform.gameObject.name + "_Content";
                    tabContent.SetSiblingIndex(tabTransform.GetSiblingIndex());   
                }
            }
        }
        
    }
}
