using System;
using System.Collections.Generic;
using _YajuluSDK._Scripts.Essentials;
using DG.Tweening;
using PROJECT.Scripts.Game.Controllers;
using RTLTMPro;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PROJECT.Scripts.UI
{
    public class UIStatsPanelController : MonoBehaviour
    {
        [SerializeField, TitleGroup("Properties"), OnValueChanged(nameof(UpdateTitles))] private string gameModeTitle;
        [SerializeField, TitleGroup("Properties"), OnValueChanged(nameof(UpdateTitles))] private string gameModeSubtitle;
        
        
        [SerializeField, TitleGroup("Refs")] private RTLTextMeshPro gameModeTitleText;
        [SerializeField, TitleGroup("Refs")] private RTLTextMeshPro gameModeSubtitleText;
        [SerializeField, TitleGroup("Refs")] private Transform progressBarTransform;
        [SerializeField, TitleGroup("Refs"), ReadOnly] private Image[] progressBarElementsList;

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

        private void OnEnable()
        {
            GameModeManager.Instance.GameModeStarted += GameModeStarted;
            GameModeManager.Instance.GameModeCheckWord += CheckWord;
        }
        
        private void OnDisable()
        {
            if (Singleton.Quitting) return;
            
            GameModeManager.Instance.GameModeStarted -= GameModeStarted;
            GameModeManager.Instance.GameModeCheckWord -= CheckWord;

        }

        private void GameModeStarted()
        {
            var color = UnityEngine.Color.white;
            color.a = 0.4f;
            foreach (var element in progressBarElementsList)
            {
                element.color = color;
            }
        }
        
        private void CheckWord(string word, bool check)
        {
            if (!check)
                return;
            var image = progressBarElementsList[GameModeManager.Instance.CurrentWordIndex];
            image.color = Color.white;
            image.transform.DOPunchScale(Vector3.one, 0.5f, 5);
        }

        [Button, TitleGroup("Properties")]
        private void UpdateTitles()
        {
            gameModeTitleText.text = gameModeTitle;
            gameModeSubtitleText.text = gameModeSubtitle;
        }

        [Button, TitleGroup("Refs")]
        private void SetRefs()
        {
            gameModeTitleText = transform.FindDeepChild<RTLTextMeshPro>("GameModeTitle_Text");
            gameModeSubtitleText = transform.FindDeepChild<RTLTextMeshPro>("GameModeSubtitle_Text");
            progressBarTransform = transform.FindDeepChild<Transform>("ProgressPoints");
            progressBarElementsList = progressBarTransform.GetComponentsInChildren<Image>(true);
        }
        
    }
}
