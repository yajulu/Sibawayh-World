using System;
using UnityEngine;

namespace PROJECT.Scripts.Core
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class AutoHideGameObject : MonoBehaviour
    {
        private SpriteRenderer 
        private void OnBecameInvisible()
        {
            gameObject.SetActive(false);
        }
        
        private void OnBecameVisible()
        {
            gameObject.SetActive(true);
        }
        
    }
}
