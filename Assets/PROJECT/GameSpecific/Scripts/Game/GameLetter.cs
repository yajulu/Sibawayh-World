using System;
using DG.Tweening;
using Project.YajuluSDK.Scripts.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PROJECT.Scripts.Game
{
    public class GameLetter : UIElementBase, IPointerDownHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
    {
        [SerializeField] private string letter = "";
        
        [SerializeField, ReadOnly] private Image image;
        [SerializeField, ReadOnly] private TextMeshProUGUI letterUI;
        [SerializeField, ReadOnly] private LetterSpawner letterSpawner;
        
        private Tweener _tweener;
        private Vector3 _dummyVector3One;

        private bool _selected = false;
        private int _buttonIndex;

        public Action<GameLetter, bool> OnButtonToggled;
        public Action OnButtonUp;

        public string Letter
        {
            get => letter;
            set
            {
                letter = value;
                letterUI.SetText(letter);
                ResetButton();
            }
        }

        [Button]
        private void SetRefs()
        {
            image = GetComponent<Image>();
            letterUI = GetComponentInChildren<TextMeshProUGUI>();
            letterSpawner = GetComponentInParent<LetterSpawner>();
        }

        private void Awake()
        {
            _dummyVector3One = Vector3.one;
            _buttonIndex = transform.GetSiblingIndex();
        }

        private void OnEnable()
        {
            letterSpawner.OnResetWord += ResetButton;
        }

        private void OnDisable()
        {
            letterSpawner.OnResetWord -= ResetButton;
        }

        #region PointerCallBacks
        public void OnPointerDown(PointerEventData eventData)
        {

            if (_selected)
            {
                DeselectButton();
            }
            else
            {
                SelectButton();
            }
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            // if(!_selected)
            //     SelectButton();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_selected)
                return;
    
            EnterExitAnimation(true);
            if (eventData.dragging)
            {
                SelectButton();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_selected)
                return;
            
            EnterExitAnimation(false);
        }
        
        // public void OnEndDrag(PointerEventData eventData)
        // {
        //     OnButtonUpOrDragEnded?.Invoke();
        // }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnButtonUp?.Invoke();
        }

        #endregion
        
        private void SelectButton()
        {
            _selected = true;
            image.color = Color.gray;
            OnButtonToggled?.Invoke(this, true);
        }

        private void DeselectButton()
        {
            _selected = false;
            image.color = Color.white;
            OnButtonToggled?.Invoke(this, false);
        }

        private void ResetButton()
        {
            DeselectButton();
            _tweener.Complete();
            transform.localScale = Vector3.one;
        }
        

        void EnterExitAnimation(bool enter)
        {
            _tweener?.Complete();

            if (enter)
            {
                _tweener = transform.DOScale(1.15f, 0.2f)
                    .SetEase(Ease.OutBack, 2f);
            }
            else
            {
                _tweener = transform.DOScale(1f, 0.2f)
                    .SetEase(Ease.OutQuad, 2f);
            }
        }
        
    }
}
