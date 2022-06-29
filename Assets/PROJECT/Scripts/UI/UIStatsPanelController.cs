using System;
using System.Collections.Generic;
using _YajuluSDK._Scripts.Essentials;
using DG.Tweening;
using PROJECT.Scripts.Data;
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
        [SerializeField, TitleGroup("Refs")] private Transform progressBarElementsParentTransform;
        [SerializeField, TitleGroup("Refs")] private RectTransform progressBarTransform;
        [SerializeField, TitleGroup("Refs"), ReadOnly] private Image[] progressBarDotsList;
        [SerializeField, TitleGroup("Refs"), ReadOnly] private Image[] progressBarStarsList;

        private List<Image> _currentProgressBarElements;
        private Image _dummyProgressBarElement;

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

        public Transform GameModeProgressTransform => progressBarTransform;

        private void Awake()
        {
            _currentProgressBarElements = new List<Image>();
        }

        private void OnEnable()
        {
            GameModeManager.Instance.GameModeStarted += GameModeStarted;
            GameModeManager.Instance.GameModeCheckWord += CheckWord;
            ResetProgressBar();
        }
        
        private void OnDisable()
        {
            if (Singleton.Quitting) return;
            
            GameModeManager.Instance.GameModeStarted -= GameModeStarted;
            GameModeManager.Instance.GameModeCheckWord -= CheckWord;

        }

        private void GameModeStarted()
        {
            SetProgressBarElements(GameModeManager.Instance.CurrentLevelData);
            progressBarElementsParentTransform.gameObject.SetActive(true);
            UpdateTitlesToCurrentLevel();
        }

        public void UpdateTitlesToCurrentLevel()
        {
            gameModeTitle = GameModeManager.Instance.GetCurrentLevelTypeName();
            gameModeSubtitle = GameModeManager.Instance.CurrentLevelData.KeyWord;
            UpdateTitles();
        }

        public void SetProgressBarElements(LevelData levelData)
        {
            _currentProgressBarElements.Clear();
            
            for (int i = 0; i < progressBarDotsList.Length; i++)
            {
                if (i < levelData.Words.Length - 3)
                {
                    progressBarDotsList[i].gameObject.SetActive(true);
                    progressBarDotsList[i].canvasRenderer.SetAlpha(0.4f);
                    _currentProgressBarElements.Add(progressBarDotsList[i]);
                }
                else
                {
                    progressBarDotsList[i].gameObject.SetActive(false);
                }
                
            }

            for (int i = 0; i < progressBarStarsList.Length; i++)
            {
                progressBarStarsList[i].transform.SetSiblingIndex(levelData.StarsCounter[i]);
                progressBarStarsList[i].canvasRenderer.SetAlpha(0.4f);
                _currentProgressBarElements.Insert(levelData.StarsCounter[i], progressBarStarsList[i]);
                // if (i == 2)
                // {
                //     _currentProgressBarElements.Add(progressBarStarsList[i]);
                // }
                // else
                // {
                //     _currentProgressBarElements.Insert(levelData.StarsCounter[i], progressBarStarsList[i]);
                // }
            }
        }

        private void ResetProgressBar()
        {
            for (int i = 0; i < progressBarDotsList.Length; i++)
            {
                progressBarDotsList[i].canvasRenderer.SetAlpha(0.4f);
            }
            
            for (int i = 0; i < progressBarStarsList.Length; i++)
            {
                progressBarStarsList[i].canvasRenderer.SetAlpha(0.4f);
            }
        }
        
        private void CheckWord(string word, bool check)
        {
            if (!check)
                return;
            _dummyProgressBarElement = _currentProgressBarElements[GameModeManager.Instance.CurrentWordIndex];
            _dummyProgressBarElement.canvasRenderer.SetAlpha(1f);
            _dummyProgressBarElement.transform.DOPunchScale(Vector3.one, 0.5f, 5);
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
            progressBarTransform = transform.FindDeepChild<RectTransform>("ProgressPanel");
            progressBarElementsParentTransform = transform.FindDeepChild<Transform>("ProgressPoints");
            var list = progressBarElementsParentTransform.GetComponentsInChildren<Image>(true);
            var starsList = new List<Image>();
            var dotsList = new List<Image>();
            
            foreach (var element in list)
            {
                if (element.name.Contains("Star"))
                {
                    starsList.Add(element);
                }
                else
                {
                    dotsList.Add(element);
                }
            }
            
            progressBarStarsList = starsList.ToArray();
            progressBarDotsList = dotsList.ToArray();
        }
        
    }
}
