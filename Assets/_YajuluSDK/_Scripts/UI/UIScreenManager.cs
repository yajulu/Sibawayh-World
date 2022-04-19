using System;
using _YajuluSDK._Scripts.MISC;
using UnityEngine;

namespace _YajuluSDK._Scripts.UI
{
    public class UIScreenManager : Singleton<UIScreenManager>
    {

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
            OnScreenOpenStarted?.Invoke(screen);
        }

        public void ScreenOpenEnded(UIScreenBase screen)
        {
            OnScreenOpenEnded?.Invoke(screen);
        }
        
        public void ScreenPreClosed(UIScreenBase screen)
        {
            OnScreenPreClose?.Invoke(screen);
        }

        public void ScreenCloseStarted(UIScreenBase screen)
        {
            OnScreenCloseStarted?.Invoke(screen);
        }

        public void ScreenCloseEnded(UIScreenBase screen)
        {
            OnScreenCloseEnded?.Invoke(screen);
        }

        #endregion
        
        
    }
}
