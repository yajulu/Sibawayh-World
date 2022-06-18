using System;
using System.Collections.Generic;
using PROJECT.Scripts.Enums;
using UnityEngine;
using UnityEngine.Purchasing;
using Json = EasyMobile.MiniJSON.Json;
using Object = UnityEngine.Object;

namespace _YajuluSDK._Scripts.Tools
{
    public static class SaveUtility 
    {
        public static void SaveList<T>(string key, T[] list)
        {
            var json = JsonUtility.ToJson(list);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public static T[] LoadList<T>(string key)
        {
            var str = PlayerPrefs.GetString(key);
            var array = JsonUtility.FromJson<T[]>(str);
            // var array = Array.ConvertAll(list.ToArray(), ConvertObject);
            return array;

            T ConvertObject(object input)
            {
                return (T) input;
            }
            
        }
        
        public static int[] LoadListToInt(string key)
        {
            var str = PlayerPrefs.GetString(key);
            var list = (List<string>)Json.Deserialize(str);
            var array = Array.ConvertAll(list.ToArray(), ConvertObject);
            return array;

            int ConvertObject(object input)
            {
                return int.Parse((string)input);
            }
            
        }
    }
}
