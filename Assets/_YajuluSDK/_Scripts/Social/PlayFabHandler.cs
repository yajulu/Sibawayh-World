using System;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.UI;
using Facebook.Unity;
using PlayFab;
using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;
using Newtonsoft.Json;
using PROJECT.Scripts.UI.Screens;
using LoginResult = PlayFab.ClientModels.LoginResult;

namespace _YajuluSDK._Scripts.Social
{
	public class PlayFabHandler : Singleton<PlayFabHandler>
	{
		[TitleGroup("Debug")] 
		[ReadOnly, SerializeField] private bool isLoggedIn;
		[ReadOnly, SerializeField] private bool isInitialized;

		private PlayerProfileModel _cachedPlayer;

		public PlayerProfileModel CachedPlayer => _cachedPlayer;

		// [TitleGroup("Refs")]
		// [SerializeField] private GameObject loginPanel;
		// [SerializeField] private GameObject profilePanel;
		// [SerializeField] private TextMeshProUGUI displayName;
		// [SerializeField] private TMP_InputField displayNameInputField;
		// [SerializeField] private Button displayNameUpdateButton;
		
		// [SerializeField] private TextMeshProUGUI scoreTest;
		// [SerializeField] private TMP_InputField scoreInputField;
		// [SerializeField] private Button scoreUpdateButton;
		// [SerializeField] private Button leaderboardButton;

		private void OnEnable()
		{
			// displayNameUpdateButton.onClick.AddListener(UpdatePlayerName);
			// scoreUpdateButton.onClick.AddListener(UpdatePlayerScore);
			// leaderboardButton.onClick.AddListener(ShowLeaderboard);
			//
			// loginPanel.SetActive(true);
			// profilePanel.SetActive(false);
			
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

		private void Start()
		{
			PlayfabManager.PlayfabLoginWithDeviceID();
		}

		private void PlayerLoggedIn()
		{
			UIScreenManager.Instance.CloseScreen(nameof(Panel_Loading));
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
				// loginPanel.SetActive(false);
				PlayerLoggedIn();
			}
			else
			{
				// loginPanel.SetActive(true);
			}
			
		}


		private void OnPlayerProfileReceived(PlayerProfileModel obj)
		{
			PlayerLoggedIn();
			_cachedPlayer = obj;
			// displayName.SetText(obj.DisplayName);
			// scoreTest.SetText(obj.Statistics[0].Value.ToString());


			// PlayfabManager.GetPlayerStatistics(null);
		}

		private void OnDisable()
		{
			// displayNameUpdateButton.onClick.RemoveAllListeners();
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
			// profilePanel.SetActive(true);
			isLoggedIn = true;
			PlayfabManager.GetPlayerData(null);
		}

		private void SetPlayerData()
		{
			
		}
		

		private void UpdatePlayerName()
		{
			// PlayfabManager.UpdatePlayerDisplayName(displayNameInputField.text);
		}
		
		private void UpdatePlayerScore()
		{
			// PlayfabManager.UpdateLeaderBoard("MaxScore", int.Parse(scoreInputField.text));
		}

		private void ShowLeaderboard()
		{
			// PlayfabManager.GetLeaderBoardData("MaxScore", 0);
		}
		
		
	}

}
