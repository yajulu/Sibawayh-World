using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using _YajuluSDK._Scripts.Essentials;
using _YajuluSDK._Scripts.Social;
using Newtonsoft.Json;
using PROJECT.Scripts.Data;
using PROJECT.Scripts.Data.Items;
using PROJECT.Scripts.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _YajuluSDK._Scripts.GameConfig
{
	[CreateAssetMenu(fileName = "GameConfig")]
	[Serializable]
	public class GameConfig : GameConfigBase
	{
		public DebugVariablesEditor Debug = new DebugVariablesEditor();
		public InputVariablesEditor Input = new InputVariablesEditor();
		public GamePlayVariablesEditor GamePlay = new GamePlayVariablesEditor();
		public LevelsVariablesEditor Levels = new LevelsVariablesEditor();
		public ShopVariableEditor Shop = new ShopVariableEditor();
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

		[EnableIf(nameof(DebugMode)), SerializeField]
		private bool m_UnlockAllStages = false;

		public bool UnlockAllStages
		{
			get { return m_UnlockAllStages && DebugMode; }
		}
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
			[SerializeField, TitleGroup("Level State"), InlineEditor(InlineEditorModes.LargePreview)]
			private Sprite unlocked;

			[SerializeField, TitleGroup("Level State"), InlineEditor(InlineEditorModes.LargePreview)]
			private Sprite locked;

			[SerializeField, TitleGroup("Level State"), InlineEditor(InlineEditorModes.LargePreview)]
			private Sprite complete;

			[SerializeField, TitleGroup("Level Shadow"), InlineEditor(InlineEditorModes.LargePreview)]
			private Sprite blueShadow;

			[SerializeField, TitleGroup("Level Shadow"), InlineEditor(InlineEditorModes.LargePreview)]
			private Sprite redShadow;

			[SerializeField, TitleGroup("Level Star"), InlineEditor(InlineEditorModes.LargePreview)]
			private Sprite levelStar;

			[SerializeField, TitleGroup("Prefabs"), AssetsOnly]
			private GameObject buttonPrefab;

			[SerializeField, TitleGroup("Prefabs"), AssetsOnly]
			private GameObject stonePrefab;

			[SerializeField, TitleGroup("Prefabs"), AssetsOnly]
			private GameObject mapElementPrefab;

			[SerializeField, TitleGroup("Prefabs"), AssetsOnly]
			private GameObject button2DPrefab;

			[SerializeField, TitleGroup("Prefabs"), AssetsOnly]
			private GameObject stone2DPrefab;

			[SerializeField, TitleGroup("Prefabs"), AssetsOnly]
			private GameObject mapElement2DPrefab;

			public Sprite Unlocked => unlocked;
			public Sprite Locked => locked;
			public Sprite Complete => complete;
			public Sprite BlueShadow => blueShadow;
			public Sprite RedShadow => redShadow;
			public Sprite LevelStar => levelStar;
			public GameObject ButtonPrefab => buttonPrefab;
			public GameObject StonePrefab => stonePrefab;
			public GameObject MapElementPrefab => mapElementPrefab;
			public GameObject MapElement2DPrefab => mapElement2DPrefab;
			public GameObject Button2DPrefab => button2DPrefab;
			public GameObject Stone2DPrefab => stone2DPrefab;
		}
		
		[SerializeField, TitleGroup("Data")] private LevelDataDict _dataDict;
		[SerializeField, TitleGroup("Data")] private List<LevelDictKey> _levelDataKeys;
		[SerializeField, TitleGroup("Data")] private LevelNamesType _levelTypesNamesDict;

		[SerializeField, Sirenix.OdinInspector.FilePath, TitleGroup("Data")]
		private string filePath;

		private string _line;

		public int GetLevelsCount => _levelDataKeys.Count;
		
		public LevelData TryGetLevelData(LevelDictKey key)
		{
			//if (!_dataDict.TryGetValue(key, out var levelDictData)) return null;
			var levelDictData = _dataDict[key];

			var data = new LevelData(key.Type, key.KeyWord, levelDictData.words.ToArray(), levelDictData.starsCounter);
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

		public void FetchLevelsData()
		{
			PlayfabManager.FetchTitleData(null, Success);
			
			void Success(Dictionary<string, string> obj)
			{
				if (obj.ContainsKey("LevelDict"))
				{
					_dataDict = JsonConvert.DeserializeObject<LevelDataDict>(obj["LevelDict"]);
					Debug.Log("Levels Updated Successfully.");
				}
				
				if (obj.ContainsKey("LevelDictKeys"))
				{
					_levelDataKeys = JsonConvert.DeserializeObject<List<LevelDictKey>>(obj["LevelDictKeys"]);
					Debug.Log("Levels Keys Updated Successfully.");
				}
				
			}
		}
		
		[Button]
		private void LoadData()
		{
			_dataDict = new LevelDataDict();
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
				var key = new LevelDictKey { Type = (eLevelType)int.Parse(values[0]), KeyWord = values[1].Trim() };
				
				List<string> wordList = new List<string>();
				int[] starsIndexes = new int[3];

				int stars = 0;

				for (int i = values.Length - 1; i > 1; i--)
				{
					// stars = (values[i].Equals("") ? 1 : 0) & stars;
					if (values[i].Equals("")) continue;
					wordList.Add(values[i].Trim());
				}
				
				if( wordList.Count < 7 )
					continue;
				
				starsIndexes[0] = values[4].Equals("") ? 2 : 3;
				starsIndexes[1] = values[8].Equals("") ? 4 : 5;
				starsIndexes[2] = wordList.Count - 1;

				if (!_dataDict.ContainsKey(key))
				{
					_levelDataKeys.Add(key);
					_dataDict.Add(key, new LevelDictData { words = wordList, starsCounter = starsIndexes.ToArray() });
				}
			}
		}

		[Button]
		private void RandomizeLevels()
		{
			_levelDataKeys.Shuffle();
		}

		[Button]
		private void ExportLevelsAsJSON()
		{
			levelsDictJson = JsonConvert.SerializeObject(_dataDict);
			levelsKeysJson = JsonConvert.SerializeObject(_levelDataKeys);
		}

		[SerializeField, TextArea, FoldoutGroup("JSON")] private string levelsDictJson;
		[SerializeField, TextArea, FoldoutGroup("JSON")] private string levelsKeysJson;

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

		[Serializable]
		public class LevelNamesType : UnitySerializedDictionary<eLevelType, string>
		{
		}

		[Serializable]
		public class LevelDataDict : UnitySerializedDictionary<LevelDictKey, LevelDictData>
		{
		}
	}

	[Serializable]
	public class ShopVariableEditor
	{
		[SerializeField, OnValueChanged(nameof(RefreshDict), includeChildren:true)] private ShopSpritesDictionary shopDictionary;
		[SerializeField] private ShopItemIDSpriteDictionary shopItemIDDictionary;
		[SerializeField] private SpriteSerializedDictionary virtualCurrenciesDict;
		
		public ShopSpritesDictionary ShopDictionary => shopDictionary;
		public ShopItemIDSpriteDictionary ShopItemIDDictionary => shopItemIDDictionary;
		public SpriteSerializedDictionary VirtualCurrenciesDict => virtualCurrenciesDict;
		
		[Button]
		private void RefreshDict()
		{
			ShopItemIDDictionary.LoadDictionary(shopDictionary);
		}

		#region Dictionary Definitions
		
		[Serializable]
		public class SpriteSerializedDictionary : UnitySerializedDictionary<string, Sprite>
		{
		}

		[Serializable]
		public class ShopItemIDSpriteDictionary : UnitySerializedDictionary<string, Sprite>
		{
			public void LoadDictionary(ShopSpritesDictionary dict)
			{
				Clear();
				foreach (var key in dict.Keys)
				{
					var prefix = key.ToString().ToLower() + "_";
					var list = dict[key].spriteList;
					
					for (var i = 0; i < list.Count; i++)
					{
						Add(prefix + i, list[i]);
					}
				}
			}
		}
		
		[Serializable]
		public class ShopSpritesDictionary : UnitySerializedDictionary<eItemType, ItemSpriteList>
		{
		}
		
		[Serializable]
		public class ItemSpriteList
		{
			public List<Sprite> spriteList;
		}

		

		#endregion
		
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