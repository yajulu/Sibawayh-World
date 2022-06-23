using System;
using PROJECT.Scripts.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.GameSpecific.Scripts.Data
{
    [Serializable, InlineProperty]
    public class LevelData
    {
        [SerializeField] private eLevelType _levelType;
        [SerializeField] private string _keyWord;
        [SerializeField] private string[] _words;
        [SerializeField] private int[] _starsCounter;
        public string KeyWord => _keyWord;
        public eLevelType LevelType => _levelType;
        public string[] Words => _words;
        public int[] StarsCounter => _starsCounter;


        public LevelData(eLevelType type, string keyWord, string[] words, int[] starsCounter)
        {
            _levelType = type;
            _keyWord = keyWord;
            _words = words;
            _starsCounter = starsCounter;
        }
        
    }
}
