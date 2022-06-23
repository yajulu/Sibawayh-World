using PROJECT.Scripts.UI;
using Project.YajuluSDK.Scripts.Essentials;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.YajuluSDK.Scripts.UI
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
