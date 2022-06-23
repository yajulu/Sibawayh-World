using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.YajuluSDK.Scripts.UI
{
    [Serializable]
    public class UIScreenNavigatorController : UIElementBase
    {
        [SerializeField] private UINavigationButton[] navigationButtons;

        [SerializeField] private List<NavigationControllerData> navigationData = new List<NavigationControllerData>();
        
        [OnInspectorGUI]
        private void UpdateUiNavButtons()
        {
            navigationButtons = GetComponentsInChildren<UINavigationButton>();
            navigationData.Clear();
            foreach (var button in navigationButtons)
            {
                navigationData.Add( new NavigationControllerData
                {
                    button = button,
                    data = button.NavigationData
                });
            }
        }
        
    }
    
    [Serializable]
    public struct NavigationControllerData
    {
        public UINavigationButton button;

        public UINavigationData data;
    }
}
