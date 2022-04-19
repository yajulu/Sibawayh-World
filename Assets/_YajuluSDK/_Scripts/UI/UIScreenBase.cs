namespace _YajuluSDK._Scripts.UI
{
    public class UIScreenBase : UIElementBase
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public virtual void Open()
        {
            if (OnScreenPreOpen())
            {
                OnScreenOpenStarted();
            }
        }

        protected virtual bool OnScreenPreOpen()
        {
            return true;
        }
        
        protected virtual void OnScreenOpenStarted()
        {
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
            UIScreenManager.Instance.ScreenCloseEnded(this);
        }
        
        public virtual void Close()
        {
            if (OnScreenPreClose())
            {
                OnScreenCloseStarted();
            }
        }

        protected virtual bool OnScreenPreClose()
        {
            UIScreenManager.Instance.ScreenPreOpened(this);
            return true;
        }

        protected virtual void OnScreenCloseStarted()
        {
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
            UIScreenManager.Instance.ScreenCloseEnded(this);
        }
        
    }
}
