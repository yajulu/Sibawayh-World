using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace _YajuluSDK._Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class UINavigationButton : UIElementBase
    {
        [SerializeField, ValueDropdown(nameof(ScreensList))] private UIScreenBase navigateToScreen;
        [SerializeField] private bool closeCurrent;
        private List<UIScreenBase> ScreensList => FindObjectOfType<UIScreenManager>().ScreenRefs.ScreenBaseList;

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
            UIScreenManager.Instance.NavigateTo(navigateToScreen, closeCurrent);
        }
        
    }
}
