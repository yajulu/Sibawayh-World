using System;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace PROJECT.Scripts
{
#if UNIT_EDITOR
	[InitializeOnLoad]
	public class ProjectInitializer {
		static ProjectInitializer()
		{
			Debug.Log("Initializing Project...");
			UpdateKeyStorePath();
			Debug.Log("InitializationComplete.");
		}
		
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
	}
#endif

}
