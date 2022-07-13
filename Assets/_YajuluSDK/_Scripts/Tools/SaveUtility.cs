using System;
using System.Collections.Generic;
using _YajuluSDK._Scripts.Social;
using Newtonsoft.Json;
using UnityEngine;


namespace _YajuluSDK._Scripts.Tools
{
    public static class SaveUtility 
    {
        public static void SaveObject<T>(string key, T obj, bool localOnly = false)
        {
            var json = JsonConvert.SerializeObject(obj);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
            if (localOnly)
                return;
            PlayfabManager.UpdatePlayerData(key, json);
        }

        public static T LoadObject<T>(string key)
        {
            var str = PlayerPrefs.GetString(key);
            var obj = JsonConvert.DeserializeObject<T>(str);
            Debug.Log(obj);
            // var array = Array.ConvertAll(list.ToArray(), ConvertObject);
            return obj;
        }
        
        public static void DeleteObject(string key)
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
        }

        public static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }
        
    }
}
