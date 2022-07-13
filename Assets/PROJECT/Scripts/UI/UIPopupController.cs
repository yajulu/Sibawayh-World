using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTLTMPro;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using _YajuluSDK._Scripts.Essentials;
using System;
using UnityEngine.Events;
using DG.Tweening;

public class UIPopupController : MonoBehaviour
{
    [SerializeField] private Image itemIconImage;
    [SerializeField] private Image smallIconImage;
    [SerializeField] private RectTransform itemHighlight;
    [SerializeField] private RectTransform itemIconPanel;
    [SerializeField] private RectTransform buttonsPanel;
    [SerializeField] private RectTransform mainPanel;
    [SerializeField] private RTLTextMeshPro popupMessageText;

    [SerializeField] private Button backgroundButton;
    [SerializeField] private Image backgroundImage;

    [SerializeField] private Button[] buttons;
    [SerializeField] private Image[] buttonIcons;
    [SerializeField] private RTLTextMeshPro[] buttonTexts;

    private Action[] buttonActions;

    private Action cancelAction;

    private bool interactable = true;

    public event Action<UIPopupController> OnPopUpOpened;
    public event Action<UIPopupController> OnPopUpClosed;

    protected Sequence OpenSequence;
    protected Sequence CloseSequence;

    protected void OnDisable()
    {
        for (var i  = 0; i < buttonActions.Length; i++)
        {
            buttonActions[i] = () => { };
        }
    }

    public void ShowPopup(PopupRequest request)
    {
        ShowPopup(request.ButtonsConfig, request.Msg, request.Icon, request.IconType, request.PopupSize, request.CancelAction);
    }

    public void ShowPopup(List<PopupButtonAction> buttonsConfig, string msg, Sprite icon, PopupIconType iconType, float popupSize, Action cancelAction)
    {
        switch (iconType)
        {
            case PopupIconType.smallIcon:
                itemIconImage.sprite = icon;
                break;
            case PopupIconType.ItemIconHighlighted:
            case PopupIconType.ItemIconNormal:
                itemIconImage.sprite = icon;
                break;
        }

        
        smallIconImage.gameObject.SetActive(iconType == PopupIconType.smallIcon);
        itemIconPanel.gameObject.SetActive(iconType != PopupIconType.smallIcon);
        itemHighlight.gameObject.SetActive(iconType == PopupIconType.ItemIconHighlighted);

        this.cancelAction = cancelAction;

        popupMessageText.text = msg;

        buttonsPanel.gameObject.SetActive(buttonsConfig.Count > 0);

        int i = 0;

        if (buttonActions.IsNullOrEmpty())
        {
            InitializeButtonListeners();
        }

        for (; i < buttonsConfig.Count; i++)
        {
            if (i < buttons.Length)
            {
                Debug.Log(buttonActions.Length);
                buttonActions[i] = buttonsConfig[i].buttonAction;
                buttonTexts[i].text = buttonsConfig[i].buttonText;
                buttons[i].gameObject.SetActive(true);
            }
            else
                break;            
        }

        for (;i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }

        interactable = true;
        OpenAnimation();
    }

    public struct PopupButtonAction
    {
        public Action buttonAction;
        public string buttonText;
    }

    protected virtual void OnAnyButtonClicked(int index)
    {
        interactable = false;
        if (index == -1)
            cancelAction?.Invoke();
        else
            buttonActions[index]?.Invoke();
        CloseAnimation();
    }

    protected void OpenAnimation()
    {
        OpenSequence = DOTween.Sequence();

        OpenSequence.OnStart(() => gameObject.SetActive(true));

        OpenSequence.Append(
            backgroundImage.DOFade(0.3f, 0.3f)
            .From(0f)
            .SetEase(Ease.Linear));

        OpenSequence.Join(
            mainPanel.DOScale(1f, 0.3f)
            .From(0.5f)
            .SetEase(Ease.OutBack));

    }

    protected void CloseAnimation()
    {
        CloseSequence = DOTween.Sequence();

        CloseSequence.OnComplete(() => gameObject.SetActive(false));

        CloseSequence.Append(
            backgroundImage.DOFade(0f, 0.3f)
            .From(0.3f)
            .SetEase(Ease.Linear));

        CloseSequence.Join(
            mainPanel.DOScale(0.5f, 0.3f)
            .From(1f)
            .SetEase(Ease.InBack));
    }

    public class PopupRequest
    {
        public List<PopupButtonAction> ButtonsConfig;
        public Action CancelAction;
        public string Msg;
        public Sprite Icon;
        public PopupIconType IconType;
        public float PopupSize;
    }

    public enum PopupIconType
    {
        smallIcon,
        ItemIconNormal,
        ItemIconHighlighted
    }

    private void InitializeButtonListeners()
    {
        buttonActions = new Action[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            var index = i;
            buttons[i].onClick.AddListener(() => OnAnyButtonClicked(index));
        }

        backgroundButton.onClick.AddListener(() => OnAnyButtonClicked(-1));
    }


    [Button]
    private void SetRefs()
    {
        mainPanel = transform.FindDeepChild<RectTransform>("PopupMain_Panel");
        popupMessageText = transform.FindDeepChild<RTLTextMeshPro>("PopupMainText_Text");
        smallIconImage = transform.FindDeepChild<Image>("Icon_Image");
        itemIconPanel = transform.FindDeepChild<RectTransform>("ItemIcon_Panel");
        itemIconImage = transform.FindDeepChild<Image>("ItemIcon_Image");
        buttonsPanel = transform.FindDeepChild<RectTransform>("Buttons_Panel");
        itemHighlight = transform.FindDeepChild<RectTransform>("ItemHighLight");
        buttonIcons = buttonsPanel.GetComponentsInChildren<Image>(true);
        buttons = buttonsPanel.GetComponentsInChildren<Button>(true);
        buttonTexts = buttonsPanel.GetComponentsInChildren<RTLTextMeshPro>(true);
        backgroundButton = transform.FindDeepChild<Button>("PopupBackground_Image");
        backgroundImage = transform.FindDeepChild<Image>("PopupBackground_Image");
    }
}
