using System;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.UI;
using DG.Tweening;
using PROJECT.Scripts.Enums;
using PROJECT.Scripts.Game.Controllers;
using PROJECT.Scripts.UI.Screens;
using RTLTMPro;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

namespace PROJECT.Scripts.UI
{
    public class Panel_LevelSelection : UIPanelBase
    {
        [SerializeField, TitleGroup("Refs")] private RTLTextMeshPro levelSelectionTitleText;
        [SerializeField, TitleGroup("Refs")] private RectTransform starsPanelTransform;
        [SerializeField, TitleGroup("Refs")] private Image[] stars;
        [SerializeField, TitleGroup("Refs")] private Button startButton;
        [SerializeField, TitleGroup("Refs")] private RectTransform statsPanelPlaceholder;
        
        [SerializeField, TitleGroup("Debug")] private int currentLevelNumber;
        [SerializeField, TitleGroup("Debug")] private eLevelState currentLevelState;
        
        private UIStatsPanelController _statsPanelRef;

        [SerializeField, ReadOnly, TitleGroup("Debug")]
        private int dummyIntLevelState;

        private bool _levelStared = false;

        private void Start()
        {
            _statsPanelRef = UIScreenManager.Instance.MiscRefs.StatsPanelController;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _levelStared = false;
            startButton.onClick.AddListener(LoadLevel);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            startButton.onClick.RemoveListener(LoadLevel);
        }

        protected override void OnScreenOpenStarted()
        {
            currentLevelNumber = GameModeManager.Instance.CurrentLevel;
            UpdatePanelUI();
            base.OnScreenOpenStarted();
        }
        
        protected override void OpenAnimation()
        {
            base.OpenAnimation();

            OpenSequence.OnStart(OnStarted);
            
            _statsPanelRef = UIScreenManager.Instance.MiscRefs.StatsPanelController;
            _statsPanelRef.GameModeProgressTransform.gameObject.SetActive(false);
            _statsPanelRef.transform.position = statsPanelPlaceholder.position;

            OpenSequence.Append(_statsPanelRef.transform.DOScale(1, 0.15f)
                .From(0)
                .SetEase(Ease.OutQuad));

            void OnStarted()
            {
                _statsPanelRef.UpdateTitlesToCurrentLevel();
                _statsPanelRef.gameObject.SetActive(true);
                gameObject.SetActive(true);
            }
        }

        protected override void CloseAnimation()
        {
            base.CloseAnimation();
            
            if (_levelStared)
            {
                // CloseSequence.Append(_statsPanelRef.transform.DOLocalMove(Vector3.zero, 0.2f)
                //     .SetEase(Ease.InSine));
            }
            else 
            {
                CloseSequence.Prepend(_statsPanelRef.transform.DOScale(0, 0.15f)
                    .From(1)
                    .SetEase(Ease.OutQuad));
                CloseSequence.AppendCallback(() =>
                {
                    _statsPanelRef.transform.localScale = Vector3.one;
                    _statsPanelRef.gameObject.SetActive(false);
                });    
            }
        }
        
        private void LoadLevel()
        {
            _levelStared = true;
            GameModeManager.Instance.LoadGameMode();
        }

        private void UpdatePanelUI(bool instant = true)
        {
            levelSelectionTitleText.text = GameModeManager.Instance.GetCurrentLevelTypeName();

            currentLevelState = DataPersistenceManager.Instance.Progress.GetLevelState(currentLevelNumber);
            
            dummyIntLevelState = (int)currentLevelState;
            if (instant)
            {
                for (int i = 0; i < stars.Length; i++)
                {
                    stars[i].canvasRenderer.SetAlpha(i < dummyIntLevelState - 1 ? 1 : 0.5f);
                }
            }
        }
        
        
        [Button, TitleGroup("Refs")]
        protected override void SetRefs()
        {
            levelSelectionTitleText = transform.FindDeepChild<RTLTextMeshPro>("LevelSelectionTitle_Text");
            starsPanelTransform = transform.FindDeepChild<RectTransform>("LevelSelectionStars_Panel");
            stars = starsPanelTransform.GetComponentsInChildren<Image>();
            startButton = transform.FindDeepChild<Button>("LevelSelection_Button");
            statsPanelPlaceholder = transform.FindDeepChild<RectTransform>("StatsPlaceHolder_Panel");
            
            //Base
            screenBackground = transform.FindDeepChild<Image>("LevelSelectionBackground_Image");
            mainPanel = transform.FindDeepChild<RectTransform>("LevelSelectionFrame_Panel");
            backgroundButton = transform.FindDeepChild<Button>("LevelSelectionBackground_Image");
        }
    }
}
