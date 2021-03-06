using System;
using _YajuluSDK._Scripts.Essentials;
using PROJECT.Scripts.UI;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace _YajuluSDK._Scripts.UI
{
    public class UIMiscRefs : MonoBehaviour
    {

        [SerializeField] private Transform _miscParent;
        [SerializeField] private UIStatsPanelController _statsPanelController;

        public UIStatsPanelController StatsPanelController => _statsPanelController;

        private void OnEnable()
        {
            CloseAllObjects();
        }

        private void CloseAllObjects()
        {
            for (int i = 0; i < _miscParent.childCount; i++)
            {
                _miscParent.GetChild(i).gameObject.SetActive(false);
            }
        }

        [Button]
        private void SetRefs()
        {
            _miscParent = transform.FindDeepChild<Transform>("Misc");
            _statsPanelController = _miscParent.transform.GetComponentInChildren<UIStatsPanelController>(true);
        }
    }
}
