using _YajuluSDK._Scripts.UI;
using PROJECT.Scripts.Game.Controllers;


namespace PROJECT.Scripts.UI.Screens
{
    public class Screen_Map : UIScreenBase
    {
        protected override void OnScreenOpenStarted()
        {
            base.OnScreenOpenStarted();
            UIScreenManager.Instance.Background.gameObject.SetActive(false);
        }

        protected override void OnScreenOpenEnded()
        {
            base.OnScreenOpenEnded();
            GameModeManager.Instance.UnloadGameMode();
        }


        protected override void OnScreenCloseEnded()
        {
            base.OnScreenCloseEnded();
            UIScreenManager.Instance.Background.gameObject.SetActive(true);
        }
    }
}
