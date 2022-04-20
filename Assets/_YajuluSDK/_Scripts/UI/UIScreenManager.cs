using System;
using System.Collections.Generic;
using _YajuluSDK._Scripts.Essentials;
using PROJECT.Scripts.UI;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace _YajuluSDK._Scripts.UI
{
    public class UIScreenManager : Singleton<UIScreenManager>
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            
            _openedScreens = new List<UIScreenBase>();
            // _startedCloseScreens = new List<UIScreenBase>();
            // _startedOpenScreens = new List<UIScreenBase>();
        }

        private void Start()
        {
            InitializeScreens();
            OpenScreen(nameof(TestScreen));
        }

        #region Inspector

        [OnInspectorGUI]
        private void UpdateScreenRefs()
        {
            if (!ScreenRefs.SafeIsUnityNull())
                return;
            ScreenRefs = transform.GetComponentInChildren<UIScreenRefs>();
        }

        #endregion
        
        #region Fields

        public UIScreenRefs ScreenRefs;

        private List<UIScreenBase> _openedScreens;
        // private List<UIScreenBase> _startedOpenScreens;
        // private List<UIScreenBase> _startedCloseScreens;

        #endregion
        
        #region Events

        public static event Action<UIScreenBase> OnScreenPreOpen;
        public static event Action<UIScreenBase> OnScreenOpenStarted;
        public static event Action<UIScreenBase> OnScreenOpenEnded;
        
        public static event Action<UIScreenBase> OnScreenPreClose;
        public static event Action<UIScreenBase> OnScreenCloseStarted;
        public static event Action<UIScreenBase> OnScreenCloseEnded;

        #endregion

        #region Event Invokers

        public void ScreenPreOpened(UIScreenBase screen)
        {
            OnScreenPreOpen?.Invoke(screen);
        }

        public void ScreenOpenStarted(UIScreenBase screen)
        {
            // _startedOpenScreens.Add(screen);
            OnScreenOpenStarted?.Invoke(screen);
        }

        public void ScreenOpenEnded(UIScreenBase screen)
        {
            _openedScreens.Add(screen);
            OnScreenOpenEnded?.Invoke(screen);
        }
        
        public void ScreenPreClosed(UIScreenBase screen)
        {
            OnScreenPreClose?.Invoke(screen);
        }

        public void ScreenCloseStarted(UIScreenBase screen)
        {
            // _startedCloseScreens.Add(screen);
            OnScreenCloseStarted?.Invoke(screen);
        }

        public void ScreenCloseEnded(UIScreenBase screen)
        {
            _openedScreens.Remove(screen);
            OnScreenCloseEnded?.Invoke(screen);
        }

        #endregion

        private void InitializeScreens()
        {
            foreach (var screen in ScreenRefs.Screens.Values)
            {
                screen.gameObject.SetActive(false);
            }   
        }

        public void OpenScreen(string i_screen)
        {
            var screen = ScreenRefs.Screens[i_screen];
            if (screen.State != eUIScreenState.Closed)
            {
                Debug.LogError($"Screen ({i_screen}) is already Opened or is being opened. Current Screen state ({screen.State})");
                return;
            }
            
            ScreenRefs.Screens[i_screen].Open();
        }
        
        public void CloseScreen(string i_screen)
        {
            var screen = ScreenRefs.Screens[i_screen];
            if (screen.State != eUIScreenState.Opened)
            {
                Debug.LogError($"Screen ({i_screen}) is already closed or is being closed. Current Screen state ({screen.State})");
                return;
            }

            ScreenRefs.Screens[i_screen].Close();
        }
        
        public void NavigateTo(UIScreenBase i_screen, bool i_closeCurrent)
        {
            if (_openedScreens.Count > 0 && i_closeCurrent)
            {
                var latestScreen = _openedScreens[^1];
                latestScreen.Close(
                    () => i_screen.Open()
                );    
            }
            else
            {
                i_screen.Open();
            }
        }
        
        public void NavigateTo(string i_screen, bool i_closeCurrent)
        {
            var screen = ScreenRefs.Screens[i_screen];
            NavigateTo(screen, i_closeCurrent);
        }
        
    }
}
