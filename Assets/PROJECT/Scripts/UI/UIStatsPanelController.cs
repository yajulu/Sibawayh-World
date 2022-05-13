using _YajuluSDK._Scripts.Essentials;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace PROJECT.Scripts.UI
{
    public class UIStatsPanelController : MonoBehaviour
    {
        [SerializeField, TitleGroup("Properties"), OnValueChanged(nameof(UpdateTitles))] private string gameModeTitle;
        [SerializeField, TitleGroup("Properties"), OnValueChanged(nameof(UpdateTitles))] private string gameModeSubtitle;
        
        
        [SerializeField, TitleGroup("Refs")] private TextMeshProUGUI gameModeTitleText;
        [SerializeField, TitleGroup("Refs")] private TextMeshProUGUI gameModeSubtitleText;

        public string GameModeTitle
        {
            get => gameModeTitle;
            set
            {
                gameModeTitle = value;
                UpdateTitles();
            }
        }

        public string GameModeSubtitle
        {
            get => gameModeSubtitle;
            set
            {
                gameModeSubtitle = value;
                UpdateTitles();
            }
        }
        
        [Button, TitleGroup("Properties")]
        private void UpdateTitles()
        {
            gameModeTitleText.SetText(gameModeTitle);
            gameModeSubtitleText.SetText(gameModeSubtitle);
        }

        [Button, TitleGroup("Refs")]
        private void SetRefs()
        {
            gameModeTitleText = transform.FindDeepChild<TextMeshProUGUI>("GameModeTitle_Text");
            gameModeSubtitleText = transform.FindDeepChild<TextMeshProUGUI>("GameModeSubtitle_Text");
        }
        
    }
}
