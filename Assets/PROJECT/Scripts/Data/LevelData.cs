using System;
using System.Collections;
using PROJECT.Scripts.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PROJECT.Scripts.Data
{
    [Serializable, InlineProperty]
    public class LevelData
    {
        [SerializeField] private eLevelType _levelType;
        [SerializeField] private string _keyWord;
        [SerializeField] private string[] _words;

        public string KeyWord => _keyWord;
        public eLevelType LevelType => _levelType;
        public string[] Words => _words;
        
        public LevelData(eLevelType type, string keyWord, string[] words)
        {
            _levelType = type;
            _keyWord = keyWord;
            _words = words;
        }
        
    }
}
