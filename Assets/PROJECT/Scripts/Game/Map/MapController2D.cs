using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _YajuluSDK._Scripts.GameConfig;
using _YajuluSDK._Scripts.UI;
using Project.Scripts.Data;
using PROJECT.Scripts.Game.Controllers;
using PROJECT.Scripts.UI;
using PROJECT.Scripts.UI.Screens;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace PROJECT.Scripts.Game.Map
{
    public class MapController2D : MonoBehaviour
    {
        [SerializeField, TitleGroup("Properties"), MinValue(0)]
        private int stonesPerLevel = 3;
        
        [SerializeField, TitleGroup("Properties")]
        private int numberOfLevels;
        
        [SerializeField, TitleGroup("Scroll"), MinValue(0)]
        private float deceleration;
        
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
        private Vector2 _dummyPosition;
        private Vector2 _inertia;
        private Vector3 _initialClickToObjectOriginDelta;
        

        private Camera _main;

        private float _screenFactor;
        private Vector3 _initialClickWordSpace;
        private InputAction _tab;
        private InputAction _tabPosition;
        private InputAction _scroll;
        private EventSystem _current;

        private void Awake()
        {
            _input = new MainInput();
            _main = Camera.main;
            _tab = _input.Map.Tab;
            _tabPosition = _input.Map.TabPosition;
            _scroll = _input.Map.Scroll;
            _current = EventSystem.current;
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

        private void UpdateButtonsAsync(PlayerProgress progress)
        {
            Task.Run(Activate);
        
            void Activate()
            {
                for (int i = 0; i < levelButtons.Count; i++)
                {
                    levelButtons[i].ButtonState = progress.GetLevelState(i);
                }
            }
        }

        private void UIScreenManagerOnScreenOpenStarted(UIScreenBase obj)
        {
            if (obj.GetType().Name.Equals(nameof(Screen_Map)))
            {
                mapHolder.gameObject.SetActive(true);
                // UpdateButtonsAsync(DataPersistenceManager.Instance.Progress);
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

        private void LateUpdate()
        {
            DragMap();
        }
        
        private void DragMap()
        {
            if (!_input.Map.enabled)
                return;
            
            if (_tab.WasPressedThisFrame() && !_current.IsPointerOverGameObject())
            {
                _initialClickToObjectOriginDelta = mapHolder.transform.position - _main.ScreenToWorldPoint(_tabPosition.ReadValue<Vector2>());
            }
            else if (_tab.IsPressed() && !_current.IsPointerOverGameObject())
            {
                _dummyPosition = mapHolder.position;
                _dummyNewPosition = _main.ScreenToWorldPoint(_tabPosition.ReadValue<Vector2>()) + _initialClickToObjectOriginDelta;
                _dummyNewPosition = _dummyNewPosition.Clamp(minScroll, maxScroll) - _dummyPosition;
                mapHolder.Translate(_dummyNewPosition);
                _inertia = (_inertia + (_dummyNewPosition / Time.deltaTime)) * 0.5f;
            }
            else if (_tab.WasReleasedThisFrame() && !_current.IsPointerOverGameObject())
            {
                // var scroll = _scroll.ReadValue<Vector2>();
                // _inertia = scroll.normalized * (_main.ScreenToWorldPoint(scroll).magnitude / Time.deltaTime);
            }
            else if (!_tab.IsPressed())
            {
                _inertia += -_inertia * (deceleration * Time.deltaTime);    
                if (_inertia.magnitude > 0.1f)
                {
                    _dummyPosition = mapHolder.position;
                    _dummyNewPosition = (_inertia * Time.deltaTime) + _dummyPosition;
                    _dummyNewPosition = _dummyNewPosition.Clamp(minScroll, maxScroll) - _dummyPosition;
                    mapHolder.Translate(_dummyNewPosition);
                }
            }
        }
    }
}
