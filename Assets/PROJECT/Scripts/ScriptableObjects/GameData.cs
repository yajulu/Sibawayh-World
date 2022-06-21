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
        
        [SerializeField, TitleGroup("Data")] private GameDataDict _dataDict;
        [SerializeField, TitleGroup("Data")] private List<LevelDictKey> _levelDataKeys;

        [SerializeField, TitleGroup("Data")] private LevelNamesType _levelTypesNamesDict;

        [SerializeField, Sirenix.OdinInspector.FilePath, TitleGroup("Data")]
        private string filePath;

        private string _line;
        public GameData()
        {
            progressKey = "levelProgress";
        }

        public LevelData TryGetLevelData(LevelDictKey key)
        {
            Debug.Log($"Data Dict: {_dataDict}");
            //if (!_dataDict.TryGetValue(key, out var levelDictData)) return null;
            var levelDictData = _dataDict[key];
            Debug.Log($"Level Data: {levelDictData}");
            var data = new LevelData(key.Type, key.KeyWord, levelDictData.words.ToArray(), levelDictData.starsCounter);
            return data;
        }

        public LevelData GetLevelData(int levelNumber)
        {
            Debug.Log($"Keys {_levelDataKeys.Count} -- {levelNumber}");
            if (levelNumber > _levelDataKeys.Count || levelNumber < 0)
            {
                return null;
            }
            Debug.Log($"Level Key: {_levelDataKeys[levelNumber]}");
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
            _dataDict = new GameDataDict();
            _levelDataKeys = new List<LevelDictKey>();
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
                var key = new LevelDictKey
                {
                    Type = (eLevelType)int.Parse(values[0]),
                    KeyWord = values[1].Trim()   
                };
                    
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

        [Serializable]
        public struct LevelDictKey
        {
            public eLevelType Type;
            public string KeyWord;

        }
        
        
    }

    [Serializable]
    public class LevelNamesType : UnitySerializedDictionary<eLevelType, string>
    { }
    
    [Serializable]
    public class GameDataDict : UnitySerializedDictionary<GameData.LevelDictKey, GameData.LevelDictData>
    { }
    public abstract class UnitySerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector]
        protected List<TKey> keyData = new List<TKey>();

        [SerializeField, HideInInspector]
        protected List<TValue> valueData = new List<TValue>();

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Clear();
            //Debug.LogError(this.keyData.Count + "   " + valueData.Count);

            for (int i = 0; i < this.keyData.Count; i++)
            {
                this[keyData[i]] = valueData[i];
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            keyData.Clear();
            valueData.Clear();

            foreach (var entry in this)
            {
                keyData.Add(entry.Key);
                valueData.Add(entry.Value);
            }
        }
    }
}
