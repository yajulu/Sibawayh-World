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
            OpenSequence = DOTween.Sequence();
            
            _statsPanelRef = UIScreenManager.Instance.MiscRefs.StatsPanelController;
        
            OpenSequence.OnStart(onStarted);
            OpenSequence.OnComplete(OnScreenOpenEnded);
            
            OpenSequence.Append(_statsPanelRef.transform.DOMove(statsPanelPlaceHolder.transform.position, 0.5f)
                .SetEase(Ease.OutQuad));
            OpenSequence.AppendCallback(() => _statsPanelRef.GameModeProgressTransform.gameObject.SetActive(true));
            OpenSequence.Append(_statsPanelRef.GameModeProgressTransform
                .DOScale(1, 0.15f)
                .SetEase(Ease.Linear)
                .From(0));
        
            void onStarted()
            {
                _statsPanelRef.gameObject.SetActive(true);
                gameObject.SetActive(true);
            }
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
