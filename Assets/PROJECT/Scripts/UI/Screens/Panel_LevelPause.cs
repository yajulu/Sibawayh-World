using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.UI;
using PROJECT.Scripts.Game.Controllers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace PROJECT.Scripts.UI.Screens
{
    public class Panel_LevelPause : UIPanelBase
    {
        [SerializeField, TitleGroup("Refs")] private Button resumeButton;
        [SerializeField, TitleGroup("Refs")] private Button restartButton;
        [SerializeField, TitleGroup("Refs")] private Button homeButton;

        protected override void OnEnable()
        {
            base.OnEnable();
            resumeButton.onClick.AddListener(ClosePanel);
            homeButton.onClick.AddListener(Home);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            resumeButton.onClick.RemoveListener(ClosePanel);
            homeButton.onClick.RemoveListener(Home);
        }
        
        private void Home()
        {
            GameModeManager.Instance.StopGameMode();
        }

        protected override void SetRefs()
        {
            base.SetRefs();
            restartButton = transform.FindDeepChild<Button>("LevelPausePanelRestart_Button");
            resumeButton = transform.FindDeepChild<Button>("LevelPausePanelResume_Button");
            homeButton = transform.FindDeepChild<Button>("LevelPausePanelHome_Button");
        }
    }
}
