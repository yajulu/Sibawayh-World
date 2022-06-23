using DG.Tweening;
using Project.YajuluSDK.Scripts.UI;
using UnityEngine;

namespace Project.GameSpecific.Scripts.UI.Screens
{
    public class Screen_HomeScreen : UIScreenBase
    {
        [SerializeField]
        Transform logo_Image, buttons_Panel;
        
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
                .SetEase(Ease.OutBack)
                .From(0)
                .OnComplete(base.OpenAnimation);

            buttons_Panel.DOLocalMoveX(buttons_Panel.transform.localPosition.x, 0.6f)
                .SetEase(Ease.OutBack)
                .From(-600)
                .SetRelative(true)
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

