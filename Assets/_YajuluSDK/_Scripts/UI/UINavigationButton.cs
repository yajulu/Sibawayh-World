using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace _YajuluSDK._Scripts.UI
{
    [RequireComponent(typeof(Button)), Serializable]
    public class UINavigationButton : UIElementBase
    {
        public UINavigationData NavigationData => navigationData;
        [SerializeField] private UINavigationData navigationData = new UINavigationData();

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(Navigate);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Navigate);
        }

        private void Navigate()
        {
            UIScreenManager.Instance.NavigateTo(navigationData.navigateToScreen, navigationData.closeCurrent);
        }
        
    }
    
    [Serializable]
    public class UINavigationData 
    {
        [SerializeField, ValueDropdown(nameof(ScreensList))] public UIScreenBase navigateToScreen;
        [SerializeField] public bool closeCurrent;
        private List<UIScreenBase> ScreensList => Object.FindObjectOfType<UIScreenManager>().screenPanelRefs.ScreenBaseList;
    }
}
