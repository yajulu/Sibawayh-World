using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.UI;
using DG.Tweening;
using PROJECT.Scripts.Game;
using PROJECT.Scripts.Game.Controllers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PROJECT.Scripts.UI.Screens
{
    public class Screen_GameMode : UIScreenBase
    {
        [SerializeField, TitleGroup("Refs")] private LetterSpawner letterSpawner;

        [SerializeField, TitleGroup("Refs")] private RectTransform statsPanelPlaceHolder;
        // [SerializeField]

        private UIStatsPanelController _statsPanelRef;
        protected override void OpenAnimation()
        {
            base.OpenAnimation();
            _statsPanelRef = UIScreenManager.Instance.MiscRefs.StatsPanelController;
            
            OpenSequence.AppendCallback(OnStarted);
            
            OpenSequence.Append(_statsPanelRef.transform.DOMove(statsPanelPlaceHolder.transform.position, 0.7f)
                .SetEase(Ease.OutQuad));
            OpenSequence.AppendCallback(() => _statsPanelRef.GameModeProgressTransform.gameObject.SetActive(true));
            OpenSequence.Append(_statsPanelRef.GameModeProgressTransform
                .DOScale(1, 0.15f)
                .SetEase(Ease.Linear)
                .From(0));
        
            void OnStarted()
            {
                _statsPanelRef.gameObject.SetActive(true);
            }
        }
        
        protected override void CloseAnimation()
        {
            base.CloseAnimation();
            
            _statsPanelRef = UIScreenManager.Instance.MiscRefs.StatsPanelController;
            
            CloseSequence.Prepend(_statsPanelRef.GameModeProgressTransform
                .DOScale(0, 0.15f)
                .SetEase(Ease.Linear)
                .From(1));
            // CloseSequence.Prepend(_statsPanelRef.transform.DOMove(transform.position, 0.5f)
            //     .SetEase(Ease.OutQuad));
            // CloseSequence.Prepend(UIScreenManager.Instance.BlackScreenMid.DOFade(0.3f, 0.2f)
            //     .From(1));
            // CloseSequence.PrependCallback(() => UIScreenManager.Instance.BlackScreenMid.gameObject.SetActive(true));
            
            CloseSequence.AppendCallback(() => UIScreenManager.Instance.BlackScreenMid.gameObject.SetActive(false));            
        }
        protected override void OnScreenOpenEnded()
        {
            base.OnScreenOpenEnded();
            
            //TODO: Use UImanager events Events
            GameModeManager.Instance.StartGameMode();
        }

        protected override void OnScreenCloseEnded()
        {
            _statsPanelRef.gameObject.SetActive(false);
            base.OnScreenCloseEnded();
        }

        [Button]
        private void SetRefs()
        {
            letterSpawner = GetComponentInChildren<LetterSpawner>();
            statsPanelPlaceHolder = transform.FindDeepChild<RectTransform>("StatsPlaceHolder_Panel");
        }
    }
}
