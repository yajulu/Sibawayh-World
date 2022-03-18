using System;
using System.IO;
using Facebook.Unity.Settings;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace _YajuluSDK._Scripts
{

    [CreateAssetMenu()]
    public class ProjectConfig : ScriptableObject
    {
        public static FacebookSettings CurrentSettings;
#if UNITY_EDITOR
        [Button]
        
        private static void UpdateKeyStorePath()
        {
            
            var yajuluPath = Environment.GetEnvironmentVariable("Yajulu", EnvironmentVariableTarget.User);
            if (yajuluPath == null)
            {
                Debug.LogError("Can't find Environment Variable 'Yajulu', Please set it in the User Environment Varaiables.");
                return;
            }
			
            var vProductName = PlayerSettings.productName;
            var keyStorePath = $"{yajuluPath}\\{vProductName}\\keystore\\{vProductName}.keystore";
            if(!File.Exists(keyStorePath))
            {    
                Debug.LogError($"Cannot find a keystore with this path {keyStorePath}");
				
                return;
            }
            PlayerSettings.Android.keystoreName = keyStorePath;
            Debug.Log($"KeyStore Path Updated to {keyStorePath}");
        }
#endif
    }

}
