using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
public class PluginsTestScript : MonoBehaviour
{
    private void Awake ()
    {
        if (!FB.IsInitialized) {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        } else {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
        
    }
    private void InitCallback ()
    {
        if (FB.IsInitialized) {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        } else {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
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

    private void LoginStatusCallback(ILoginStatusResult result) 
    {
        if (!string.IsNullOrEmpty(result.Error)) {
            Debug.Log("Error: " + result.Error);
        } else if (result.Failed) {
            Debug.Log("Failure: Access Token could not be retrieved");
        } else {
            // Successfully logged user in
            // A popup notification will appear that says "Logged in as <User Name>"
            Debug.Log("Success: " + result.AccessToken.UserId);
        }
    }


    private void AuthCallback (ILoginResult result) {
        if (FB.IsLoggedIn) {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log( "USER ID" + aToken.UserId);
        
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions) {
                Debug.Log("PARAMETERS " + perm);
            }
        } else {
            Debug.Log("ERROR IS: " + result.Error);
            Debug.Log("User cancelled login");
        }
    }


    public void FBExpressLoginTest()
    {
        //FB.Android.RetrieveLoginStatus(LoginStatusCallback);
        var perms = new List<string>(){"public_profile", "email"};
        FB.LogInWithReadPermissions(perms, AuthCallback);
   


   
    }
}
