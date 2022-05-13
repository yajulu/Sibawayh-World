using _YajuluSDK._Scripts.UI;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PROJECT.Scripts.UI.Screens
{
    public class Screen_HomeScreen : UIScreenBase
    {
        [SerializeField]
        Transform logo_Image, buttons_Panel;

        private float buttons_panelEndValue;

        protected override void Awake()
        {
            base.Awake();
            buttons_panelEndValue = buttons_Panel.localPosition.x;

        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }



        protected override void OnSkipOpenAnimation()
        {
            logo_Image.transform.DOComplete();
        }

        protected override void OnSkipCloseAnimation()
        {
            logo_Image.transform.DOComplete();
        }



        protected override void OpenAnimation()
        {
            gameObject.SetActive(true);
            logo_Image.DOScale(1, 0.6f)
                .SetEase(Ease.OutBounce)
                .From(0)
                .OnComplete(base.OpenAnimation);

            buttons_Panel.DOLocalMoveX(buttons_panelEndValue, 0.6f)
                .SetEase(Ease.OutBounce)
                .From(buttons_panelEndValue - 200)
                .OnComplete(base.OpenAnimation);
        }

        protected override void CloseAnimation()
        {
            logo_Image.transform.DOScale(0, 0.3f)
                .SetEase(Ease.InBack)
                .From(1)
                .OnComplete(base.CloseAnimation);
        }
    }
}

