using System;
using System.Collections.Generic;
using _YajuluSDK._Scripts.Essentials;
using PROJECT.Scripts.UI;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace _YajuluSDK._Scripts.UI
{
    public class UIScreenManager : Singleton<UIScreenManager>
    {
        
        #region Inspector

        [OnInspectorGUI]
        private void UpdateScreenRefs()
        {
            if (ScreenRefs.SafeIsUnityNull())
            {
                ScreenRefs = transform.GetComponentInChildren<UIScreenRefs>();    
            }

            if (_skipAnimationButton.SafeIsUnityNull())
            {
                _skipAnimationButton = transform.FindDeepChild<Button>("SkipAnimation_Button");
            }
                
        }

        #endregion
        
        #region Fields

        public UIScreenRefs ScreenRefs;

        private List<UIScreenBase> _openedScreens;

        [SerializeField] private Button _skipAnimationButton;
        private Queue<UIScreenQueueData> _screenQueue;
        private UIScreenBase _currentChangingScreen;
        private UIScreenQueueData _dummyQueueData;

        private bool _screenNavigationLock;
        
        #endregion
        
        protected override void OnAwake()
        {
            base.OnAwake();
            
            _openedScreens = new List<UIScreenBase>();
            _skipAnimationButton.onClick.AddListener(SkipCurrenScreenAnimation);
            _screenQueue = new Queue<UIScreenQueueData>();
        }

        private void Start()
        {
            InitializeScreens();
            OpenScreen(nameof(TestScreen));
        }
        
        #region Events

        public static event Action<UIScreenBase, bool> OnScreenPreOpen;
        public static event Action<UIScreenBase> OnScreenOpenStarted;
        public static event Action<UIScreenBase> OnScreenOpenEnded;
        
        public static event Action<UIScreenBase, bool> OnScreenPreClose;
        public static event Action<UIScreenBase> OnScreenCloseStarted;
        public static event Action<UIScreenBase> OnScreenCloseEnded;

        #endregion

        #region Event Invokers

        public void ScreenPreOpened(UIScreenBase screen, bool isValid)
        {
            if (!isValid)
            {
                _currentChangingScreen = null;
                _screenNavigationLock = false;
            }
            OnScreenPreOpen?.Invoke(screen, isValid);
            if(!isValid)
                CheckQueue();
        }

        public void ScreenOpenStarted(UIScreenBase screen)
        {
            _skipAnimationButton.gameObject.SetActive(true);
            OnScreenOpenStarted?.Invoke(screen);
        }

        public void ScreenOpenEnded(UIScreenBase screen)
        {
            _openedScreens.Add(screen);
            _currentChangingScreen = null;
            _screenNavigationLock = false;
            _skipAnimationButton.gameObject.SetActive(false);
            OnScreenOpenEnded?.Invoke(screen);
            CheckQueue();
        }
        
        public void ScreenPreClosed(UIScreenBase screen, bool isValid)
        {
            if (!isValid)
            {
                _currentChangingScreen = null;
                _screenNavigationLock = false;
            }
            OnScreenPreClose?.Invoke(screen, isValid);
            if(!isValid)
                CheckQueue();
        }

        public void ScreenCloseStarted(UIScreenBase screen)
        {
            // _startedCloseScreens.Add(screen);
            _skipAnimationButton.gameObject.SetActive(true);
            OnScreenCloseStarted?.Invoke(screen);
        }

        public void ScreenCloseEnded(UIScreenBase screen)
        {
            _openedScreens.Remove(screen);
            _currentChangingScreen = null;
            _screenNavigationLock = false;
            _skipAnimationButton.gameObject.SetActive(false);
            OnScreenCloseEnded?.Invoke(screen);
            CheckQueue();
        }

        #endregion

        private void CheckQueue()
        {
            if (_screenQueue.TryDequeue(out _dummyQueueData))
            {
                if(_dummyQueueData.IsOpening)
                    OpenScreen(_dummyQueueData.ScreenBase.GetType().Name);
                else
                    CloseScreen(_dummyQueueData.ScreenBase.GetType().Name);
            }
        }

        private void SkipCurrenScreenAnimation()
        {
            //TODO: Stop Spamming 
            if (!_currentChangingScreen.SafeIsUnityNull() && _screenNavigationLock)
            {
                _currentChangingScreen.SkipAnimation();
            }
        }
        private void InitializeScreens()
        {
            foreach (var screen in ScreenRefs.Screens.Values)
            {
                screen.gameObject.SetActive(false);
            }   
            _skipAnimationButton.gameObject.SetActive(false);
        }
        
        public void OpenScreen(string i_screen, Action onSucceeded = null, Action onFailed = null)
        {
            var screen = ScreenRefs.Screens[i_screen];
            
            OpenScreen(screen);
        }

        public void OpenScreen(UIScreenBase screen, Action onSucceeded = null, Action onFailed = null)
        {
            
            //Check if there is a screenNavigationLock or there is a screen currently opening or closing
            if (_screenNavigationLock && !_currentChangingScreen.SafeIsUnityNull())
            {
                _screenQueue.Enqueue(new UIScreenQueueData
                {
                    ScreenBase = screen,
                    IsOpening = true
                });
                Debug.LogError($"Screen ({screen}) is will be Queued to Open, Current State: ({screen.State})");
                return;
            }
            
            if (screen.State != eUIScreenState.Closed)
            {
                Debug.LogError($"Screen ({screen}) is already Opened or is being opened. Current Screen state ({screen.State})");
                return;
            }

            _currentChangingScreen = screen;
            _screenNavigationLock = true;
            screen.Open(onSucceeded, onFailed);
        }
        
        public void CloseScreen(string i_screen, Action onSucceeded = null, Action onFailed = null)
        {
            var screen = ScreenRefs.Screens[i_screen];
            
            CloseScreen(screen);
        }
        
        public void CloseScreen(UIScreenBase screen, Action onSucceeded = null, Action onFailed = null)
        {
            //Check if there is a screenNavigationLock or there is a screen currently opening or closing
            if (_screenNavigationLock && !_currentChangingScreen.SafeIsUnityNull())
            {
                _screenQueue.Enqueue(new UIScreenQueueData
                {
                    ScreenBase = screen,
                    IsOpening = false
                });
                Debug.LogError($"Screen ({screen}) is will be Queued to Close, Current State: ({screen.State})");
                return;
            }
            
            if (screen.State != eUIScreenState.Opened)
            {
                Debug.LogError($"Screen ({screen}) is already closed or is being closed. Current Screen state ({screen.State})");
                return;
            }

            _currentChangingScreen = screen;
            _screenNavigationLock = true;
            screen.Close(onSucceeded, onFailed);
        }
        
        public void NavigateTo(UIScreenBase i_screen, bool i_closeCurrent)
        {
            // //Check if there is a screenNavigationLock or there is a screen currently opening or closing
            // if (_screenNavigationLock && !_currentChangingScreen.SafeIsUnityNull())
            // {
            //     _screenQueue.Enqueue(new UIScreenQueueData
            //     {
            //         ScreenBase = i_screen,
            //         IsOpening = true
            //     });
            //     Debug.LogError($"Screen ({i_screen}) is will be Queued to Close, Current State: ({i_screen.State})");
            //
            //     if (i_closeCurrent)
            //     {
            //         var latestScreen = _openedScreens[^1];
            //         _screenQueue.Enqueue(new UIScreenQueueData
            //         {
            //             ScreenBase = latestScreen,
            //             IsOpening = false
            //         });
            //     }
            //     return;
            // }
            
            if (_openedScreens.Count > 0 && i_closeCurrent)
            {
                var latestScreen = _openedScreens[^1];
                CloseScreen(latestScreen, OnSucceeded, OnFailed);
            }
            else
            {
                OpenScreen(i_screen);
            }
            

            #region local Methods

            void OnSucceeded()
            {
                OpenScreen(i_screen);
            }

            void OnFailed()
            {
                Debug.LogError($"Screen {i_screen} failed to open.");
            }

            #endregion
        }
        
        public void NavigateTo(string i_screen, bool i_closeCurrent)
        {
            var screen = ScreenRefs.Screens[i_screen];
            NavigateTo(screen, i_closeCurrent);
        }
        
    }

    [Serializable]
    public struct UIScreenQueueData
    {
        public UIScreenBase ScreenBase;
        public bool IsOpening;
    }
}
