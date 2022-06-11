using System;
using System.Collections.Generic;
using System.Linq;
using _YajuluSDK._Scripts.GameConfig;
using _YajuluSDK._Scripts.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace PROJECT.Scripts.Game
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField, TitleGroup("Properties")]
        private LayerMask _mask;
        [SerializeField, TitleGroup("Refs")] private RectTransform parent;
        
        [SerializeField] private MapElement[] mapElements;

        private GameObject _mapElementPrefabRef;

        private List<MapElement> _forcedIndex = new List<MapElement>();
        private List<MapElement> _normalElements = new List<MapElement>();

        private List<MapElementCreator> _creatorElements = new List<MapElementCreator>();
        
        private Image _dummyImage;
        private GameObject _dummyGameObject;
        private MapElementCreator _currentCreator;
        
        [Button]
        private void GenerateMap()
        {
            ClearMap();
            // cam = Camera.main;
            // _current = FindObjectOfType<EventSystem>();
            _mapElementPrefabRef = GameConfig.Instance.Levels.levelButtonUI.MapElementPrefab;
            _forcedIndex.Clear();
            _normalElements.Clear();
            var parentRect = parent.rect;
            
            _creatorElements = new List<MapElementCreator>();
            // _currentCreatorElements = new List<MapElementCreator>();
            
            
            foreach (var mapElement in mapElements)
            {
                if (mapElement.forceLayerIndex)
                {
                    _forcedIndex.Add(mapElement);
                    _dummyGameObject = InstantiateParent();
                    _dummyGameObject.name = $"{mapElement.name}_Parent";
                }
                else
                {
                    _normalElements.Add(mapElement);
                }
            }
            
            _dummyGameObject = InstantiateParent();
            _dummyGameObject.name = $"NormalElements_Parent";
            // _forcedIndex.Sort();
            
            for (int i = 0; i < _forcedIndex.Count; i++)
            {
                PlaceElement(_forcedIndex[i], (RectTransform)parent.GetChild(i), false);
            }
            
            // foreach (var element in _normalElements)
            // {
            //     PlaceElement(element, (RectTransform)parent.GetChild(_forcedIndex.Count), true);
            // }
            
            RectTransform currentNormalItemsParent = (RectTransform)parent.GetChild(_forcedIndex.Count);
            
            foreach (var element in _normalElements)
            {
                _creatorElements.Add(new MapElementCreator(element, currentNormalItemsParent, _mapElementPrefabRef));
            }
            
            bool check = true;
            while (check)
            {
                check = false;
                _currentCreator = null;
                
                // _currentCreatorElements.Clear();
                
                float maxValue = float.MinValue;
                foreach (var creator in _creatorElements)
                {
                    check = creator.CreateNextObject() || check;
                    // _currentCreatorElements.Add(creator);
                    
                    if (creator.CurrentGameObject != null && creator.CurrentGameObject.transform.position.y > maxValue)
                    {
                        maxValue = creator.CurrentGameObject.transform.position.y;
                        _currentCreator = creator;
                    }
                    
                }
                
                if (_currentCreator != null)
                {
                    // _currentCreatorElements.Remove(_currentCreator);
                    if (_currentCreator.CurrentGameObject != null)
                    {
                        _currentCreator.CurrentGameObject.transform.SetParent(currentNormalItemsParent);
                        _currentCreator.CurrentGameObject = null;    
                    }
                }
                
                // for (int i = 0; i < _creatorElements.Count; i++)
                // {
                //     
                //     foreach (var creator in _currentCreatorElements)
                //     {
                //         if (creator.CurrentGameObject != null && creator.CurrentGameObject.transform.position.y > maxValue)
                //         {
                //             maxValue = creator.CurrentGameObject.transform.position.y;
                //             _currentCreator = creator;
                //         }
                //     }
                //     
                //     if (_currentCreator != null)
                //     {
                //         _currentCreatorElements.Remove(_currentCreator);
                //         if (_currentCreator.CurrentGameObject != null)
                //         {
                //             _currentCreator.CurrentGameObject.transform.SetParent(parent);
                //             _currentCreator.CurrentGameObject = null;    
                //         }
                //     }
                // }

            }
            
            //Sorting
            // _orderedChildren = new GameObject[parent.childCount];
            for (int i = 0; i < currentNormalItemsParent.childCount; i++)
            {
                var minValue = float.PositiveInfinity;
                var minChild = currentNormalItemsParent.GetChild(0);
                for (int j = i; j < currentNormalItemsParent.childCount; j++)
                {
                    var child = currentNormalItemsParent.GetChild(j);
                    if (child.transform.localPosition.y < minValue)
                    {
                        minValue = child.transform.localPosition.y;
                        minChild = child;
                    }
                }
                minChild.SetSiblingIndex(0);
            }
            


            void PlaceElement(MapElement element, RectTransform elementsParent, bool randomiseSibling)
            {
                var spacing = new Vector2(
                    parentRect.width / element.numberOfItems, 
                    parentRect.height / element.numberOfItems);

                // var currentPlace = Vector2.zero;
                for (int i = 0; i < element.numberOfItems; i++)
                {
                    _dummyImage = Instantiate(_mapElementPrefabRef, elementsParent).GetComponent<Image>();
                    var newPosition = _dummyImage.rectTransform.localPosition;
                    
                    newPosition.x = (Random.Range(-100, 100) > 0 ? 1 : -1) * Random.Range(element.horizontalRange.x, element.horizontalRange.y);
                    newPosition.y = (i * spacing.y + ((Random.Range(-100, 100) > 0 ? 1 : -1) * Random.Range(element.verticalRange.x, element.verticalRange.y))) % parentRect.height;
                    
                    var currentSprite = element.sprites[Random.Range(0, element.sprites.Length)];
                    
                    _dummyImage.sprite = currentSprite;
                    _dummyImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentSprite.rect.width * element.scale);
                    _dummyImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, currentSprite.rect.height * element.scale);
                    _dummyImage.rectTransform.pivot = currentSprite.pivot / currentSprite.rect.size;
                    _dummyImage.rectTransform.localPosition = newPosition;
                    // _dummyImage.gameObject.name = $"{element.name}_({newPosition.x},{newPosition.y})";
                    _dummyImage.gameObject.name = element.name;
                    
                    // var ray = cam.ScreenPointToRay(_dummyImage.rectTransform.position);
                    // PointerEventData data = new PointerEventData(_current);
                    // data.position = ray.origin;
                    // _results = new List<RaycastResult>();
                    // // UIScreenManager.Instance.rayCaster.blockingMask = _mask;
                    // UIScreenManager.Instance.rayCaster.Raycast(data, _results);
                    //
                    // if (_results != null && _results.Count > 0)
                    // {
                    //     Debug.Log("Hello");    
                    // }
                    
                    if (randomiseSibling)
                    {
                        elementsParent.SetSiblingIndex(Random.Range(0,elementsParent.childCount));
                    }
                    // currentPlace = newPosition;
                }
            }

            GameObject InstantiateParent()
            {
                var child = (RectTransform)Instantiate(_mapElementPrefabRef, parent).transform;
                child.anchorMax = Vector2.one;
                child.anchorMin = Vector2.zero;
                child.offsetMax = Vector2.zero;
                child.offsetMin = Vector2.zero;
                child.pivot = new Vector2(0.5f, 0);
                DestroyImmediate(child.gameObject.GetComponent<Image>());
                return child.gameObject;
            }
        }

        [Button]
        private void ClearMap()
        {
            var childCount = parent.childCount;
            for (int i = 0; i < childCount; i++)
            {
                DestroyImmediate(parent.GetChild(0).gameObject);
            }
        }

        [Serializable]
        public class MapElement : IComparer<MapElement>
        {
            public string name;
            public int layerIndex;
            public bool forceLayerIndex;
            public Vector2 horizontalRange;
            public Vector2 verticalRange;
            public int numberOfItems;
            [MinValue(0)] public float scale = 1f; 
            public Sprite[] sprites;

            public int Compare(MapElement x, MapElement y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(null, y)) return 1;
                if (ReferenceEquals(null, x)) return -1;
                // var nameComparison = string.Compare(x.name, y.name, StringComparison.Ordinal);
                // if (nameComparison != 0) return nameComparison;
                // var forceLayerIndexComparison = x.forceLayerIndex.CompareTo(y.forceLayerIndex);
                // if (forceLayerIndexComparison != 0) return forceLayerIndexComparison;
                var layerIndexComparison = x.layerIndex.CompareTo(y.layerIndex);
                return layerIndexComparison;
            }
        }

        [Serializable]
        public class MapElementCreator
        {
            private MapElement _mapElement;
            private int _currentIndex;
            private Vector2 _spacing;
            public GameObject CurrentGameObject;

            public MapElement MapElement => _mapElement;

            public int CurrentIndex => _currentIndex;

            public Vector2 Spacing => _spacing;

            private RectTransform _parentRect;
            private GameObject _mapElementPrefabRef;
            private Image _image;
            
            public MapElementCreator(MapElement mapElement, RectTransform parentRect, GameObject mapElementPrefabRef)
            {
                _mapElement = mapElement;
                _currentIndex = mapElement.numberOfItems - 2;
                _parentRect = parentRect;
                _mapElementPrefabRef = mapElementPrefabRef;
                _spacing = new Vector2(
                    _parentRect.rect.width / _mapElement.numberOfItems, 
                    _parentRect.rect.height / _mapElement.numberOfItems);

            }

            public bool CreateNextObject()
            {
                if (_currentIndex <= 0)
                {
                    _image = null;
                    return false;    
                }
                
                _image = Instantiate(_mapElementPrefabRef, _parentRect.gameObject.transform).GetComponent<Image>();
                var newPosition = _image.rectTransform.localPosition;
                    
                newPosition.x = (Random.Range(-100, 100) > 0 ? 1 : -1) * Random.Range(_mapElement.horizontalRange.x, _mapElement.horizontalRange.y);
                newPosition.y = ((_currentIndex) * _spacing.y + ((Random.Range(-100, 100) > 0 ? 1 : -1) * Random.Range(_mapElement.verticalRange.x, _mapElement.verticalRange.y)));
                    
                var currentSprite = _mapElement.sprites[Random.Range(0, _mapElement.sprites.Length)];
                    
                _image.sprite = currentSprite;
                _image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentSprite.rect.width * _mapElement.scale);
                _image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, currentSprite.rect.height * _mapElement.scale);
                _image.rectTransform.pivot = Vector2.right * 0.5f;
                _image.rectTransform.localPosition = newPosition;
                // _dummyImage.gameObject.name = $"{element.name}_({newPosition.x},{newPosition.y})";
                _image.gameObject.name = _mapElement.name;
                // _image.gameObject.transform.SetParent(null);
                CurrentGameObject = _image.gameObject;
                _currentIndex--;
                return true;
            }
            
        }
    }
}
