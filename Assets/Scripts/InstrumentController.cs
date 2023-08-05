using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InstrumentController : MonoBehaviour, ISerializationCallbackReceiver
{
	#region SerializedTuple<float,int>
	[Serializable]
	public struct SerializableTuple
	{
		public float time;
		public int sort;
	}

	
	public List<SerializableTuple> serializedTuples;

	public void OnBeforeSerialize()
	{
		serializedTuples = new List<SerializableTuple>();
		foreach (var tuple in instrumentBeatList)
		{
			serializedTuples.Add(new SerializableTuple { time = tuple.Item1, sort = tuple.Item2 });
		}
	}

	public void OnAfterDeserialize()
	{
		instrumentBeatList = new List<Tuple<float, int>>();
		foreach (var serializedTuple in serializedTuples)
		{
			instrumentBeatList.Add(Tuple.Create(serializedTuple.time, serializedTuple.sort));
		}
	}
	

	#endregion
	public List<Tuple<float, int>> instrumentBeatList = new List<Tuple<float, int>>();

	public int index;
	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}