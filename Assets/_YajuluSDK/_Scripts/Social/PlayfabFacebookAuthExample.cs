// Import statements introduce all the necessary classes for this example.

using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using LoginResult = PlayFab.ClientModels.LoginResult;

namespace _YajuluSDK._Scripts.Social
{
    public class PlayfabFacebookAuthExample : MonoBehaviour
    {
        // holds the latest message to be displayed on the screen
        private string _message;

        public static event Action<LoginResult> OnPlayerLoggedIn;
        public static event Action OnFbInitialized;

        public static event Action<PlayerProfileModel> OnPlayerProfileRecived;

        public void Awake()
        {
            SetMessage("Initializing Facebook..."); // logs the given message and displays it on the screen using OnGUI method
            if (!FB.IsInitialized) {
                // This call is required before any other calls to the Facebook API. We pass in the callback to be invoked once initialization is finished
                FB.Init(OnFacebookInitialized, OnHideUnity);
            } else {
                // Already initialized, signal an app activation App Event
                FB.ActivateApp();
            }
           
        }

        private void OnFacebookInitialized()
        {
            if (FB.IsInitialized) {
                // Signal an app activation App Event
                FB.ActivateApp();
                // Continue with Facebook SDK
                // ...
                SetMessage("Facebook Initialized.");
                OnFbInitialized?.Invoke();
                // Once Facebook SDK is initialized, if we are logged in, we log out to demonstrate the entire authentication cycle.
                // if (FB.IsLoggedIn)
                // {
                //     FB.LogOut();
                //     SetMessage("Logging Out.");
                // }
            } else {
                Debug.Log("Failed to Initialize the Facebook SDK");
            }
            
        }
        
        public void LoginWithFacebook()
        {
            SetMessage("Logging into Facebook...");
            //FB.Android.RetrieveLoginStatus(LoginStatusCallback);
            var perms = new List<string>(){"public_profile", "email"};
            // We invoke basic login procedure and pass in the callback to process the result
            FB.LogInWithReadPermissions(perms, OnFacebookLoggedIn);
        }
        
        private void OnHideUnity (bool isGameShown)
        {
            if (!isGameShown) {
                // Pause the game - we will need to hide
                Time.timeScale = 0;
            } else {
                // Resume the game - we're getting focus again
                Time.timeScale = 1;
            }
        }


        private void OnFacebookLoggedIn(ILoginResult result)
        {
            // If result has no errors, it means we have authenticated in Facebook successfully
            if (result == null || string.IsNullOrEmpty(result.Error))
            {
                SetMessage($"Facebook Auth Complete! Access Token: " + AccessToken.CurrentAccessToken.TokenString + $"\nLogging into PlayFab...  {AccessToken.CurrentAccessToken.UserId}");

                // foreach (var per in AccessToken.CurrentAccessToken.Permissions)
                // {
                //     Debug.Log(per);
                // }
                //
                /*
             * We proceed with making a call to PlayFab API. We pass in current Facebook AccessToken and let it create
             * and account using CreateAccount flag set to true. We also pass the callback for Success and Failure results
             */
                PlayFabClientAPI.LoginWithFacebook(new LoginWithFacebookRequest { CreateAccount = true, AccessToken = AccessToken.CurrentAccessToken.TokenString, 
                        InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                    {
                        GetPlayerProfile = true
                    }},
                    OnPlayfabFacebookAuthComplete, OnPlayfabFacebookAuthFailed);
                
            }
            else
            {
                // If Facebook authentication failed, we stop the cycle with the message
                SetMessage("Facebook Auth Failed: " + result.Error + "\n" + result.RawResult, true);
            }
        }

        // When processing both results, we just set the message, explaining what's going on.
        private void OnPlayfabFacebookAuthComplete(LoginResult result)
        {
            SetMessage("PlayFab Facebook Auth Complete. Session ticket: " + result.SessionTicket);
            Debug.Log($"PlayFab ID: {result.PlayFabId}");
            OnPlayerLoggedIn?.Invoke(result);
        }

        
        public static void UpdatePlayerDisplayName(string displayName)
        {
            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = displayName
            };
            //TODO: Update Callbacks
            PlayFabClientAPI.UpdateUserTitleDisplayName(request,
                nameResult =>
                {
                    Debug.Log($"Name Updated: {nameResult.DisplayName}");
                    GetPlayerData(null);
                },
                error => Debug.LogError(error.GenerateErrorReport()));
        }
        
        public static void GetPlayerData(string playFabID)
        {
            var request = new GetPlayerProfileRequest
            {
                PlayFabId = playFabID
            };
            PlayFabClientAPI.GetPlayerProfile(request, OnPlayerDataReceived, OnPlayerProfileRequestError);
        }

        private static void OnPlayerProfileRequestError(PlayFabError obj)
        {
            Debug.LogError(obj.GenerateErrorReport());
            
        }

        private static void OnPlayerDataReceived(GetPlayerProfileResult obj)
        {
            Debug.Log(obj.PlayerProfile.ToString());
            OnPlayerProfileRecived?.Invoke(obj.PlayerProfile);
        }

        private void OnPlayfabFacebookAuthFailed(PlayFabError error)
        {
            SetMessage("PlayFab Facebook Auth Failed: " + error.GenerateErrorReport(), true);
        }

        public void SetMessage(string message, bool error = false)
        {
            _message = message;
            if (error)
                Debug.LogError(_message);
            else
                Debug.Log(_message);
        }

        // public void OnGUI()
        // {
        //     var style = new GUIStyle { fontSize = 40, normal = new GUIStyleState { textColor = Color.white }, alignment = TextAnchor.MiddleCenter, wordWrap = true };
        //     var area = new Rect(0,0,Screen.width,Screen.height);
        //     GUI.Label(area, _message,style);
        // }
    }
}