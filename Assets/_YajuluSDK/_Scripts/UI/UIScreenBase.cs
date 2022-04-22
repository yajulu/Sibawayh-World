using System;
using UnityEngine;

namespace _YajuluSDK._Scripts.UI
{
    [RequireComponent(typeof(UIScreenNavigatorController))]
    public class UIScreenBase : UIElementBase
    {
        public eUIScreenState State { get; private set; } = eUIScreenState.Closed;

        private eUIScreenState _previousState;

        private Action _openSucceedsAction;
        private Action _openFailedAction;
        
        private Action _closeSuccessAction;
        private Action _closeFailedAction;

        private UIScreenNavigatorController navController;

        protected virtual void Awake()
        {
            navController = GetComponent<UIScreenNavigatorController>();
        }

        public void Open(Action onSucceeded = null, Action onFailed = null)
        {
            if (State != eUIScreenState.Closed)
            {
                Debug.LogWarning($"Screen {this.GetType().Name} cannot be opened!, Current State {State}");
                return;
            }

            _openSucceedsAction = onSucceeded;
            _openFailedAction = onFailed;
            
            var prevState = State;
            State = eUIScreenState.PreOpen;
            UIScreenManager.Instance.ScreenPreOpened(this);
            if (OnScreenPreOpen())
            {
                OnScreenOpenStarted();
            }
            else
            {
                State = prevState;
            }
        }

        protected virtual bool OnScreenPreOpen()
        {
            return true;
        }
        
        protected virtual void OnScreenOpenStarted()
        {
            State = eUIScreenState.OpenStarted;
            UIScreenManager.Instance.ScreenOpenStarted(this);
            
            OpenAnimation();
        }
        
        protected virtual void OpenAnimation()
        {
            gameObject.SetActive(true);
            
            OnScreenOpenEnded();
        }

        protected virtual void OnScreenOpenEnded()
        {
            State = eUIScreenState.Opened;
            
            UIScreenManager.Instance.ScreenOpenEnded(this);
            _openSucceedsAction?.Invoke();
            _openSucceedsAction = null;
        }

        public void Close(Action onSucceeded = null, Action onFailed = null)
        {
            
            if (State != eUIScreenState.Opened)
            {
                Debug.LogWarning($"Screen {this.GetType().Name} cannot be closed!, Current State {State}");
                return;
            }
            
            _closeSuccessAction = onSucceeded;
            _closeFailedAction = onFailed;
            
            var prevState = State;
            State = eUIScreenState.PreClose;
            UIScreenManager.Instance.ScreenPreClosed(this);
            
            if (OnScreenPreClose())
            {
                OnScreenCloseStarted();
            }
            else
            {
                State = prevState;
            }
        }

        protected virtual bool OnScreenPreClose()
        {
            
            return true;
        }

        protected virtual void OnScreenCloseStarted()
        {
            State = eUIScreenState.CloseStarted;
            UIScreenManager.Instance.ScreenCloseStarted(this);
            
            CloseAnimation();
        }

        protected virtual void CloseAnimation()
        {
            gameObject.SetActive(false);
            
            OnScreenCloseEnded();
        }

        protected virtual void OnScreenCloseEnded()
        {
            State = eUIScreenState.Closed;
            UIScreenManager.Instance.ScreenCloseEnded(this);
            _closeSuccessAction?.Invoke();
            _closeSuccessAction = null;
        }
        
    }
}
