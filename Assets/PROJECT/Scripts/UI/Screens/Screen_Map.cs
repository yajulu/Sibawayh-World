using _YajuluSDK._Scripts.UI;


namespace PROJECT.Scripts.UI.Screens
{
    public class Screen_Map : UIScreenBase
    {
        protected override void OnScreenOpenStarted()
        {
            base.OnScreenOpenStarted();
            UIScreenManager.Instance.Background.gameObject.SetActive(false);
        }
        
        
        protected override void OnScreenCloseEnded()
        {
            base.OnScreenCloseEnded();
            UIScreenManager.Instance.Background.gameObject.SetActive(true);
        }
    }
}
