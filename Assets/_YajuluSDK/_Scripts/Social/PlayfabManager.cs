using System;
using System.Collections.Generic;
using EasyMobile;
using Facebook.Unity;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.CloudScriptModels;
using Project.Scripts.Inventory;
using Sirenix.OdinInspector;
using UnityEngine;
using EntityKey = PlayFab.CloudScriptModels.EntityKey;
using GS = EasyMobile.GameServices;
using LoginResult = PlayFab.ClientModels.LoginResult;
using PlayerProfileModel = PlayFab.ClientModels.PlayerProfileModel;

namespace _YajuluSDK._Scripts.Social
{
    public class PlayfabManager : MonoBehaviour
    {
        // holds the latest message to be displayed on the screen

        private static class Constants
        {
            public static string EQUIP_ITEM_METHOD { get; private set; } = "EquipItem";
            public static string UPDATE_PLAYER_GAME_PROFILE { get; } = "UpdatePlayerGameProfile";
        }
        private string _message;

        public static event Action OnPlayerLoggedInBasic;
        public static event Action<LoginResult> OnPlayerLoggedIn;
        public static event Action OnFbInitialized;
        public static event Action<ILoginStatusResult> OnFacebookLoginStatusRetrieved;
        public static event Action<PlayerProfileModel> OnPlayerProfileReceived;

        public static event Action<GetPlayerCombinedInfoResultPayload> OnPlayerCombinedInfoLoaded;

        private static GetPlayerCombinedInfoResultPayload _playerCombinedInfo;
        public static GetPlayerCombinedInfoResultPayload PlayerCombinedInfo
        {
            get
            {
                if (_playerCombinedInfo == null)
                {
                    LoadPlayerDataCombined();
                    return null;
                }
                else
                {
                    return _playerCombinedInfo;
                }
            }
        }

        private static bool _playerCombinedRequestLock = false;
        
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
        }

        private void OnDisable()
        {
            GS.UserLoginSucceeded -= OnGameServicesLogInSucceeded;
            GS.UserLoginFailed -= OnGameServicesLogInFailed;
        }


        #region Facebook SDK
        
