using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;


namespace _YajuluSDK._Scripts.Tools
{
    public static class SaveUtility 
    {
        public static void SaveList<T>(string key, T[] list)
        {
            var json = JsonConvert.SerializeObject(list);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public static T[] LoadList<T>(string key)
        {
            var str = PlayerPrefs.GetString(key);
            var list = JsonConvert.DeserializeObject<T[]>(str);
            // var array = Array.ConvertAll(list.ToArray(), ConvertObject);
            return list;
        }

        public static void SaveObject<T>(string key, T obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public static T LoadObject<T>(string key)
        {
            var str = PlayerPrefs.GetString(key);
            var obj = JsonConvert.DeserializeObject<T>(str);
            Debug.Log(obj);
            // var array = Array.ConvertAll(list.ToArray(), ConvertObject);
            return obj;
        }
        
        public static int[] LoadListToInt(string key)
        {
            var str = PlayerPrefs.GetString(key);
            var list = JsonConvert.DeserializeObject<int[]>(str);
            // var array = Array.ConvertAll(list.ToArray(), ConvertObject);
            return list;

            int ConvertObject(object input)
            {
                return Convert.ToInt32(input);
            }
            
        }
    }
}
