using System;
using System.Collections;
using System.Collections.Generic;
using EasyMobile;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Purchasing;

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
			InAppPurchasing.RegisterPrePurchaseProcessDelegate(PlayFabReceiptValidation);
		}

		private InAppPurchasing.PrePurchaseProcessResult PlayFabReceiptValidation(PurchaseEventArgs args)
		{
#if UNITY_ANDROID
			// Deserialize receipt
			
			var googleReceipt = GooglePurchase.FromJson(args.purchasedProduct.receipt);
			PlayFabClientAPI.ValidateGooglePlayPurchase(new ValidateGooglePlayPurchaseRequest
				{
					// Pass in currency code in ISO format
					CurrencyCode = args.purchasedProduct.metadata.isoCurrencyCode,
					// Convert and set Purchase price
					PurchasePrice = (uint)(args.purchasedProduct.metadata.localizedPrice * 100),
					// Pass in the receipt
					ReceiptJson = googleReceipt.PayloadData.json,
					// Pass in the signature
					Signature = googleReceipt.PayloadData.signature
				}, result =>
				{
					Debug.Log("Validation successful!");
					InAppPurchasing.ConfirmPendingPurchase(args.purchasedProduct, true);
				},
				error =>
				{
					Debug.Log("Validation failed: " + error.GenerateErrorReport());
					InAppPurchasing.ConfirmPendingPurchase(args.purchasedProduct, false);
				});
			return InAppPurchasing.PrePurchaseProcessResult.Suspend;
#elif UNITY_IOS
			PlayFabClientAPI.ValidateIOSReceipt(new ValidateIOSReceiptRequest
			{
				ReceiptData = args.purchasedProduct.receipt,
				CurrencyCode = args.purchasedProduct.metadata.isoCurrencyCode,
				PurchasePrice = (int) (args.purchasedProduct.metadata.localizedPrice * 100)
			}, result =>
			{
				Debug.Log("Validation successful!");
				InAppPurchasing.ConfirmPendingPurchase(args.purchasedProduct, true);
			}, error =>
			{
				Debug.Log("Validation failed: " + error.GenerateErrorReport());
				InAppPurchasing.ConfirmPendingPurchase(args.purchasedProduct, false);
			});
			return InAppPurchasing.PrePurchaseProcessResult.Suspend;
#endif
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
					Debug.Log(product.Name + " was purchased with " + product.Price + ". The user should be granted it now.");
					break;
			}
		}

		private void PurchasingFailed(IAPProduct product, string error)
		{
			NativeUI.Alert("Error", "The purchase of product " + product.Name + " has failed with reason: " + error);
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
	public class JsonData {
		// JSON Fields, ! Case-sensitive

		public string orderId;
		public string packageName;
		public string productId;
		public long purchaseTime;
		public int purchaseState;
		public string purchaseToken;
	}

	public class PayloadData {
		public JsonData JsonData;

		// JSON Fields, ! Case-sensitive
		public string signature;
		public string json;

		public static PayloadData FromJson(string json) {
			var payload = JsonUtility.FromJson<PayloadData>(json);
			payload.JsonData = JsonUtility.FromJson<JsonData>(payload.json);
			return payload;
		}
	}

	public class GooglePurchase {
		public PayloadData PayloadData;

		// JSON Fields, ! Case-sensitive
		public string Store;
		public string TransactionID;
		public string Payload;

		public static GooglePurchase FromJson(string json) {
			var purchase = JsonUtility.FromJson<GooglePurchase>(json);
			purchase.PayloadData = PayloadData.FromJson(purchase.Payload);
			return purchase;
		}
	}
}