        private static void FacebookInitialization()
        {
            // SetMessage("Initializing Facebook..."); // logs the given message and displays it on the screen using OnGUI method
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
            
            static void OnFacebookInitialized()
            {
                if (FB.IsInitialized)
                {
                    // Signal an app activation App Event
                    FB.ActivateApp();
                    // Continue with Facebook SDK
                    // ...
                    // SetMessage("Facebook Initialized.");
                    OnFbInitialized?.Invoke();
#if UNITY_ANDROID
                    FB.Android.RetrieveLoginStatus(OnFacebookLoginStatusRetrievedHandler);
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
                
                static void OnFacebookLoginStatusRetrievedHandler(ILoginStatusResult result)
                {
                    Debug.Log($"Login Retrieved Failed: {result.Failed}");
                    Debug.Log($"Login Retrieved Canceled: {result.Cancelled}");
                    if (!result.Failed && !result.Cancelled)
                    {
                        OnFacebookLoggedIn(result);
                    }

                    OnFacebookLoginStatusRetrieved?.Invoke(result);
                }

            }
            
            static void OnHideUnity(bool isGameShown)
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
        }
        
        public static void FacebookLogout()
        {
            FB.LogOut();
        }
        
        #endregion
        
        #region GameServices
        
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
                        PlayFabClientAPI.LoginWithGoogleAccount(req, OnPlayFabLoginSucceeded, OnPlayfabAuthFailed);
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
                PlayFabClientAPI.LoginWithGameCenter(req, OnPlayFabLoginSucceeded, OnPlayfabAuthFailed);
            }
#endif

        }
        
        private static void OnGameServicesLogInFailed()
        {
            Debug.Log("Game Services Login Failed");
            PlayfabLoginWithDeviceID();
        }
        

        #endregion

        public static void PlayfabLoginWithDeviceID()
        {
            Debug.Log("Logging in with device ID.");
#if UNITY_ANDROID
            var req = new LoginWithAndroidDeviceIDRequest
            {
                CreateAccount = true,
                AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
                AndroidDevice = SystemInfo.deviceModel,
            };
            PlayFabClientAPI.LoginWithAndroidDeviceID(req, OnPlayFabLoginSucceeded, OnPlayfabAuthFailed);
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
            PlayFabClientAPI.LoginWithIOSDeviceID(req, OnPlayFabLoginSucceeded, OnPlayfabAuthFailed);
#endif
        }
        
        public static void LoginWithFacebook()
        {
            // SetMessage("Logging into Facebook...");
            //FB.Android.RetrieveLoginStatus(LoginStatusCallback);
            var perms = new List<string>() { "public_profile", "email" };
            // We invoke basic login procedure and pass in the callback to process the result
            FB.LogInWithReadPermissions(perms, OnFacebookLoggedIn);
        }

        public static void LoginWithGameServices()
        {
            GS.Init();
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

        #region PlayFab Login Handlers

        

        #endregion

        private static void OnFacebookLoggedIn(ILoginResult result)
        {
            // If result has no errors, it means we have authenticated in Facebook successfully
            if (result == null || string.IsNullOrEmpty(result.Error))
            {
                // SetMessage($"Facebook Auth Complete! Access Token: " + AccessToken.CurrentAccessToken.TokenString + $"\nLogging into PlayFab...  {AccessToken.CurrentAccessToken.UserId}");

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
                        OnPlayFabLoginSucceeded, OnPlayfabAuthFailed);
                }


            }
            else
            {
                // If Facebook authentication failed, we stop the cycle with the message
                // SetMessage("Facebook Auth Failed: " + result.Error + "\n" + result.RawResult, true);
            }
        }
        
        
        private static void OnPlayFabLoginSucceeded(LoginResult result)
        {
            // SetMessage("PlayFab Auth Complete. Session ticket: " + result.SessionTicket);
            Debug.Log($"PlayFab ID: {result.PlayFabId}");
            // GameConfig.GameConfig.Instance.Levels.FetchLevelsData();
            OnPlayerLoggedIn?.Invoke(result);
            OnPlayerLoggedInBasic?.Invoke();
        }
        
        public static void UpdatePlayerDisplayName(string displayName)
        {
            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = displayName
            };
            //TODO: Update Callbacks
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, Succeeded, Failed);

            static void Succeeded(UpdateUserTitleDisplayNameResult result)
            {
                Debug.Log($"Name Updated: {result.DisplayName}");
                GetPlayerData(null);
            }

            static void Failed(PlayFabError error)
            {
                
            }
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
            
            void OnPlayerProfileRequestError(PlayFabError obj)
            {
                Debug.LogError(obj.GenerateErrorReport());

            }

            void OnPlayerDataReceived(GetPlayerProfileResult obj)
            {
                Debug.Log(obj.PlayerProfile.ToString());
                OnPlayerProfileReceived?.Invoke(obj.PlayerProfile);
            }
        }
        
        private static void OnPlayfabAuthFailed(PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());
            // SetMessage("PlayFab Auth Failed: " + error.GenerateErrorReport(), true);
        }
        public void GameServices()
        {
            Debug.Log(EasyMobile.GameServices.LocalUser.userName);
        }

        #region Leaderboards
        public static void GetLeaderboardData(string statisticName, int startPosition)
        {
            var request  = new GetLeaderboardRequest
            {
                StartPosition = startPosition,
                StatisticName = statisticName,
                MaxResultsCount = 10
            };
            
            PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardSuccess, OnError );
            
            static void OnGetLeaderboardSuccess(GetLeaderboardResult obj)
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
            
            static void OnUpdatePlayerStatisticsSuccess(UpdatePlayerStatisticsResult obj)
            {
                Debug.Log($"Player Statistics Updated Successfully");
                //TODO: maybe update the leaderboard to reflect the change.
                GetPlayerData(null);
            }
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
            
            static void OnGetPlayerStatisticsSuccess(GetPlayerStatisticsResult obj)
            {
                Debug.Log("---------- Statistics Start -------------");
                foreach (var statisticValue in obj.Statistics)
                {
                    Debug.Log($"{statisticValue.StatisticName}: {statisticValue.Value}");
                }
                Debug.Log("---------- Statistics End -------------");
            }
        }
        
        #endregion

        public static void UpdatePlayerData(string key, string data)
        {
            var dict = new Dictionary<string, string> { { key, data } };
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest{
                Data = dict}, OnPlayerDataUpdated, OnError);
            
            static void OnPlayerDataUpdated(UpdateUserDataResult obj)
            {
                
            }

        }
        
        public static void LoadPlayerReadOnlyData<T>(string key, Action<T> onSuccess, Action onFailure = null)
        {
            PlayFabClientAPI.GetUserReadOnlyData(new GetUserDataRequest
            {
                Keys = new List<string>(){key}
            }, Success, Failed);

            void Success(GetUserDataResult result)
            {
                if (result.Data.ContainsKey(key))
                {
                    onSuccess?.Invoke(JsonConvert.DeserializeObject<T>(result.Data[key].Value));   
                }
                else
                {
                    onFailure?.Invoke();
                }
            }

            void Failed(PlayFabError error)
            {
                onFailure?.Invoke();
            }
        }

        public static void LoadPlayerData<T>(string key, Action<T> onSuccess, Action onFailure = null)
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest
            {
                Keys = new List<string>(){key}
            }, Success, Failed);

            void Success(GetUserDataResult result)
            {
                if (result.Data.ContainsKey(key))
                {
                    onSuccess?.Invoke(JsonConvert.DeserializeObject<T>(result.Data[key].Value));   
                }
                else
                {
                    onFailure?.Invoke();
                }
            }

            void Failed(PlayFabError error)
            {
                onFailure?.Invoke();
            }
        }

        [Button]
        public static void LoadPlayerInventory(Action<GetUserInventoryResult> resultCallBack, Action<PlayFabError> errorCallBack = null)
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), Success, Failed);

            void Success(GetUserInventoryResult result)
            {
                Debug.Log(result.Inventory);
                resultCallBack?.Invoke(result);
            }

            void Failed(PlayFabError error)
            {
                Debug.LogError(error.GenerateErrorReport());
                errorCallBack?.Invoke(error);
            }
        }
        
        [Button]
        public static void LoadCatalogData(Action<List<CatalogItem>> resultCallBack, Action<PlayFabError> errorCallBack = null)
        {
            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), Success, Failed);

            void Success(GetCatalogItemsResult result)
            {
                Debug.Log(result.Catalog);
                resultCallBack?.Invoke(result.Catalog);
            }

            void Failed(PlayFabError error)
            {
                Debug.Log(error.GenerateErrorReport());
                errorCallBack?.Invoke(error);
            }
        }
        
        [Button]
        public static void LoadPlayerDataCombined()
        {
            if(_playerCombinedRequestLock)
                return;
            _playerCombinedRequestLock = true;
            
            PlayFabClientAPI.GetPlayerCombinedInfo(new GetPlayerCombinedInfoRequest
            {
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true,
                    GetPlayerStatistics = true,
                    GetUserData = true,
                    GetUserInventory = true,
                    GetUserVirtualCurrency = true
                }
            }, Success, Failed);
            
            
            void Success(GetPlayerCombinedInfoResult result)
            {
                Debug.Log(result.InfoResultPayload);
                _playerCombinedRequestLock = false; 
            }

            void Failed(PlayFabError error)
            {
                Debug.LogError(error.GenerateErrorReport());
                _playerCombinedRequestLock = false;
            }
        }
        
        [Button]
        public static void ConsumeItem(string instanceID)
        {
            PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
            {
                ItemInstanceId = instanceID,
                ConsumeCount = 1
            }, Success, Failure);
            
            void Success(ConsumeItemResult result)
            {
                Debug.Log(result.ItemInstanceId);
            }

            void Failure(PlayFabError error)
            {
                Debug.LogError(error.GenerateErrorReport());
            }
        }
        
        [Button]
        public static void PurchaseItem(string itemID, int price, string virtualCurrency, Action<PurchaseItemResult> resultCallBack, Action<PlayFabError> errorCallBack)
        {
            PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
            {
                ItemId = itemID,
                Price = price,
                VirtualCurrency = virtualCurrency
            }, Success, Failure);
            
            void Success(PurchaseItemResult result)
            {
                Debug.Log(result.Items);
                resultCallBack?.Invoke(result);
            }

            void Failure(PlayFabError error)
            {
                Debug.LogError(error.GenerateErrorReport());
                errorCallBack?.Invoke(error);
            }
        }
        
        [Button]
        public static void GetOtherUserData(string playfabID)
        {
            PlayFabClientAPI.GetUserReadOnlyData(new GetUserDataRequest
            {
                PlayFabId = playfabID,
                Keys = new List<string> { "Equipped" }
            }, Success, Failure);
            
            void Success(GetUserDataResult result)
            {
                Debug.Log(result.ToJson());
            }

            void Failure(PlayFabError error)
            {
                Debug.LogError(error.GenerateErrorReport());
            }
            
        }
        
        public static void UpdatePlayerGameProfile(ProfileData profileData, Action<ExecuteFunctionResult> resultCallBack, Action<PlayFabError> errorCallBack)
        {
            Dictionary<string, object> body = new Dictionary<string, object>
            {
                { nameof(ProfileData), JsonConvert.SerializeObject(profileData) }
            };
            Debug.Log($"Updating PlayerProfile: {profileData}");
            ExecuteFunction(body, Constants.UPDATE_PLAYER_GAME_PROFILE, resultCallBack, errorCallBack);
        }

        [Button]
        public static void EquipItem(string itemID, Action<ExecuteFunctionResult> resultCallBack, Action<PlayFabError> errorCallBack)
        {
            Dictionary<string, object> body = new Dictionary<string, object>
            {
                { "itemID", itemID }
            };
            
            ExecuteFunction(body, Constants.EQUIP_ITEM_METHOD, resultCallBack, errorCallBack);
        }
        
        [Button]
        private static void ExecuteFunction(object body, string functionName,
            Action<ExecuteFunctionResult> resultCallBack, Action<PlayFabError> errorCallBack,
            bool generatePlayStreamEvent = true)
        {
            PlayFabCloudScriptAPI.ExecuteFunction(new ExecuteFunctionRequest
            {
                FunctionName = functionName,
                FunctionParameter = body,
                GeneratePlayStreamEvent = generatePlayStreamEvent
            }, Success, Failure);

            void Success(ExecuteFunctionResult response)
            {
                Debug.Log($"Func {functionName} Success, Result: {response.FunctionResult} -- Execution Time: {response.ExecutionTimeMilliseconds}");
                resultCallBack?.Invoke(response);
            }
            
            void Failure(PlayFabError error)
            {
                Debug.LogError(error.GenerateErrorReport());
                errorCallBack?.Invoke(error);
            }
        }

        [Button]
        public static void FetchTitleData(List<string> keys, Action<Dictionary<string, string>> resultCallBack)
        {
            PlayFabClientAPI.GetTitleData(new GetTitleDataRequest
            {
                Keys = keys
            }, Success, Failure);

            void Success(GetTitleDataResult getTitleDataResult)
            {
                resultCallBack?.Invoke(getTitleDataResult.Data);
            }

            void Failure(PlayFabError playFabError)
            {
                Debug.Log(playFabError.GenerateErrorReport());
            }
        }
        
        //Should be moved to another script
        public void RateUs()
        {
            // Show the rating dialog with default behavior
            StoreReview.RequestRating();
        }
    }
}