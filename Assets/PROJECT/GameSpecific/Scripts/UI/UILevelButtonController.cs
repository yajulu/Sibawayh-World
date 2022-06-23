using Project.GameSpecific.Scripts.Game.Controllers;
using PROJECT.Scripts.Enums;
using Project.YajuluSDK.Scripts.Essentials;
using Project.YajuluSDK.Scripts.GameConfig;
using Project.YajuluSDK.Scripts.UI;
using RTLTMPro;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PROJECT.Scripts.UI
{
    public class UILevelButtonController : UIBehaviour
    {

        [SerializeField, FoldoutGroup("Refs")] private Image[] stars;
        [SerializeField, FoldoutGroup("Refs")] private Image shadowImage;
        [SerializeField, FoldoutGroup("Refs")] private Image lightImage;
        [SerializeField, FoldoutGroup("Refs")] private RectTransform starsPanel;
        [SerializeField, FoldoutGroup("Refs")] private RTLTextMeshPro numberText;
        [SerializeField, FoldoutGroup("Refs")] private Button levelButton;

        [SerializeField, TitleGroup("Properties"), PropertyOrder(-1), OnValueChanged(nameof(OnStateChanged))]
        private eLevelState buttonState;

        [SerializeField, TitleGroup("Properties"), PropertyOrder(-1), OnValueChanged(nameof(OnLevelNumberChanged))] 
        private int levelIndex;

        private LevelsVariablesEditor _levelsVariablesEditor => GameConfig.Instance.Levels;

        public eLevelState ButtonState
        {
            get => buttonState;
            set
            {
                OnStateChanged(value);
                buttonState = value;
            }
        }

        public int LevelNumber
        {
            get => levelIndex;
            set
            {
                OnLevelNumberChanged(value);
                levelIndex = value;
            }
        }

        #region Unity Callbacks

        protected override void OnEnable()
        {
            base.OnEnable();
            levelButton.onClick.AddListener(PlayLevel);
            ButtonState =  DataPersistenceManager.Instance.PlayerData.GetLevelState(levelIndex);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            levelButton.onClick.RemoveListener(PlayLevel);
        }

        #endregion

        private void OnStateChanged(eLevelState newState)
        {
            UpdateButtonUI(newState);
        }

        private void OnLevelNumberChanged(int newLevel)
        {
            numberText.text = (newLevel + 1).ToString();
        }
        
        private void UpdateButtonUI(eLevelState newState, bool instant = true)
        {
            var state = (int)newState;
            if (instant)
            {
                starsPanel.gameObject.SetActive(state != 0);
                levelButton.interactable = state != 0;
                for (int i = 0; i < stars.Length; i++)
                {
                    stars[i].canvasRenderer.SetAlpha(i < state - 1 ? 1 : 0.5f);
                }
                lightImage.sprite = state == 0 ? _levelsVariablesEditor.levelButtonUI.Locked : 
                    state > 1 ? _levelsVariablesEditor.levelButtonUI.Complete : _levelsVariablesEditor.levelButtonUI.Unlocked;
            }
            
        }

        private void PlayLevel()
        {
            GameModeManager.Instance.CurrentLevel = levelIndex;
            UIScreenManager.Instance.OpenScreen(nameof(Panel_LevelSelection));
        }
        
        
        [Button, FoldoutGroup("Refs")]
        private void SetRefs()
        {
            starsPanel = transform.FindDeepChild<RectTransform>("LevelButtonStars_Panel");
            stars = starsPanel.GetComponentsInChildren<Image>();
            lightImage = transform.FindDeepChild<Image>("LevelButtonLight_Image");
            shadowImage = transform.FindDeepChild<Image>("LevelButtonShadow_Image");
            numberText = transform.FindDeepChild<RTLTextMeshPro>("LevelButtonNumber_Text");
            levelButton = lightImage.GetComponent<Button>();
        }
        
    }
}
