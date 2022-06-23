using System;
using UnityEngine;

namespace Project.YajuluSDK.Scripts.Essentials
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

    }
}
