using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace _YajuluSDK._Scripts.Essentials
{
    public static class ExtensionMethods 
    {
        //Breadth-first search
        public static Transform FindDeepChild(this Transform aParent, string aName)
        {
            if (aParent != null)
            {
                var result = aParent.Find(aName);
                if (result != null)
                    return result;

                foreach (Transform child in aParent)
                {
                    result = child.FindDeepChild(aName);
                    if (result != null)
                        return result;
                }
            }

            return null;
        }

        public static T FindDeepChild<T>(this Transform aParent, string aName)
        {
            T result = default(T);

            var transform = aParent.FindDeepChild(aName);

            if (transform != null)
            {
                result = (typeof(T) == typeof(GameObject)) ? (T)Convert.ChangeType(transform.gameObject, typeof(T)) : transform.GetComponent<T>();
            }

            if (result == null)
            {
                Debug.LogError($"FindDeepChild didn't find: '{aName}' on GameObject: '{aParent.name}'");
            }

            return result;
        }
        
        private static Random rng = new Random();  

        public static void Shuffle<T>(this IList<T> list)  
        {  
            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                (list[k], list[n]) = (list[n], list[k]);
            }  
        }

    }
}
