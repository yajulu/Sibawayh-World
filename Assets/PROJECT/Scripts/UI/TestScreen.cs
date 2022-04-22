using System;
using _YajuluSDK._Scripts.UI;
using DG.Tweening;
using UnityEngine.UI;

namespace PROJECT.Scripts.UI
{
    public class TestScreen : UIScreenBase
    {
        // Start is called before the first frame update

        private Button navButton;
        
        protected override void Awake()
        {
            base.Awake();
            navButton = GetComponentInChildren<Button>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        protected override void OpenAnimation()
        {
            gameObject.SetActive(true);
            navButton.transform.DOScale(1, 1)
                .SetEase(Ease.OutBounce)
                .From(0)
                .OnComplete(base.OpenAnimation);
        }
    }
}
