using System;
using System.Collections.Generic;
using _YajuluSDK._Scripts.Essentials;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _YajuluSDK._Scripts.GameConfig
{
    
    public abstract class GameConfigBase : SingletonScriptableObject<GameConfig>
    {
        
        [PropertyOrder(100)]
        public ResolutionsData ResolutionsData = new ResolutionsData();
    }

    [Serializable]
    public abstract class DebugVariablesEditorBase
    {
        public bool GodMode = false;

        [SerializeField] private bool m_DebugMode = false;
        public bool DebugMode
        {
            get
            {
                if (Debug.isDebugBuild)
                    return m_DebugMode;
                else
                    return false;
            }
        }
        
    }

    [Serializable]
    public abstract class InputVariablesEditorBase
    {
    }

    [Serializable]
    public abstract class LevelsVariablesEditorBase
    {
    }

    [Serializable]
    public abstract class GamePlayVariablesEditorBase
    {
    }

    [Serializable]
    public abstract class HUDVariablesEditorBase
    {
    }

    [Serializable]
    public abstract class PlayerVariablesEditorBase
    {
    }

    [Serializable]
    public abstract class CameraVariablesEditorBase
    {
    }

    [Serializable]
    public class ResolutionsData
    {
        [SerializeField, ListDrawerSettings(ListElementLabelName = nameof(ResolutionData.NameLabel))]
        private List<ResolutionData> m_ResolutionDatas;

        public bool IsDebugEnabled = true;

        public ResolutionData GetResolutionData()
        {
            return GetResolutionData(Camera.main.pixelWidth, Camera.main.pixelHeight);
        }

        public ResolutionData GetResolutionData(int i_Width, int i_Height)
        {
            ResolutionData result = null;
            foreach (ResolutionData resolutionData in m_ResolutionDatas)
            {
                if ((i_Width == resolutionData.Width && i_Height == resolutionData.Height) ||
                    (i_Width == resolutionData.Height && i_Height == resolutionData.Width))
                {
                    result = resolutionData;
                    break;
                }
            }

            if (IsDebugEnabled && GameConfig.Instance.Debug.DebugMode)
            {
                if (result == null)
                {
                    Debug.LogErrorFormat("ResolutionData for {0} {1} not found. pls add this info to gameConfigV2 (ctrl+shift+t) \n NOTE: this info needs only for Editor mode", Camera.main.pixelWidth, Camera.main.pixelHeight);
                }
                else
                {
                    Debug.Log("ResolutionData = " + result);
                }
            }

            return result;
        }

        [Serializable]
        public class ResolutionData
        {
            public string Name;
            public int Width;
            public int Height;
            public float DPI;

            public string NameLabel { get { return "" + Width + ", " + Height + ", " + DPI; } }

            public override string ToString()
            {
                return Name + " " + NameLabel;
            }
        }
    }
}