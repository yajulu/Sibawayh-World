using System;
using System.Collections.Generic;
using System.Linq;
using _YajuluSDK._Scripts.Essentials;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _YajuluSDK._Scripts.UI
{

    /// <summary>
    /// A component that represents a group of UI.Toggles.
    /// </summary>
    /// <remarks>
    /// When using a group reference the group from a UI.Toggle. Only one member of a group can be active at a time.
    /// </remarks>
    [AddComponentMenu("UI/Tab Group", 31)]
    [DisallowMultipleComponent]
    
    public class UITabGroup : UIBehaviour
    {
        [SerializeField] private bool m_AllowSwitchOff = false;

        /// <summary>
        /// Is it allowed that no toggle is switched on?
        /// </summary>
        /// <remarks>
        /// If this setting is enabled, pressing the toggle that is currently switched on will switch it off, so that no toggle is switched on. If this setting is disabled, pressing the toggle that is currently switched on will not change its state.
        /// Note that even if allowSwitchOff is false, the Toggle Group will not enforce its constraint right away if no toggles in the group are switched on when the scene is loaded or when the group is instantiated. It will only prevent the user from switching a toggle off.
        /// </remarks>
        public bool allowSwitchOff
        {
            get { return m_AllowSwitchOff; }
            set { m_AllowSwitchOff = value; }
        }

        protected List<UITab> m_Tabs = new List<UITab>();

        protected UITabGroup()
        {
        }

        private Tween _contentTween;
        
        private bool _transition;
        
        
        [SerializeField] private Transform tabsParent;
        [SerializeField] private Transform tabsContentParent;

        private Dictionary<UITab, Transform> _tabGroupDictionary = new Dictionary<UITab, Transform>();

        /// <summary>
        /// Because all the Toggles have registered themselves in the OnEnabled, Start should check to
        /// make sure at least one Toggle is active in groups that do not AllowSwitchOff
        /// </summary>
        protected override void Start()
        {
            EnsureValidState();
            base.Start();
        }

        protected override void OnEnable()
        {
            EnsureValidState();
            base.OnEnable();
        }

        private void ValidateToggleIsInGroup(UITab tab)
        {
            if (tab == null || !m_Tabs.Contains(tab))
                throw new ArgumentException(string.Format("Toggle {0} is not part of ToggleGroup {1}", new object[] { tab, this }));
        }

        /// <summary>
        /// Notify the group that the given toggle is enabled.
        /// </summary>
        /// <param name="tab">The toggle that got triggered on.</param>
        /// <param name="sendCallback">If other toggles should send onValueChanged.</param>
        public void NotifyToggleOn(UITab tab, bool sendCallback = true)
        {
            ValidateToggleIsInGroup(tab);
            // disable all toggles in the group
            for (var i = 0; i < m_Tabs.Count; i++)
            {
                if (m_Tabs[i] == tab)
                    continue;

                if (sendCallback)
                    m_Tabs[i].isOn = false;
                else
                    m_Tabs[i].SetIsOnWithoutNotify(false);
            }
            
        }

        /// <summary>
        /// Unregister a toggle from the group.
        /// </summary>
        /// <param name="toggle">The toggle to remove.</param>
        public void UnregisterToggle(UITab tab)
        {
            if (m_Tabs.Contains(tab))
                m_Tabs.Remove(tab);
            
            if (_tabGroupDictionary.TryGetValue(tab, out var val))
            {
                _tabGroupDictionary.Remove(tab);
                if (val != null)
                {
                    val.name = "Unused";
                    val.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Register a toggle with the toggle group so it is watched for changes and notified if another toggle in the group changes.
        /// </summary>
        /// <param name="tab">The toggle to register with the group.</param>
        /// <param name="tabContentTransform">The transform of the content of this tab</param>
        public void RegisterToggle(UITab tab, Transform tabContentTransform = null)
        {
            if (!m_Tabs.Contains(tab))
                m_Tabs.Add(tab);
            
            var found = _tabGroupDictionary.TryGetValue(tab, out var content);
            
            if (content == null)
            {
                _tabGroupDictionary.Remove(tab);
                found = false;
            }
            
            if (tabContentTransform == null)
            {
                tabContentTransform = found  ? content : Instantiate(tabsContentParent.GetChild(0), tabsContentParent, false);
            }
            else
            {
                if (tabContentTransform == content)
                    return;
            }

            if (!found)
            {
                _tabGroupDictionary.Add(tab, tabContentTransform);    
            }
            
            tabContentTransform.name = tab.transform.gameObject.name + "_Content";
            
            RefreshTabs();
        }

        /// <summary>
        /// Ensure that the toggle group still has a valid state. This is only relevant when a ToggleGroup is Started
        /// or a Toggle has been deleted from the group.
        /// </summary>
        public void EnsureValidState()
        {
            if (!allowSwitchOff && !AnyTogglesOn() && m_Tabs.Count != 0)
            {
                m_Tabs[0].isOn = true;
                NotifyToggleOn(m_Tabs[0]);
            }

            IEnumerable<UITab> activeToggles = ActiveToggles();

            if (activeToggles.Count() > 1)
            {
                UITab firstActive = GetFirstActiveToggle();

                foreach (UITab toggle in activeToggles)
                {
                    if (toggle == firstActive)
                    {
                        continue;
                    }

                    toggle.isOn = false;
                }
            }
        }

        /// <summary>
        /// Are any of the toggles on?
        /// </summary>
        /// <returns>Are and of the toggles on?</returns>
        public bool AnyTogglesOn()
        {
            return m_Tabs.Find(x => x.isOn) != null;
        }

        /// <summary>
        /// Returns the toggles in this group that are active.
        /// </summary>
        /// <returns>The active toggles in the group.</returns>
        /// <remarks>
        /// Toggles belonging to this group but are not active either because their GameObject is inactive or because the Toggle component is disabled, are not returned as part of the list.
        /// </remarks>
        public IEnumerable<UITab> ActiveToggles()
        {
            return m_Tabs.Where(x => x.isOn);
        }

        /// <summary>
        /// Returns the toggle that is the first in the list of active toggles.
        /// </summary>
        /// <returns>The first active toggle from m_Toggles</returns>
        /// <remarks>
        /// Get the active toggle for this group. As the group
        /// </remarks>
        public UITab GetFirstActiveToggle()
        {
            IEnumerable<UITab> activeToggles = ActiveToggles();
            return activeToggles.Count() > 0 ? activeToggles.First() : null;
        }

        /// <summary>
        /// Switch all toggles off.
        /// </summary>
        /// <remarks>
        /// This method can be used to switch all toggles off, regardless of whether the allowSwitchOff property is enabled or not.
        /// </remarks>
        public void SetAllTogglesOff(bool sendCallback = true)
        {
            bool oldAllowSwitchOff = m_AllowSwitchOff;
            m_AllowSwitchOff = true;

            if (sendCallback)
            {
                for (var i = 0; i < m_Tabs.Count; i++)
                    m_Tabs[i].isOn = false;
            }
            else
            {
                for (var i = 0; i < m_Tabs.Count; i++)
                    m_Tabs[i].SetIsOnWithoutNotify(false);
            }

            m_AllowSwitchOff = oldAllowSwitchOff;
        }

        private void OnTabChanged(bool isOn, int index)
        {

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
            UITab tab = null;

            // if (tabsParent.childCount > tabsContentParent.childCount)
            // {
            //     // Adding new TabContents
            //     var delta = tabsParent.childCount - tabsContentParent.childCount;
            //     for (var i = 0; i < delta; i++)
            //     {
            //         Instantiate(tabsContentParent.GetChild(0), tabsContentParent, false);
            //     }
            // }
            // else 
            if (tabsParent.childCount < tabsContentParent.childCount)
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
                if (tabTransform.TryGetComponent(out tab))
                {
                    if (i < tabsContentParent.childCount)
                    {
                        var tabContent = tabsContentParent.GetChild(i).gameObject.transform;
                        tab.SetTabGroup(this, tabContent);
                    }
                    else
                    {
                        tab.group = this;
                    }
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
                if (tabTransform.TryGetComponent(out UITab tab))
                {
                    if (!_tabGroupDictionary.TryGetValue(tab, out Transform tabContent) || tabContent == null)
                    {
                        tab.group = this;
                        tabContent = _tabGroupDictionary[tab];
                    }

                    tabContent.gameObject.SetActive(true);
                    tabContent.SetSiblingIndex(tabTransform.GetSiblingIndex());
                }
            }
        }
    }

}
