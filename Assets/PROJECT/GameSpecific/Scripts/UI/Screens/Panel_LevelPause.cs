using Project.GameSpecific.Scripts.Game.Controllers;
using Project.YajuluSDK.Scripts.Essentials;
using Project.YajuluSDK.Scripts.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Project.GameSpecific.Scripts.UI.Screens
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
