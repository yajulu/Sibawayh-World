using System;
using Facebook.Unity;
using PlayFab;
using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;
using LoginResult = PlayFab.ClientModels.LoginResult;

namespace _YajuluSDK._Scripts.Social
{
	public class PlayFabTest : MonoBehaviour
	{
		[TitleGroup("Debug")] 
		[ReadOnly, SerializeField] private bool isLoggedIn;
		[ReadOnly, SerializeField] private bool isInitialized;
		[TitleGroup("Refs")]
		[SerializeField] private GameObject loginPanel;
		[SerializeField] private GameObject profilePanel;
		[SerializeField] private TextMeshProUGUI displayName;
		[SerializeField] private TMP_InputField displayNameInputField;
		[SerializeField] private Button displayNameUpdateButton;

		private void OnEnable()
		{
			displayNameUpdateButton.onClick.AddListener(UpdatePlayerName);

			loginPanel.SetActive(true);
			profilePanel.SetActive(false);
			
			PlayfabManager.OnPlayerLoggedIn += OnPlayerLogin;
			PlayfabManager.OnPlayerProfileReceived += OnPlayerProfileReceived;
			PlayfabManager.OnFacebookLoginStatusRetrieved += OnFBLoginStatusRetrieved;
			
			if (isInitialized)
			{
				OnFBInitialized();
			}
			else
			{
				PlayfabManager.OnFbInitialized += OnFBInitialized;
			}

			
		}

		private void OnFBLoginStatusRetrieved(ILoginStatusResult obj)
		{
			if (obj.Failed)
			{
				Debug.Log($"Login Status Retrieved Failed");
				Debug.LogError(obj.Error);
			}
			
			if (obj.Cancelled)
			{
				Debug.Log($"Login Status Retrieved Canceled");
			}

			isLoggedIn = FB.IsLoggedIn;
			
			Debug.Log($"OnFacebookLoginRetrieved -> LoggedIn: {isLoggedIn}");
			
			if (FB.IsLoggedIn)
			{
				loginPanel.SetActive(false);
				profilePanel.SetActive(true);
			}
			else
			{
				loginPanel.SetActive(true);
			}
			
		}


		private void OnPlayerProfileReceived(PlayerProfileModel obj)
		{
			displayName.SetText(obj.DisplayName);
		}

		private void OnDisable()
		{
			displayNameUpdateButton.onClick.RemoveAllListeners();
			PlayfabManager.OnPlayerLoggedIn -= OnPlayerLogin;
			PlayfabManager.OnFbInitialized -= OnFBInitialized;
		}

		private void OnFBInitialized()
		{
			isInitialized = true;
		}

		private void OnPlayerLogin(LoginResult obj)
		{
			if (isLoggedIn)
				return;
			// loginPanel.SetActive(false);
			profilePanel.SetActive(true);
			isLoggedIn = true;
			PlayfabManager.GetPlayerData(null);
		}

		private void UpdatePlayerName()
		{
			PlayfabManager.UpdatePlayerDisplayName(displayNameInputField.text);
		}
		
		
	}

}
