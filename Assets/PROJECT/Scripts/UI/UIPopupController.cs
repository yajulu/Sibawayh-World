using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTLTMPro;
using Sirenix.OdinInspector;
using _YajuluSDK._Scripts.Essentials;
using System;
using UnityEngine.Events;
using DG.Tweening;

public class UIPopupController : MonoBehaviour
{
    [SerializeField] private Image itemIconImage;
    [SerializeField] private Image smallIconImage;
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

    protected Sequence OpenSequence;
    protected Sequence CloseSequence;

    protected void Awake()
    {
        buttonActions = new Action[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(() => OnAnyButtonClicked(i));
        }

        backgroundButton.onClick.AddListener(() => OnAnyButtonClicked(-1));

    }

    protected void OnDisable()
    {
        for (var i  = 0; i < buttonActions.Length; i++)
        {
            buttonActions[i] = () => { };
        }
    }

    public void ShowPopup(List<PopupButtonAction> buttonsConfig, string msg, Sprite icon, bool isItem)
    {
        if (isItem)
            itemIconImage.sprite = icon;
        else
            smallIconImage.sprite = icon;
        
        smallIconImage.gameObject.SetActive(isItem);
        itemIconPanel.gameObject.SetActive(!isItem);

        popupMessageText.text = msg;

        buttonsPanel.gameObject.SetActive(buttonsConfig.Count > 0);

        int i = 0;

        for (; i < buttonsConfig.Count; i++)
        {
            if (i < buttons.Length)
            {
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
            backgroundImage.DOFade(1f, 0.2f)
            .From(0f)
            .SetEase(Ease.Linear));

        OpenSequence.Join(
            mainPanel.DOScale(1f, 0.2f)
            .From(0.3f)
            .SetEase(Ease.OutBack));

    }

    protected void CloseAnimation()
    {
        CloseSequence = DOTween.Sequence();

        CloseSequence.OnComplete(() => gameObject.SetActive(false));

        CloseSequence.Append(
            backgroundImage.DOFade(0f, 0.2f)
            .From(1f)
            .SetEase(Ease.Linear));

        CloseSequence.Join(
            mainPanel.DOScale(0.3f, 0.2f)
            .From(1f)
            .SetEase(Ease.OutBack));
    }


    [Button]
    private void SetRefs()
    {
        popupMessageText = transform.FindDeepChild<RTLTextMeshPro>("PopupMainText_Text");
        smallIconImage = transform.FindDeepChild<Image>("Icon_Image");
        itemIconPanel = transform.FindDeepChild<RectTransform>("ItemIcon_Panel");
        itemIconImage = transform.FindDeepChild<Image>("ItemIcon_Image");
        buttonsPanel = transform.FindDeepChild<RectTransform>("Buttons_Panel");
        buttonIcons = buttonsPanel.GetComponentsInChildren<Image>(true);
        buttons = buttonsPanel.GetComponentsInChildren<Button>(true);
        buttonTexts = buttonsPanel.GetComponentsInChildren<RTLTextMeshPro>(true);
        backgroundButton = transform.FindDeepChild<Button>("PopupBackground_Image");
        backgroundImage = transform.FindDeepChild<Image>("PopupBackground_Image");
    }
}
