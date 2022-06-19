using System;
using System.Collections.Generic;
using System.Linq;
using _YajuluSDK._Scripts.GameConfig;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PROJECT.Scripts.Game.Map
{
    public class MapGenerator2D : MonoBehaviour
    {
        [SerializeField, TitleGroup("Properties")]
        private LayerMask _mask;
        [SerializeField, TitleGroup("Refs")] private Transform parent;
        
        [SerializeField] private MapElement[] mapElements;

        private GameObject _mapElementPrefabRef;

        private List<MapElement> _forcedIndex = new List<MapElement>();
        private List<MapElement> _normalElements = new List<MapElement>();
        
        
        private SpriteRenderer _dummySpriteRenderer;
        private GameObject _dummyGameObject;

        [Button]
        private void GenerateMap()
        {
            ClearMap();
            // cam = Camera.main;
            // _current = FindObjectOfType<EventSystem>();
            _mapElementPrefabRef = GameConfig.Instance.Levels.levelButtonUI.MapElement2DPrefab;
            _forcedIndex.Clear();
            _normalElements.Clear();
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
                PlaceElement(_forcedIndex[i], parent.GetChild(i));
            }
            
            foreach (var element in _normalElements)
            {
                PlaceElement(element, parent.GetChild(_forcedIndex.Count));
            }

            var currentNormalItemsParent = parent.GetChild(parent.childCount - 1);
            
            //Sorting
            // _orderedChildren = new GameObject[parent.childCount];
            var elements = currentNormalItemsParent.GetComponentsInChildren<SpriteRenderer>(true);
            List<SpriteRenderer> elementsList = new List<SpriteRenderer>();
            elementsList.AddRange(elements);

            elementsList.Sort((spriteRenderer, renderer1) =>
                spriteRenderer.transform.localPosition.y > renderer1.transform.localPosition.y ? -1 : 1
            );
            
            for (int i = 0; i < elementsList.Count; i++)
            {
                var element = elementsList[i];
                element.sortingOrder = i;
                element.sortingLayerName = "Background";
            }
            
            // for (int i = 0; i < elements.Length; i++)
            // {
            //     
            //     var minValue = float.PositiveInfinity;
            //     var minChild = currentNormalItemsParent.GetChild(0);
            //     for (int j = i; j < currentNormalItemsParent.childCount; j++)
            //     {
            //         var child = currentNormalItemsParent.GetChild(j);
            //         if (child.transform.localPosition.y < minValue)
            //         {
            //             minValue = child.transform.localPosition.y;
            //             minChild = child;
            //         }
            //     }
            //     minChild.SetSiblingIndex(0);
            // }
            


            void PlaceElement(MapElement element, Transform elementsParent)
            {
                var spacing = new Vector2(
                    (Mathf.Abs(element.horizontalRange.x) + Mathf.Abs(element.horizontalRange.y)) * 0.5f, 
                    (Mathf.Abs(element.verticalRange.x) + Mathf.Abs(element.verticalRange.y)) * 0.5f);
                // var currentPlace = Vector2.zero;
                for (int i = 0; i < element.numberOfItems; i++)
                {
                    _dummySpriteRenderer = Instantiate(_mapElementPrefabRef, elementsParent).GetComponent<SpriteRenderer>();
                    var newPosition = _dummySpriteRenderer.transform.localPosition;
                    
                    newPosition.x = (Random.Range(-100, 100) > 0 ? 1 : -1) * Random.Range(element.horizontalRange.x, element.horizontalRange.y);
                    newPosition.y = (i * spacing.y + ((Random.Range(-100, 100) > 0 ? 1 : -1) * Random.Range(element.verticalRange.x, element.verticalRange.y)));
                    
                    var currentSprite = element.sprites[Random.Range(0, element.sprites.Length)];
                    
                    _dummySpriteRenderer.sprite = currentSprite;
                    _dummySpriteRenderer.transform.localPosition = newPosition;
                    // _dummyImage.gameObject.name = $"{element.name}_({newPosition.x},{newPosition.y})";
                    _dummySpriteRenderer.gameObject.name = element.name;
                    
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
                }
            }

            GameObject InstantiateParent()
            {
                var child = Instantiate(_mapElementPrefabRef, parent).transform;
                child.localPosition = Vector3.zero;
                DestroyImmediate(child.gameObject.GetComponent<SpriteRenderer>());
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

        // [Serializable]
        // public class MapElementCreator
        // {
        //     private MapElement _mapElement;
        //     private int _currentIndex;
        //     private Vector2 _spacing;
        //     public GameObject CurrentGameObject;
        //
        //     public MapElement MapElement => _mapElement;
        //
        //     public int CurrentIndex => _currentIndex;
        //
        //     public Vector2 Spacing => _spacing;
        //
        //     private Transform _parentRect;
        //     private GameObject _mapElementPrefabRef;
        //     private SpriteRenderer _spriteRenderer;
        //     
        //     public MapElementCreator(MapElement mapElement, Transform parentRect, GameObject mapElementPrefabRef)
        //     {
        //         _mapElement = mapElement;
        //         _currentIndex = mapElement.numberOfItems - 2;
        //         _parentRect = parentRect;
        //         _mapElementPrefabRef = mapElementPrefabRef;
        //         _spacing = new Vector2(
        //             (Mathf.Abs(mapElement.horizontalRange.x) + Mathf.Abs(mapElement.horizontalRange.y)) * 0.5f, 
        //             (Mathf.Abs(mapElement.verticalRange.x) + Mathf.Abs(mapElement.verticalRange.y)) * 0.5f);
        //
        //     }
        //
        //     public bool CreateNextObject()
        //     {
        //         if (_currentIndex <= 0)
        //         {
        //             _spriteRenderer = null;
        //             return false;    
        //         }
        //         
        //         _spriteRenderer = Instantiate(_mapElementPrefabRef, _parentRect.gameObject.transform).GetComponent<SpriteRenderer>();
        //         var newPosition = _spriteRenderer.transform.localPosition;
        //             
        //         newPosition.x = (Random.Range(-100, 100) > 0 ? 1 : -1) * Random.Range(_mapElement.horizontalRange.x, _mapElement.horizontalRange.y);
        //         newPosition.y = ((_currentIndex) * _spacing.y + ((Random.Range(-100, 100) > 0 ? 1 : -1) * Random.Range(_mapElement.verticalRange.x, _mapElement.verticalRange.y)));
        //             
        //         var currentSprite = _mapElement.sprites[Random.Range(0, _mapElement.sprites.Length)];
        //             
        //         _spriteRenderer.sprite = currentSprite;
        //         var spriteRendererTransform = _spriteRenderer.transform;
        //         spriteRendererTransform.localScale *= _mapElement.scale;
        //         spriteRendererTransform.localPosition = newPosition;
        //         
        //         _spriteRenderer.gameObject.name = _mapElement.name;
        //         // _image.gameObject.transform.SetParent(null);
        //         CurrentGameObject = _spriteRenderer.gameObject;
        //         _currentIndex--;
        //         return true;
        //     }
        //     
        // }
    }
}
