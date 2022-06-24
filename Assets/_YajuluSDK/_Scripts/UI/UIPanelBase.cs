using _YajuluSDK._Scripts.Essentials;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace _YajuluSDK._Scripts.UI
{
    public class UIPanelBase : UIScreenBase
    {
        [SerializeField, TitleGroup("Base Refs")] protected Image screenBackground;
        [SerializeField, TitleGroup("Base Refs")] protected RectTransform mainPanel;
        [SerializeField, TitleGroup("Base Refs")] protected Button backgroundButton;
        
        protected virtual void OnEnable()
        {
            backgroundButton.onClick.AddListener(ClosePanel);
        }

        protected virtual void OnDisable()
        {
            backgroundButton.onClick.RemoveListener(ClosePanel);
        }

        protected override void OpenAnimation()
        {
            OpenSequence = DOTween.Sequence();

            OpenSequence.OnStart(OnStart);
            OpenSequence.Append(screenBackground.DOFade(0.5f, 0.3f)
                .SetEase(Ease.OutQuad)
                .From(0));

            OpenSequence.Insert(0.15f, mainPanel.transform.DOScale(1, 0.25f).From(0).SetEase(Ease.OutBack));

            OpenSequence.OnComplete(OnScreenOpenEnded);

            void OnStart()
            {
                gameObject.SetActive(true);
            }
        }


        protected override void CloseAnimation()
        {
            CloseSequence = DOTween.Sequence();

            CloseSequence.Append(screenBackground.DOFade(0f, 0.3f)
                .SetEase(Ease.InQuad)
                .From(0.5f));

            CloseSequence.Insert(0.15f, mainPanel.transform.DOScale(0, 0.25f).From(1).SetEase(Ease.InBack));

            CloseSequence.OnComplete(OnComplete);

            void OnComplete()
            {
                gameObject.SetActive(false);
                OnScreenCloseEnded();
            }
        }

        protected virtual void ClosePanel()
        {
            UIScreenManager.Instance.CloseScreen(this);
        }

        [Button]
        protected virtual void SetRefs()
        {
            screenBackground = transform.FindDeepChild<Image>("Background");
            mainPanel = transform.FindDeepChild<RectTransform>("MainPanel");
            backgroundButton = screenBackground.GetComponent<Button>();
        }
    }
}
