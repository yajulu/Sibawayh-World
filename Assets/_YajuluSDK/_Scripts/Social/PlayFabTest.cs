using System;
using Facebook.Unity;
using PlayFab;
using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
			isInitialized = FB.IsInitialized;
			isLoggedIn = FB.IsLoggedIn;
			
			loginPanel.SetActive(false);
			profilePanel.SetActive(false);
			
			if (isInitialized)
			{
				if(isLoggedIn)
					loginPanel.SetActive(true);
			}
			else
			{
				PlayfabFacebookAuthExample.OnFbInitialized += OnFBInitialized;
			}
			PlayfabFacebookAuthExample.OnPlayerLoggedIn += OnPlayerLogin;
			PlayfabFacebookAuthExample.OnPlayerProfileRecived += OnPlayerProfileReceived;
			
		}

		private void OnPlayerProfileReceived(PlayerProfileModel obj)
		{
			displayName.SetText(obj.DisplayName);
		}

		private void OnDisable()
		{
			displayNameUpdateButton.onClick.RemoveAllListeners();
			PlayfabFacebookAuthExample.OnPlayerLoggedIn -= OnPlayerLogin;
			PlayfabFacebookAuthExample.OnFbInitialized -= OnFBInitialized;
		}

		private void OnFBInitialized()
		{
			loginPanel.SetActive(true);
			isInitialized = true;
		}

		private void OnPlayerLogin(LoginResult obj)
		{
			if (isLoggedIn)
				return;
			loginPanel.SetActive(false);
			profilePanel.SetActive(true);
			isLoggedIn = true;
			PlayfabFacebookAuthExample.GetPlayerData(null);
		}

		private void UpdatePlayerName()
		{
			PlayfabFacebookAuthExample.UpdatePlayerDisplayName(displayNameInputField.text);
		}
		
	}

}
