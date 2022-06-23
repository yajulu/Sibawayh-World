using Project.GameSpecific.Scripts.Game.Controllers;
using PROJECT.Scripts.Enums;
using PROJECT.Scripts.UI;
using Project.YajuluSDK.Scripts.Essentials;
using Project.YajuluSDK.Scripts.GameConfig;
using Project.YajuluSDK.Scripts.UI;
using RTLTMPro;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.GameSpecific.Scripts.Game.Map
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
        private int levelIndex;

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
            get => levelIndex;
            set
            {
                OnLevelNumberChanged(value);
                levelIndex = value;
            }
        }

        #region Unity Callbacks

        protected void OnEnable()
        {
            ButtonState = DataPersistenceManager.Instance.PlayerData.GetLevelState(levelIndex);
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

        private void OnMouseUpAsButton()
        {
            if (!_interactable || EventSystem.current.IsPointerOverGameObject())
                return;
            ShowLevelSelection();
        }

        private void UpdateButtonUI(eLevelState newState, bool instant = true)
        {
            var state = (int)newState;
            if (instant)
            {
                _interactable = state != 0;
                if (state > 1)
                {
                    starsPanel.gameObject.SetActive(true);
                    for (int i = 0; i < stars.Length; i++)
                    {
                        stars[i].color = (i < state - 1 ? enabledColor : disabledColor);
                    }    
                }
                
                lightImage.sprite = state == 0 ? _levelsVariablesEditor.levelButtonUI.Locked : 
                    state > 1 ? _levelsVariablesEditor.levelButtonUI.Complete : _levelsVariablesEditor.levelButtonUI.Unlocked;
            }
            
        }

        private void ShowLevelSelection()
        {
            GameModeManager.Instance.CurrentLevel = levelIndex;
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
