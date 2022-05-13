using _YajuluSDK._Scripts.UI;
using PROJECT.Scripts.Game;
using PROJECT.Scripts.Game.Controllers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PROJECT.Scripts.UI.Screens
{
    public class Screen_GameMode : UIScreenBase
    {
        [SerializeField] private LetterSpawner letterSpawner;
        // [SerializeField]


        protected override void OnScreenOpenEnded()
        {
            base.OnScreenOpenEnded();
            
            //TODO: Use UImanager events Events
            GameModeManager.Instance.StartGameMode();
        }

        [Button]
        private void SetRefs()
        {
            letterSpawner = GetComponentInChildren<LetterSpawner>();
        }
    }
}
