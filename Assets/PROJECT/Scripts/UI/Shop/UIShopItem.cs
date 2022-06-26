using _YajuluSDK._Scripts.UI;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Shop
{
    public class UIShopItem : UIElementBase
    {
        [SerializeField] private Image icon;
        [SerializeField] private Button button;
        [SerializeField] private RTLTextMeshPro price;
        [SerializeField] private RectTransform pricePanel;
        [SerializeField] private string itemID;

    }
}
