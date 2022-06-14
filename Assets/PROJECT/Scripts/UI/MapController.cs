using System.Collections.Generic;
using _YajuluSDK._Scripts.GameConfig;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace PROJECT.Scripts.UI
{
    public class MapController : MonoBehaviour
    {
        [SerializeField, TitleGroup("Properties"), MinValue(0)]
        private int stonesPerLevel = 3;

        [SerializeField, TitleGroup("Properties")]
        private int numberOfLevels;
        
        [SerializeField, TitleGroup("Refs")] private List<UILevelButtonController> levelButtons;
        [SerializeField, TitleGroup("Refs")] private List<Image> stones;
        [SerializeField, TitleGroup("Refs")] private RectTransform mapStonesParent;

        private UILevelButtonController _dummyLevelButton;
        private Image _dummyStoneImage;
        private GameObject _dummyPrefab;

        private GameObject _buttonPrefabRef;
        private GameObject _stonePrefabRef;
        
        [Button]
        private void SpawnLevelButtons()
        {
            // for (int i = 0; i < mapStonesParent.childCount; i++)
            // {
            //     DestroyImmediate(mapStonesParent.GetChild(i));
            // }

            _buttonPrefabRef = GameConfig.Instance.Levels.levelButtonUI.ButtonPrefab;
            _stonePrefabRef = GameConfig.Instance.Levels.levelButtonUI.StonePrefab;
            
            levelButtons.Clear();
            stones.Clear();
            
            for (int i = 0; i < numberOfLevels; i++)
            {
                _dummyPrefab = Instantiate(_buttonPrefabRef, mapStonesParent);
                _dummyLevelButton = _dummyPrefab.transform.GetComponent<UILevelButtonController>();
                
                levelButtons.Add(_dummyLevelButton);
                
                for (int j = 0; j < stonesPerLevel; j++)
                {
                    _dummyPrefab = Instantiate(_stonePrefabRef, mapStonesParent);
                    _dummyStoneImage = _dummyPrefab.GetComponent<Image>();
                    
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
    }
}
