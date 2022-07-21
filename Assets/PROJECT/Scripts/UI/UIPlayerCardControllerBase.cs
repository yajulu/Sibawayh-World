using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.GameConfig;
using Project.Scripts.Inventory;
using RTLTMPro;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace PROJECT.Scripts.UI
{
    public class UIPlayerCardControllerBase : MonoBehaviour
    {
        [SerializeField, TitleGroup("Refs")] private RTLTextMeshPro playerDisplayNameText;
        [SerializeField, TitleGroup("Refs")] private RTLTextMeshPro playerLevelText;
        [SerializeField, TitleGroup("Refs")] private Image profileIconImage;
        [SerializeField, TitleGroup("Refs")] private Image bannerImage;

        
        public virtual void SetPlayerData(string displayName, int level, ProfileData data)
        {
            playerLevelText.text = level.ToString();
            playerDisplayNameText.text = displayName;
            bannerImage.sprite = GameConfig.Instance.Shop.ShopItemIDDictionary[data.Banner.ItemID];
            profileIconImage.sprite = GameConfig.Instance.Shop.ShopItemIDDictionary[data.Icon.ItemID];
        }

        public virtual void SetPlayerProfileItems(Sprite banner, Sprite icon)
        {
            bannerImage.sprite = banner;
            profileIconImage.sprite = icon;
        }

        public virtual void UpdatePlayerDisplayName(string displayName)
        {
            playerDisplayNameText.text = displayName;
        }

        [Button]
        protected virtual void SetRefs()
        {
            playerDisplayNameText = transform.FindDeepChild<RTLTextMeshPro>("ProfileName_Text");
            playerLevelText = transform.FindDeepChild<RTLTextMeshPro>("ProfileName_Text");
            profileIconImage = transform.FindDeepChild<Image>("ProfileIcon_Image");
            bannerImage = transform.FindDeepChild<Image>("ProfileBanner_Image");
        }
    }
}
