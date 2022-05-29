using System;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace PROJECT.Scripts.UI.Screens
{
    public class Panel_Shop : UIScreenBase
    {
        [SerializeField] private Image screenBackground;
        [SerializeField] private Image mainPanel;
        [SerializeField] private Button backgroundButton;

        private Sequence _openSequence;

        private void OnEnable()
        {
            backgroundButton.onClick.AddListener(CloseShop);
        }

        private void OnDisable()
        {
            backgroundButton.onClick.RemoveListener(CloseShop);
        }

        protected override void OpenAnimation()
        {
            _openSequence = DOTween.Sequence();

            _openSequence.OnStart(OnStart);
            _openSequence.Append(screenBackground.DOFade(0.5f, 0.3f)
                .SetEase(Ease.OutQuad)
                .From(0));

            _openSequence.Insert(0.15f, mainPanel.transform.DOScale(1, 0.25f).From(0).SetEase(Ease.OutBack));

            _openSequence.OnComplete(OnScreenOpenEnded);

            void OnStart()
            {
                gameObject.SetActive(true);
            }
        }

        private void CloseShop()
        {
            UIScreenManager.Instance.CloseScreen(this);
        }

        [Button]
        private void SetRefs()
        {
            screenBackground = transform.FindDeepChild<Image>("Background");
            mainPanel = transform.FindDeepChild<Image>("ShopMain_Panel");
            backgroundButton = screenBackground.GetComponent<Button>();
        }
    }
}
