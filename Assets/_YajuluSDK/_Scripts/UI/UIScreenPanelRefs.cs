using System;
using System.Collections.Generic;
using PROJECT.Scripts.UI.Screens;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _YajuluSDK._Scripts.UI
{
    public class UIScreenPanelRefs : MonoBehaviour
    {
        [SerializeField] private List<UIScreenBase> _screenBaseList = new List<UIScreenBase>();
        [SerializeField] private List<UIPanelBase> _panelBaseList = new List<UIPanelBase>();
        
        [ReadOnly] public Dictionary<string, UIScreenBase> Screens = new Dictionary<string, UIScreenBase>();
        [ReadOnly] public Dictionary<string, UIPanelBase> Panels = new Dictionary<string, UIPanelBase>();

        public List<UIScreenBase> ScreenBaseList => _screenBaseList;
        public List<UIScreenBase> PanelBaseList => _screenBaseList;

        private UIScreenBase _screenBase;
        private UIPanelBase _panelBase;

        [SerializeField] private RectTransform screensParent;
        [SerializeField] private RectTransform panelsParent;
        
        [OnInspectorGUI]
        private void LoadScreens()
        {
            _screenBaseList.Clear();
            
            for (var i = 0; i < screensParent.childCount; i++)
            {
                _screenBase = null;
                if (screensParent.GetChild(i).TryGetComponent(out _screenBase))
                {
                    _screenBaseList.Add(_screenBase);   
                }
                else
                {
                    Debug.LogWarning($"Screen {screensParent.GetChild(i).name} does not contain UIScreenBase Component!, it will be ignored.");
                }
            }
        }
        
        [OnInspectorGUI]
        private void LoadPanels()
        {
            _panelBaseList.Clear();
            
            for (var i = 0; i < panelsParent.childCount; i++)
            {
                _panelBase = null;
                if (panelsParent.GetChild(i).TryGetComponent(out _panelBase))
                {
                    _panelBaseList.Add(_panelBase);   
                }
                else
                {
                    Debug.LogWarning($"Screen {panelsParent.GetChild(i).name} does not contain UIScreenBase Component!, it will be ignored.");
                }
            }
        }
        private void Awake()
        {
            PrepareScreenDictionary();
            PreparePanelDictionary();
        }
        
        private void PrepareScreenDictionary()
        {
            foreach (UIScreenBase screen in _screenBaseList)
            {
                string key = screen.GetType().Name;
                if (!Screens.ContainsKey(key))
                {
                    Screens.Add(key, screen);
                }
                else
                {
                    Debug.LogError($"Screen type: {key} is Duplicated in Screen :{screen.gameObject.name}!");
                }
            }
        }
        
        private void PreparePanelDictionary()
        {
            foreach (UIPanelBase panel in _panelBaseList)
            {
                string key = panel.GetType().Name;
                if (!Panels.ContainsKey(key))
                {
                    Panels.Add(key, panel);
                }
                else
                {
                    Debug.LogError($"Panel type: {key} is Duplicated in Panel :{panel.gameObject.name}!");
                }
            }
        }

        
    }
}
