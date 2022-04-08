using System.Collections;
using System.Collections.Generic;
using Firebase;
using UnityEngine;


public class Analytics_Manager : MonoBehaviour
{
    FirebaseApp app;
    // Start is called before the first frame update
    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                Debug.Log("Firebase app " + app.Name + " is Initialized successfully");
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    public void LogEvent()
    {
        // Log an event with a float parameter
        Firebase.Analytics.FirebaseAnalytics
          .LogEvent("progress", "percent", 0.4f);
        // Log an event with multiple parameters, passed as a struct:
        Firebase.Analytics.Parameter[] LevelUpParameters = {
        new Firebase.Analytics.Parameter(
            Firebase.Analytics.FirebaseAnalytics.ParameterLevel, 5),
        new Firebase.Analytics.Parameter(
            Firebase.Analytics.FirebaseAnalytics.ParameterCharacter, "FakeCharacter"),
        new Firebase.Analytics.Parameter(
            "hit_accuracy", 3.14f)
        };
        Firebase.Analytics.FirebaseAnalytics.LogEvent(
        Firebase.Analytics.FirebaseAnalytics.EventLevelUp,
        LevelUpParameters);
    }
}
