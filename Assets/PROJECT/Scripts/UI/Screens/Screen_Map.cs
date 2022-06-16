using _YajuluSDK._Scripts.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace PROJECT.Scripts.UI.Screens
{
    public class Screen_Map : UIScreenBase
    {
        [SerializeField, TitleGroup("Refs")] private RectTransform statsPanelPlaceholder;
        [SerializeField, TitleGroup("Refs")] 
        protected override void CloseAnimation()
        {
            base.CloseAnimation();
        }

        [Button, TitleGroup("Refs")]
        private void SetRefs()
        {
            
        }
    }
}
