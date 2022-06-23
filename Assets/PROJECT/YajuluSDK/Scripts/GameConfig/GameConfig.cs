using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Project.GameSpecific.Scripts.Data;
using PROJECT.Scripts.Enums;
using Project.YajuluSDK.Scripts.Essentials;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.YajuluSDK.Scripts.GameConfig
{
	[CreateAssetMenu(fileName = "GameConfig")]
	[Serializable]
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

				starsIndexes[0] = values[3].Equals("") ? 2 : 3;
				starsIndexes[1] = values[7].Equals("") ? 6 : 7;
				starsIndexes[2] = 9;

				if (!_dataDict.ContainsKey(key))
				{
					_levelDataKeys.Add(key);
					_dataDict.Add(key, new LevelDictData { words = wordList, starsCounter = starsIndexes.ToArray() });
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