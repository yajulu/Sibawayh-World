using System.Collections.Generic;
using UnityEngine;

namespace Project.YajuluSDK.Scripts.Essentials
{
	public abstract class UnitySerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
	{
		[SerializeField, HideInInspector]
		protected List<TKey> keyData = new List<TKey>();

		[SerializeField, HideInInspector]
		protected List<TValue> valueData = new List<TValue>();

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			Clear();

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