using _YajuluSDK._Scripts.UI;
using DG.Tweening;
using UnityEngine.UI;

namespace PROJECT.Scripts.UI
{
    public class TestScreenTwo : UIScreenBase
    {
        private Button navButton;
        
        protected override void Awake()
        {
            base.Awake();
            navButton = GetComponentInChildren<Button>();
        }
        
        protected override void OnSkipOpenAnimation()
        {
            navButton.transform.DOComplete();
        }
        
        protected override void OnSkipCloseAnimation()
        {
            navButton.transform.DOComplete();
        }
        
        protected override void OpenAnimation()
        {
            gameObject.SetActive(true);
            navButton.transform.DOScale(1, 3)
                .SetEase(Ease.OutBounce)
                .From(0)
                .OnComplete(base.OpenAnimation);
        }
        
        protected override void CloseAnimation()
        {
            navButton.transform.DOScale(0, 3)
                .SetEase(Ease.InBack)
                .From(1)
                .OnComplete(base.CloseAnimation);
        }
    }
}
