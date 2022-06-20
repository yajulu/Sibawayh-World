using System;
using System.Collections.Generic;
using EasyMobile;
using Facebook.Unity;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using GS = EasyMobile.GameServices;
using LoginResult = PlayFab.ClientModels.LoginResult;

namespace _YajuluSDK._Scripts.Social
{
    public class PlayfabManager : MonoBehaviour
    {
        // holds the latest message to be displayed on the screen
        private string _message;

        public static event Action<LoginResult> OnPlayerLoggedIn;
        public static event Action OnFbInitialized;
        public static event Action<ILoginStatusResult> OnFacebookLoginStatusRetrieved;
        public static event Action<PlayerProfileModel> OnPlayerProfileReceived;
        
        public void Awake()
        {
            FacebookInitialization();
            GameServicesInitialization();
            //Intializing Easy Mobile
            if (!RuntimeManager.IsInitialized())
                RuntimeManager.Init();
        }

        private void OnEnable()
        {
            GS.UserLoginSucceeded += OnGameServicesLogInSucceeded;
            GS.UserLoginFailed += OnGameServicesLogInFailed;
            PlayFabClientAPI.;
        }

        private void OnDisable()
        {
            GS.UserLoginSucceeded -= OnGameServicesLogInSucceeded;
            GS.UserLoginFailed -= OnGameServicesLogInFailed;
        }

        private void FacebookInitialization()
        {
            SetMessage("Initializing Facebook..."); // logs the given message and displays it on the screen using OnGUI method
            if (!FB.IsInitialized)
            {
                // This call is required before any other calls to the Facebook API. We pass in the callback to be invoked once initialization is finished
                FB.Init(OnFacebookInitialized, OnHideUnity);
            }
            else
            {
                // Already initialized, signal an app activation App Event
                FB.ActivateApp();
            }

        }

        public void SilentLogin()
        {
            PlayLoginWithDeviceID();
        }

        public void GameServicesInitialization()
        {
#if UNITY_ANDROID
            Debug.Log($"Game Services: {GS.IsInitialized()}");
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
                .AddOauthScope("profile")
                .RequestServerAuthCode(false)
                .Build();
            PlayGamesPlatform.InitializeInstance(config);

            PlayGamesPlatform.DebugLogEnabled = true;

            PlayGamesPlatform.Activate();

            // if (GS.IsInitialized())
            // {
            // GS.ManagedInit();
            // }
#endif
        }

        private void OnGameServicesLogInSucceeded()
        {
#if UNITY_ANDROID
            Debug.Log($"Game Services Username: {GS.LocalUser.userName}");
            GS.GetAnotherServerAuthCode(true,
                authCode =>
                {
                    Debug.Log($"Google Auth Code: {authCode}");
                    if (PlayFabClientAPI.IsClientLoggedIn())
                    {

                        var linkGoogleAccountRequest = new LinkGoogleAccountRequest
                        {
                            ServerAuthCode = authCode

                        };

                        PlayFabClientAPI.LinkGoogleAccount(linkGoogleAccountRequest, result =>
                        {
                            Debug.Log($"Google Account Linked Successfully.");
                        }, OnPlayfabAuthFailed);
                    }
                    else
                    {
                        Debug.Log($"Logging In with new Google Account.");
                        var req = new LoginWithGoogleAccountRequest
                        {
                            TitleId = PlayFabSettings.TitleId,
                            ServerAuthCode = authCode,
                            CreateAccount = true
                        };
                        PlayFabClientAPI.LoginWithGoogleAccount(req, OnPlayfabAuthComplete, OnPlayfabAuthFailed);
                    }
                });


#elif UNITY_IOS
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                
                var linkGameCenterAccount = new LinkGameCenterAccountRequest()
                {
                };
                
                PlayFabClientAPI.LinkGameCenterAccount(linkGameCenterAccount, result =>
                {
                    Debug.Log($"Google Account Linked Successfully.");
                }, OnPlayfabAuthFailed);
            }
            else
            {
                Debug.Log($"Logging In with new Google Account.");
                var req = new LoginWithGameCenterRequest()
                {
                    TitleId = PlayFabSettings.TitleId,
                    CreateAccount = true,
                    PlayerId = GS.LocalUser.id
                };
                PlayFabClientAPI.LoginWithGameCenter(req, OnPlayfabAuthComplete, OnPlayfabAuthFailed);
            }
#endif

        }

