using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.GameConfig;
using _YajuluSDK._Scripts.UI;
using PROJECT.Scripts.Enums;
using PROJECT.Scripts.Game.Controllers;
using PROJECT.Scripts.UI;
using RTLTMPro;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PROJECT.Scripts.Game.Map
{
    public class LevelButton2D : MonoBehaviour
    {

        [SerializeField, FoldoutGroup("Refs")] private SpriteRenderer[] stars;
        [SerializeField, FoldoutGroup("Refs")] private SpriteRenderer shadowImage;
        [SerializeField, FoldoutGroup("Refs")] private SpriteRenderer lightImage;
        [SerializeField, FoldoutGroup("Refs")] private Transform starsPanel;
        [SerializeField, FoldoutGroup("Refs")] private RTLTextMeshPro3D numberText;
        // [SerializeField, FoldoutGroup("Refs")] private Button levelButton;

        [SerializeField, TitleGroup("Properties"), PropertyOrder(-1), OnValueChanged(nameof(OnStateChanged))]
        private eLevelState buttonState;

        [SerializeField, TitleGroup("Properties"), PropertyOrder(-1), OnValueChanged(nameof(OnLevelNumberChanged))] 
        private int levelNumber;

        private LevelsVariablesEditor _levelsVariablesEditor => GameConfig.Instance.Levels;

        private bool _interactable = false;
        
        private static Color enabledColor = Color.white;
        private static Color disabledColor = new Color(1,1,1,0.5f);

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
            get => levelNumber;
            set
            {
                OnLevelNumberChanged(value);
                levelNumber = value;
            }
        }

        #region Unity Callbacks

        protected void OnEnable()
        {
            ButtonState = GameModeManager.Instance.GetLevelState(levelNumber);
        }
        
        #endregion

        private void OnStateChanged(eLevelState newState)
        {
            UpdateButtonUI(newState);
        }

        private void OnLevelNumberChanged(int newLevel)
        {
            numberText.text = newLevel.ToString();
        }

        private void OnMouseDown()
        {
            if (!_interactable)
                return;
            PlayLevel();
        }

        private void UpdateButtonUI(eLevelState newState, bool instant = true)
        {
            var state = (int)newState;
            if (instant)
            {
                starsPanel.gameObject.SetActive(state != 0);
                _interactable = state != 0;
                for (int i = 0; i < stars.Length; i++)
                {
                    stars[i].color = (i < state - 1 ? enabledColor : disabledColor);
                }
                lightImage.sprite = state == 0 ? _levelsVariablesEditor.levelButtonUI.Locked : 
                    state > 1 ? _levelsVariablesEditor.levelButtonUI.Complete : _levelsVariablesEditor.levelButtonUI.Unlocked;
            }
            
        }

        private void PlayLevel()
        {
            GameModeManager.Instance.CurrentLevel = levelNumber;
            UIScreenManager.Instance.OpenScreen(nameof(Panel_LevelSelection));
        }
        
        
        [Button, FoldoutGroup("Refs")]
        private void SetRefs()
        {
            starsPanel = transform.FindDeepChild<Transform>("LevelButtonStars");
            stars = starsPanel.GetComponentsInChildren<SpriteRenderer>();
            lightImage = transform.FindDeepChild<SpriteRenderer>("LevelButtonLight");
            shadowImage = transform.FindDeepChild<SpriteRenderer>("LevelButtonShadow");
            numberText = transform.FindDeepChild<RTLTextMeshPro3D>("LevelButtonNumber");
        }
        
    }
}
