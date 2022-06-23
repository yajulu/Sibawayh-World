using PROJECT.Scripts.UI;
using Project.YajuluSDK.Scripts.Essentials;
using Project.YajuluSDK.Scripts.Social;
using Project.YajuluSDK.Scripts.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.GameSpecific.Scripts.UI.Screens
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
