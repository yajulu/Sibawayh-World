using System;
using _YajuluSDK._Scripts.Essentials;
using DG.Tweening;
using PROJECT.Scripts.Enums;
using PROJECT.Scripts.Game.Controllers;
using RTLTMPro;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

namespace PROJECT.Scripts.UI
{
    public class UILevelSelectionPanelController : UIBehaviour
    {
        [SerializeField, TitleGroup("Refs")] private RTLTextMeshPro levelSelectionTitleText;
        [SerializeField, TitleGroup("Refs")] private RectTransform starsPanelTransform;
        [SerializeField, TitleGroup("Refs")] private RectTransform framePanelTransform;
        [SerializeField, TitleGroup("Refs")] private Image backgroundImage;
        [SerializeField, TitleGroup("Refs")] private Image[] stars;
        [SerializeField, TitleGroup("Refs")] private Button startButton;
        [SerializeField, TitleGroup("Refs")] private Button closeButton;

        [SerializeField, TitleGroup("Debug")] private int currentLevelNumber;
        [SerializeField, TitleGroup("Debug")] private eLevelState currentLevelState;

        [SerializeField, ReadOnly, TitleGroup("Debug")]
        private int dummyIntLevelState;

        private Sequence _openSequence;
        private Sequence _closeSequence;

        private Action closeAction;
        private Action openAction;

        protected override void OnEnable()
        {
            base.OnEnable();
            closeButton.onClick.AddListener(ClosePanel);
            startButton.onClick.AddListener(StartLevel);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            closeButton.onClick.RemoveListener(ClosePanel);
            startButton.onClick.RemoveListener(StartLevel);
        }

        public void OpenPanel(int levelNumber)
        {
            currentLevelNumber = levelNumber;
            UpdatePanelUI();
            OpenAnimation();
        }

        private void OpenAnimation()
        {
            _openSequence = DOTween.Sequence();

            _openSequence.OnStart(onStart);

            _openSequence.Append(backgroundImage.DOFade(0.5f, 0.3f)
                .SetEase(Ease.OutQuad)
                .From(0));
            _openSequence.Insert(0.15f, framePanelTransform.transform.DOScale(1, 0.25f).From(0).SetEase(Ease.OutBack));

            _openSequence.Append(startButton.transform.DOScale(1, 0.3f)
                .SetEase(Ease.OutBack)
                .From(0));

            _openSequence.OnComplete(onComplete);
            
            void onStart()
            {
                gameObject.SetActive(true);
                closeButton.interactable = false;
            }

            void onComplete()
            {
                closeButton.interactable = true;
            }
        }

        public void ClosePanel()
        {
            _closeSequence = DOTween.Sequence();

            
            _closeSequence.Append(backgroundImage.DOFade(0f, 0.3f)
                .SetEase(Ease.OutQuad)
                .From(0.5f));

            _closeSequence.Insert(0.15f, framePanelTransform.transform.DOScale(0, 0.25f).From(1).SetEase(Ease.OutBack));

            _closeSequence.OnComplete(onComplete);

            void onComplete()
            {
                gameObject.SetActive(false);
                closeAction?.Invoke();
            }
        }

        private void StartLevel()
        {
            // closeAction =  
                
        }

        private void UpdatePanelUI(bool instant = true)
        {
            levelSelectionTitleText.text = GameModeManager.Instance.GetCurrentLevelTypeName();

            GameModeManager.Instance.GetLevelState(currentLevelNumber);
            
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
        private void SetRefs()
        {
            levelSelectionTitleText = transform.FindDeepChild<RTLTextMeshPro>("LevelSelectionTitle_Text");
            starsPanelTransform = transform.FindDeepChild<RectTransform>("Stars_Panel");
            stars = starsPanelTransform.GetComponentsInChildren<Image>();

            backgroundImage = transform.FindDeepChild<Image>("LevelSelectionBackground_Image");
            framePanelTransform = transform.FindDeepChild<RectTransform>("LevelSelectionFrame_Panel");
            startButton = transform.FindDeepChild<Button>("LevelSelection_Button");
            closeButton = transform.FindDeepChild<Button>("LevelSelectionBackground_Image");
        }
    }
}
