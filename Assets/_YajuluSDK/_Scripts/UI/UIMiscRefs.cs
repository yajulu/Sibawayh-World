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

        [Button]
        private void SetRefs()
        {
            _miscParent = transform.FindDeepChild<Transform>("Misc");
            _statsPanelController = _miscParent.GetComponentInChildren<UIStatsPanelController>();
        }
    }
}
