using _YajuluSDK._Scripts.Essentials;
using RTLTMPro;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace PROJECT.Scripts.UI
{
    public class PlayerCardController : MonoBehaviour
    {
        [SerializeField, TitleGroup("Refs")] private RTLTextMeshPro playerDisplayNameText;
        [SerializeField, TitleGroup("Refs")] private RTLTextMeshPro playerLevelText;
        [SerializeField, TitleGroup("Refs")] private RTLTextMeshPro expValText;
        [SerializeField, TitleGroup("Refs")] private Image profileIconImage;
        [SerializeField, TitleGroup("Refs")] private Image bannerImage;


        public void SetPlayerData(string displayName, int level, int currExp, int maxExp, Sprite icon)
        {
            playerLevelText.text = level.ToString();
            playerDisplayNameText.text = displayName;
            expValText.text = currExp.ToString() +"/"+ maxExp.ToString();
        }

        public void SetPlayerProfileItems(Sprite banner, Sprite icon)
        {
            bannerImage.sprite = banner;
            profileIconImage.sprite = icon;
        }

        public void UpdatePlayerDisplayName(string displayName)
        {
            playerDisplayNameText.text = displayName;
        }

        [Button]
        private void SetRefs()
        {
            playerDisplayNameText = transform.FindDeepChild<RTLTextMeshPro>("ProfileName_Text");
            playerLevelText = transform.FindDeepChild<RTLTextMeshPro>("ProfileName_Text");
            expValText = transform.FindDeepChild<RTLTextMeshPro>("ProgressPanel_Text");
            profileIconImage = transform.FindDeepChild<Image>("ProfileIcon_Image");
            bannerImage = transform.FindDeepChild<Image>("ProfileBanner_Image");
        }
    }
}
