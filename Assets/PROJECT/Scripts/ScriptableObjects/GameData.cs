using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        [SerializeField, TitleGroup("Progress")]
        private string progressKey;
        
        [SerializeField, TitleGroup("Data")] private Dictionary<(eLevelType, string), LevelDictData> _dataDict;
        [SerializeField, TitleGroup("Data")] private List<(eLevelType, string)> _levelDataKeys;

        [SerializeField, TitleGroup("Data")] private Dictionary<eLevelType, string> _levelTypesNamesDict;

        [SerializeField, Sirenix.OdinInspector.FilePath, TitleGroup("Data")]
        private string filePath;

        private string _line;
        public GameData()
        {
            //_dataDict = new Dictionary<(eLevelType, string), LevelDictData>();
            //_levelDataKeys = new List<(eLevelType, string)>();
            //_levelTypesNamesDict = new Dictionary<eLevelType, string>();
            //progressKey = "levelProgress";
        }

        public LevelData TryGetLevelData((eLevelType, string) key)
        {
            if (!_dataDict.TryGetValue(key, out var levelDictData)) return null;
            
            var data = new LevelData(key.Item1, key.Item2, levelDictData.words.ToArray(), levelDictData.starsCounter);
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
        
        public int GetLevelsCount => _levelDataKeys.Count;

        public string ProgressKey => progressKey;


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
                int[] starsIndexes = new int[3];

                int stars = 0;

                for (int i = values.Length - 1; i > 1; i--)
                {
                    // stars = (values[i].Equals("") ? 1 : 0) & stars;
                    if (values[i].Equals(""))
                        continue;
                    wordList.Add(values[i].Trim());
                }

                starsIndexes[0] = values[3].Equals("") ? 2 : 3;
                starsIndexes[1] = values[7].Equals("") ? 6 : 7;
                starsIndexes[2] = 9;

                if (!_dataDict.ContainsKey(key))
                {
                    _levelDataKeys.Add(key);
                    _dataDict.Add(key, new LevelDictData
                    {
                        words = wordList,
                        starsCounter = starsIndexes.ToArray()
                    } );
                }
            }
        }

        [Serializable]
        public struct LevelDictData
        {
            public List<string> words;
            public int[] starsCounter;
            
        }
    }
}
