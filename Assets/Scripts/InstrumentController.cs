using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using DG.Tweening;

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
	public Ease firstEaseType;
	public Ease secondEaseType;
	public float transScale;
	public float firstDuration;
	public float secondDuration;
	public int index;
	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	

	public void ChangeInstrumentScale()
	{
		// 입력 전 모든 움직임 정지
		transform.DOKill();
		// 최초상태로 복귀
		transform.localScale = Vector3.one;
		
		Vector3 targetSize = transform.localScale * transScale;
		transform.DOScale(targetSize, firstDuration).SetEase(firstEaseType).OnComplete(() =>
		{
			transform.DOScale(Vector3.one, secondDuration).SetEase(secondEaseType);
		});
	}
}