using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _YajuluSDK._Scripts.GameConfig
{
    [CreateAssetMenu(fileName = "GameConfig")]
    public class GameConfig : GameConfigBase
    {
        public DebugVariablesEditor Debug = new DebugVariablesEditor();
        public InputVariablesEditor Input = new InputVariablesEditor();
        public GamePlayVariablesEditor GamePlay = new GamePlayVariablesEditor();
        public LevelsVariablesEditor Levels = new LevelsVariablesEditor();
        public HUDVariablesEditor HUD = new HUDVariablesEditor();
        public PlayerVariablesEditor Player = new PlayerVariablesEditor();
        public CameraVariablesEditor Camera = new CameraVariablesEditor();
    }

    [Serializable]
    public class DebugVariablesEditor : DebugVariablesEditorBase
    {
        //IMPORTANT - FOLLOW THIS TEMPLATE FOR DEBUG VARS SO THEY ARE NOT CALLED ON FINAL BUILDS
        //[SerializeField, ShowIf("DebugMode")] private bool m_VarName = false;
        //public bool VarName { get { return DebugMode && m_VarName; } }

        [EnableIf(nameof(DebugMode)), SerializeField] private bool m_UnlockAllStages = false;
        public bool UnlockAllStages { get { return m_UnlockAllStages && DebugMode; } }
    }

    [Serializable]
    public class InputVariablesEditor : InputVariablesEditorBase
    {

    }

    [Serializable]
    public class LevelsVariablesEditor : LevelsVariablesEditorBase
    {
        public LevelButtonUI levelButtonUI = new LevelButtonUI();
        
        [Serializable]
        public class LevelButtonUI
        {
            [SerializeField, TitleGroup("Level State"), InlineEditor(InlineEditorModes.LargePreview)] private Sprite unlocked;
            [SerializeField, TitleGroup("Level State"), InlineEditor(InlineEditorModes.LargePreview)] private Sprite locked;
            [SerializeField, TitleGroup("Level State"), InlineEditor(InlineEditorModes.LargePreview)] private Sprite complete;

            [SerializeField, TitleGroup("Level Shadow"), InlineEditor(InlineEditorModes.LargePreview)]
            private Sprite blueShadow;
            
            [SerializeField, TitleGroup("Level Shadow"), InlineEditor(InlineEditorModes.LargePreview)]
            private Sprite redShadow;

            [SerializeField, TitleGroup("Level Star"), InlineEditor(InlineEditorModes.LargePreview)]
            private Sprite levelStar;
            
            public Sprite Unlocked => unlocked;

            public Sprite Locked => locked;

            public Sprite Complete => complete;

            public Sprite BlueShadow => blueShadow;

            public Sprite RedShadow => redShadow;

            public Sprite LevelStar => levelStar;
        }
    }
    
    [Serializable]
    public class GamePlayVariablesEditor : GamePlayVariablesEditorBase
    {
        
    }

    [Serializable]
    public class HUDVariablesEditor : HUDVariablesEditorBase
    {
        
    }

    [Serializable]
    public class PlayerVariablesEditor : PlayerVariablesEditorBase
    {
        
    }

    [Serializable]
    public class CameraVariablesEditor : CameraVariablesEditorBase
    {
        
    }

}
