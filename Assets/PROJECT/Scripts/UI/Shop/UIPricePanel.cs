using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.GameConfig;
using RTLTMPro;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PROJECT.Scripts.UI.Shop
{
    public class UIPricePanel : UIBehaviour
    {
        [SerializeField] private Image currencyIcon;
        [SerializeField] private RTLTextMeshPro currencyPrice;


        public void SetCurrencyPrice(string currency, uint price)
        {
            currencyIcon.sprite = GameConfig.Instance.Shop.VirtualCurrenciesDict[currency];
            currencyPrice.text = price.ToString();
        }
        

        [Button]
        private void SetRefs()
        {
            currencyIcon = transform.FindDeepChild<Image>("Currency_Image");
            currencyPrice = transform.FindDeepChild<RTLTextMeshPro>("Currency_Text");
        }
    }
}