        public void FacebookLogout()
        {
            FB.LogOut();
        }

        private void OnGameServicesLogInFailed()
        {
            Debug.Log("Game Services Login Failed");
            PlayLoginWithDeviceID();
        }

        private void PlayLoginWithDeviceID()
        {
            Debug.Log("Logging in with device ID.");
#if UNITY_ANDROID
            var req = new LoginWithAndroidDeviceIDRequest
            {
                CreateAccount = true,
                AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
                AndroidDevice = SystemInfo.deviceModel,
            };
            PlayFabClientAPI.LoginWithAndroidDeviceID(req, OnPlayfabAuthComplete, OnPlayfabAuthFailed);
#elif UNITY_IOS
            var req = new LoginWithIOSDeviceIDRequest
            {
                CreateAccount = true,
                DeviceId = SystemInfo.deviceUniqueIdentifier,
                DeviceModel = SystemInfo.deviceModel,
                // TODO: InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                // {
                //     
                // }
            };
            PlayFabClientAPI.LoginWithIOSDeviceID(req, OnPlayfabAuthComplete, OnPlayfabAuthFailed);
#endif
        }

        private void OnFacebookInitialized()
        {
            if (FB.IsInitialized)
            {
                // Signal an app activation App Event
                FB.ActivateApp();
                // Continue with Facebook SDK
                // ...
                SetMessage("Facebook Initialized.");
                OnFbInitialized?.Invoke();
#if UNITY_ANDROID
                FB.Android.RetrieveLoginStatus(OnLoginStatusRetrieved);
#endif
                // Once Facebook SDK is initialized, if we are logged in, we log out to demonstrate the entire authentication cycle.
                // if (FB.IsLoggedIn)
                // {
                //     FB.LogOut();
                //     SetMessage("Logging Out.");
                // }
            }
            else
            {
                Debug.Log("Failed to Initialize the Facebook SDK");
            }

        }

        public void LoginWithFacebook()
        {
            SetMessage("Logging into Facebook...");
            //FB.Android.RetrieveLoginStatus(LoginStatusCallback);
            var perms = new List<string>() { "public_profile", "email" };
            // We invoke basic login procedure and pass in the callback to process the result
            FB.LogInWithReadPermissions(perms, OnFacebookLoggedIn);
        }

        public void LoginWithGameServices()
        {
            GS.ManagedInit();
        }

