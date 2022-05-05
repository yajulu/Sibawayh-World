using System;
using System.Reflection;
using _YajuluSDK._Scripts.UI;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PROJECT.Scripts.Game
{
    public class GameLetter : UIElementBase, IPointerDownHandler, IDragHandler, IPointerMoveHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private Image _image;
        private Button _button;
        
        private Tweener _tweener;
        private Vector3 _dummyVector3One;

        private bool _selected = false;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _dummyVector3One = Vector3.one;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }

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

        public void OnPointerMove(PointerEventData eventData)
        {
            Debug.Log("Move");
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

        private void SelectButton()
        {
            _selected = true;
            _image.color = Color.gray;
        }

        private void DeselectButton()
        {
            _selected = false;
            _image.color = Color.white;
        }
        

        void EnterExitAnimation(bool enter)
        {
            if (_tweener == null)
            {
                _tweener =  transform.DOScale(1.25f, 0.2f)
                    .SetEase(Ease.OutBack, 2f)
                    .Pause()
                    .SetAutoKill(false);
            }
            
            if (enter)
            {
                _tweener.PlayForward();
            }
            else
            {
                _tweener.PlayBackwards();
            }
        }
    }
}
