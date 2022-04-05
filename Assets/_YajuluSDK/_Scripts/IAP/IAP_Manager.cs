using System;
using System.Collections;
using System.Collections.Generic;
using EasyMobile;
using UnityEngine;

namespace _YajuluSDK._Scripts.IAP
{
public class IAP_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InAppPurchasing.InitializePurchasing();
        bool isInitialized = InAppPurchasing.IsInitialized();
    }

    void OnEnable()
    {
        InAppPurchasing.PurchaseCompleted += PurchasingSucceded;
        InAppPurchasing.PurchaseFailed += PurchasingFailed;
    }

    void OnDisable()
    {
        InAppPurchasing.PurchaseCompleted -= PurchasingSucceded;
        InAppPurchasing.PurchaseFailed -= PurchasingFailed;
    }

        

        private void PurchasingSucceded(IAPProduct product)
        {
            switch (product.Name)
            {
                case EM_IAPConstants.Product_Fake_Item_1:
                    Debug.Log(product.Name + " was purchased with " + product.Price +". The user should be granted it now.");
                break;
            }
    
            
        }
        private void PurchasingFailed(IAPProduct product, string error)
        {
            NativeUI.Alert("Error","The purchase of product " + product.Name + " has failed with reason: " + error);
        }

        // Update is called once per frame
        void Update()
    {
        
    }

    public void PurchaseItem()
    {
        InAppPurchasing.Purchase(EM_IAPConstants.Product_Fake_Item_1);

    }
}
}
