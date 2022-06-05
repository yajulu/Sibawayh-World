using System;
using System.Collections.Generic;
using System.IO;
using PROJECT.Scripts.Data;
using PROJECT.Scripts.Enums;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace PROJECT.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data/GameData", menuName = "GameData"), Serializable]
    public class GameData : SerializedScriptableObject
    {
        [SerializeField] private Dictionary<(eLevelType, string), List<string>> _dataDict;
        [SerializeField] private List<(eLevelType, string)> _levelDataKeys;

        [SerializeField] private Dictionary<eLevelType, string> _levelTypesNamesDict;

        [SerializeField, Sirenix.OdinInspector.FilePath]
        private string filePath;

        private string _line;
        

        public GameData()
        {
            _dataDict = new Dictionary<(eLevelType, string), List<string>>();
            _levelDataKeys = new List<(eLevelType, string)>();
            _levelTypesNamesDict = new Dictionary<eLevelType, string>();
        }

        public LevelData TryGetLevelData((eLevelType, string) key)
        {
            if (!_dataDict.TryGetValue(key, out var list)) return null;
            
            var data = new LevelData(key.Item1, key.Item2, list.ToArray());
            return data;
        }

        public LevelData GetLevelData(int levelNumber)
        {
            if (levelNumber > _levelDataKeys.Count || levelNumber < 0)
            {
                return null;
            }
            
            return TryGetLevelData(_levelDataKeys[levelNumber]);
        }

        public string GetLevelTypeName(eLevelType type)
        {
            return _levelTypesNamesDict[type];
        }
        
        [Button]
        private void LoadData()
        {
            _dataDict.Clear();
            _levelDataKeys.Clear();
            StreamReader streamReader = new StreamReader(filePath);

            bool endOfFile = false;

            while (!endOfFile)
            {
                _line = streamReader.ReadLine();
                if (_line == null)
                {
                    endOfFile = true;
                    break;
                }

                var values = _line.Split(',');
                var key = ((eLevelType)int.Parse(values[0]), values[1].Trim());
                    
                List<string> wordList = new List<string>();

                for (int i = 2; i < values.Length; i++)
                {
                    wordList.Add(values[i].Trim());
                }

                if (!_dataDict.ContainsKey(key))
                {
                    _levelDataKeys.Add(key);
                    _dataDict.Add(key, wordList);
                }
                
            }
        }
    }
}
