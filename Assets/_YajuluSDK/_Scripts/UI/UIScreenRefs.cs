using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _YajuluSDK._Scripts.UI
{
    public class UIScreenRefs : MonoBehaviour
    {
        [SerializeField] private List<UIScreenBase> _screenBaseList = new List<UIScreenBase>();
        
        [ReadOnly] public Dictionary<string, UIScreenBase> Screens = new Dictionary<string, UIScreenBase>();

        public List<UIScreenBase> ScreenBaseList => _screenBaseList;

        private UIScreenBase _screenBase;
        
        [OnInspectorGUI]
        private void LoadScreens()
        {
            _screenBaseList.Clear();
            
            for (var i = 0; i < transform.childCount; i++)
            {
                _screenBase = null;
                if (transform.GetChild(i).TryGetComponent(out _screenBase))
                {
                    _screenBaseList.Add(_screenBase);   
                }
                else
                {
                    Debug.LogWarning($"Screen {transform.GetChild(i).name} does not contain UIScreenBase Component!, it will be ignored.");
                }
            }
        }
        private void Awake()
        {
            PrepareScreenDictionary();
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

        
    }
}
