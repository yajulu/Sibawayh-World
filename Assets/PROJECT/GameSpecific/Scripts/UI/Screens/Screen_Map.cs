using Project.GameSpecific.Scripts.Game.Controllers;
using Project.YajuluSDK.Scripts.UI;

namespace Project.GameSpecific.Scripts.UI.Screens
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
