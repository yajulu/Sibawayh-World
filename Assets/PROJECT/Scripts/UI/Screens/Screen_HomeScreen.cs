using _YajuluSDK._Scripts.UI;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using PROJECT.Scripts.Game.Controllers;
using UnityEngine;

namespace PROJECT.Scripts.UI.Screens
{
    public class Screen_HomeScreen : UIScreenBase
    {
        [SerializeField]
        Transform logo_Image, buttons_Panel;
        
        protected override void OpenAnimation()
        {
            base.OpenAnimation();
            OpenSequence.Append(logo_Image.DOScale(1, 0.6f)
                .SetEase(Ease.OutBack)
                .From(0));

            OpenSequence.Join(buttons_Panel.DOLocalMoveX(buttons_Panel.transform.localPosition.x, 0.6f)
                .SetEase(Ease.OutBack)
                .From(-600)
                .SetRelative(true));
        }

        protected override void CloseAnimation()
        {
            base.CloseAnimation();
            CloseSequence.Prepend(logo_Image.transform
                .DOScale(0, 0.3f)
                .SetEase(Ease.InBack)
                .From(1));
        }
    }
}

