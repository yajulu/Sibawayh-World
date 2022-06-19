using System;
using System.Collections.Generic;
using _YajuluSDK._Scripts.GameConfig;
using _YajuluSDK._Scripts.UI;
using PROJECT.Scripts.UI;
using PROJECT.Scripts.UI.Screens;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PROJECT.Scripts.Game.Map
{
    public class MapController2D : MonoBehaviour
    {
        [SerializeField, TitleGroup("Properties"), MinValue(0)]
        private int stonesPerLevel = 3;
        
        [SerializeField, TitleGroup("Properties")]
        private int numberOfLevels;

        [SerializeField, TitleGroup("Scroll"), MinValue(0)]
        private float scrollSpeed = 10f;
        
        [SerializeField, TitleGroup("Scroll")]
        private Vector2 minScroll;
        
        [SerializeField, TitleGroup("Scroll")]
        private Vector2 maxScroll;
        
        [SerializeField, TitleGroup("Refs")] private List<LevelButton2D> levelButtons;
        [SerializeField, TitleGroup("Refs")] private List<SpriteRenderer> stones;
        [SerializeField, TitleGroup("Refs")] private Transform mapStonesParent;
        [SerializeField, TitleGroup("Refs")] private Transform mapHolder;

        private LevelButton2D _dummyLevelButton;
        private SpriteRenderer _dummyStoneImage;
        private GameObject _dummyPrefab;

        private GameObject _buttonPrefabRef;
        private GameObject _stonePrefabRef;

        private MainInput _input;

        private Vector2 _dummyNewPosition;
        private Vector2 _dummyLocalPosition;

        private void Awake()
        {
            _input = new MainInput();
        }

        private void OnEnable()
        {
            UIScreenManager.OnScreenOpenStarted += UIScreenManagerOnScreenOpenStarted;
            UIScreenManager.OnScreenCloseEnded += UIScreenManagerOnScreenCloseEnded;
            mapHolder.gameObject.SetActive(false);
        }
        
        private void OnDisable()
        {
            UIScreenManager.OnScreenOpenStarted -= UIScreenManagerOnScreenOpenStarted;
            UIScreenManager.OnScreenCloseEnded -= UIScreenManagerOnScreenCloseEnded;
        }
        
        private void UIScreenManagerOnScreenOpenStarted(UIScreenBase obj)
        {
            if (obj.GetType().Name.Equals(nameof(Screen_Map)))
            {
                mapHolder.gameObject.SetActive(true);
                _input.Map.Enable();
            }
        }
        
        private void UIScreenManagerOnScreenCloseEnded(UIScreenBase obj)
        {
            if (obj.GetType().Name.Equals(nameof(Screen_Map)))
            {
                mapHolder.gameObject.SetActive(false);
                _input.Map.Disable();
            }
        }
        
        [Button]
        private void SpawnLevelButtons()
        {
            // for (int i = 0; i < mapStonesParent.childCount; i++)
            // {
            //     DestroyImmediate(mapStonesParent.GetChild(i));
            // }

            _buttonPrefabRef = GameConfig.Instance.Levels.levelButtonUI.Button2DPrefab;
            _stonePrefabRef = GameConfig.Instance.Levels.levelButtonUI.Stone2DPrefab;
            
            levelButtons.Clear();
            stones.Clear();
            
            for (int i = 0; i < numberOfLevels; i++)
            {
                _dummyPrefab = Instantiate(_buttonPrefabRef, mapStonesParent);
                _dummyLevelButton = _dummyPrefab.transform.GetComponent<LevelButton2D>();
                _dummyLevelButton.LevelNumber = i;
                levelButtons.Add(_dummyLevelButton);
                
                for (int j = 0; j < stonesPerLevel; j++)
                {
                    _dummyPrefab = Instantiate(_stonePrefabRef, mapStonesParent);
                    _dummyStoneImage = _dummyPrefab.GetComponent<SpriteRenderer>();
                    
                    stones.Add(_dummyStoneImage);
                }
            }

        }
        
        [Button]
        private void Clear()
        {
            var childCount = mapStonesParent.childCount;
            for (int i = 0; i < childCount; i++)
            {
                DestroyImmediate(mapStonesParent.GetChild(0).gameObject);
            }
        }

        private void OnMouseDrag()
        {
            _dummyLocalPosition = mapHolder.localPosition;
            _dummyNewPosition = _dummyLocalPosition + (_input.Map.Scroll.ReadValue<Vector2>() * scrollSpeed);
            _dummyNewPosition = _dummyNewPosition.Clamp(minScroll, maxScroll) - _dummyLocalPosition;
            mapHolder.Translate(_dummyNewPosition, Space.Self);
        }
    }
}