        private void OnLoginStatusRetrieved(ILoginStatusResult result)
        {
            Debug.Log($"Login Retrieved Failed: {result.Failed}");
            Debug.Log($"Login Retrieved Canceled: {result.Cancelled}");
            if (!result.Failed && !result.Cancelled)
            {
                OnFacebookLoggedIn(result);
            }

            OnFacebookLoginStatusRetrieved?.Invoke(result);
        }
        private void OnHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                // Pause the game - we will need to hide
                Time.timeScale = 0;
            }
            else
            {
                // Resume the game - we're getting focus again
                Time.timeScale = 1;
            }
        }

        private void PlayFabLoginWithGameServices()
        {

        }

        private static void OnError(PlayFabError playFabError)
        {
            Debug.LogError(playFabError.GenerateErrorReport());
        }
        
        



        public void TestRequestAuthCode()
        {
            GS.GetAnotherServerAuthCode(true, s =>
            {
                Debug.Log($"Auth Code: {s}");
            });
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
                if (PlayFabClientAPI.IsClientLoggedIn())
                {
                    var fbReq = new LinkFacebookAccountRequest
                    {
                        AccessToken = AccessToken.CurrentAccessToken.TokenString
                    };
                    PlayFabClientAPI.LinkFacebookAccount(fbReq,
                        accountResult =>
                    {
                        Debug.Log($"Facebook Account Linked Successfully.");
                    }, OnPlayfabAuthFailed);
                }
                else
                {
                    PlayFabClientAPI.LoginWithFacebook(new LoginWithFacebookRequest
                    {
                        CreateAccount = true,
                        AccessToken = AccessToken.CurrentAccessToken.TokenString,
                        InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                        {
                            GetPlayerProfile = true
                        }
                    },
                        OnPlayfabAuthComplete, OnPlayfabAuthFailed);
                }


            }
            else
            {
                // If Facebook authentication failed, we stop the cycle with the message
                SetMessage("Facebook Auth Failed: " + result.Error + "\n" + result.RawResult, true);
            }
        }

        // When processing both results, we just set the message, explaining what's going on.
        private void OnPlayfabAuthComplete(LoginResult result)
        {
            SetMessage("PlayFab Auth Complete. Session ticket: " + result.SessionTicket);
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
                PlayFabId = playFabID,
                ProfileConstraints = new PlayerProfileViewConstraints
                {
                    ShowStatistics = true,
                    ShowDisplayName = true
                }
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
            OnPlayerProfileReceived?.Invoke(obj.PlayerProfile);
        }

        private void OnPlayfabAuthFailed(PlayFabError error)
        {
            SetMessage("PlayFab Auth Failed: " + error.GenerateErrorReport(), true);
        }

        public void SetMessage(string message, bool error = false)
        {
            _message = message;
            if (error)
                Debug.LogError(_message);
            else
                Debug.Log(_message);
        }

        public void GameServices()
        {
            Debug.Log(EasyMobile.GameServices.LocalUser.userName);
        }

        #region Leaderboards

        public static void GetLeaderBoardData(string statisticName, int startPosition)
        {
            var request  = new GetLeaderboardRequest
            {
                StartPosition = startPosition,
                StatisticName = statisticName,
                MaxResultsCount = 10
            };
            
            PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardSuccess, OnError );
        }

        private static void OnGetLeaderboardSuccess(GetLeaderboardResult obj)
        {
            Debug.Log("---- LeaderBoard Start ----");
            foreach (var leaderboardEntry in obj.Leaderboard)
            {
                Debug.Log($"{leaderboardEntry.Position} - " +
                          $"{leaderboardEntry.PlayFabId} - " +
                          $"{leaderboardEntry.StatValue} - " +
                          $"{leaderboardEntry.DisplayName}");
            }
            Debug.Log("---- LeaderBoard End ----");
        }

        public static void UpdateLeaderBoard(string statistics, int value)
        {
            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate
                    {
                        StatisticName = statistics,
                        Value = value
                    }
                }
            };
            
            PlayFabClientAPI.UpdatePlayerStatistics(request, OnUpdatePlayerStatisticsSuccess, OnError);
        }

        private static void OnUpdatePlayerStatisticsSuccess(UpdatePlayerStatisticsResult obj)
        {
            Debug.Log($"Player Statistics Updated Successfully");
            //TODO: maybe update the leaderboard to reflect the change.
            GetPlayerData(null);
        }

        public static void GetPlayerStatistics(List<string> statisticNames)
        {
            GetPlayerStatisticsRequest request;
            if (statisticNames == null)
            {
                request = new GetPlayerStatisticsRequest();    
            }
            else
            {
                request = new GetPlayerStatisticsRequest
                {
                    StatisticNames = statisticNames
                };
            }
            
            PlayFabClientAPI.GetPlayerStatistics(request, OnGetPlayerStatisticsSuccess, OnError);
        }

        private static void OnGetPlayerStatisticsSuccess(GetPlayerStatisticsResult obj)
        {
            Debug.Log("---------- Statistics Start -------------");
            foreach (var statisticValue in obj.Statistics)
            {
                Debug.Log($"{statisticValue.StatisticName}: {statisticValue.Value}");
            }
            Debug.Log("---------- Statistics End -------------");
        }

        #endregion

        // public void OnGUI()
        // {
        //     var style = new GUIStyle { fontSize = 40, normal = new GUIStyleState { textColor = Color.white }, alignment = TextAnchor.MiddleCenter, wordWrap = true };
        //     var area = new Rect(0,0,Screen.width,Screen.height);
        //     GUI.Label(area, _message,style);
        // }


        //Should be moved to another script
        public void RateUs()
        {
            // Show the rating dialog with default behavior
            StoreReview.RequestRating();
        }
    }
}