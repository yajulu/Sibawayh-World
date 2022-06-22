using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.Social;
using _YajuluSDK._Scripts.UI;
using RTLTMPro;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PROJECT.Scripts.UI.Screens
{
    public class Screen_Profile : UIScreenBase
    {

        [SerializeField] private PlayerCardController playerDisplayNameText;
        protected override void OnScreenOpenStarted()
        {
            playerDisplayNameText.UpdatePlayerDisplayName(PlayFabHandler.Instance.CachedPlayer.DisplayName);
            base.OnScreenOpenStarted();
        }

        [Button]
        private void SetRefs()
        {
            playerDisplayNameText = transform.FindDeepChild<PlayerCardController>("Profile_Panel");
        }
    }
}
