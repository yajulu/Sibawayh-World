using System.Collections.Generic;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.GameConfig;
using PROJECT.Scripts.Enums;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
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

        [SerializeField, TitleGroup("Properties"), PropertyOrder(-1), OnValueChanged(nameof(OnStateChanged))]
        private eLevelState buttonState;

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

        private void OnStateChanged(eLevelState newState)
        {
            UpdateButtonUI();
        }
        
        private void UpdateButtonUI(bool instant = true)
        {
            var state = (int)buttonState;
            if (instant)
            {
                starsPanel.gameObject.SetActive(state != 0);
                for (int i = 0; i < stars.Length; i++)
                {
                    stars[i].canvasRenderer.SetAlpha(i < state - 1 ? 1 : 0.5f);
                }
                lightImage.sprite = state == 0 ? _levelsVariablesEditor.levelButtonUI.Locked : 
                    state > 1 ? _levelsVariablesEditor.levelButtonUI.Complete : _levelsVariablesEditor.levelButtonUI.Unlocked;
            }
            
        }

        [Button, FoldoutGroup("Refs")]
        private void SetRefs()
        {
            starsPanel = transform.FindDeepChild<RectTransform>("LevelButtonStars_Panel");
            stars = starsPanel.GetComponentsInChildren<Image>();
            lightImage = transform.FindDeepChild<Image>("LevelButtonLight_Image");
            shadowImage = transform.FindDeepChild<Image>("LevelButtonShadow_Image");
        }
        
    }
}
