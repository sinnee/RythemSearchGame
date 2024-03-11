using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "BeatMaker/Create Score Item")]
public class NoteScoreItem : ScriptableObject
{
    public Dictionary<int, int> scoreDic;
    public GameObject instPrefab;
    public GameObject inst;
    public Vector3 position;

    public NoteScoreItem(List<int[]> scoreArray, GameObject instPrefab)
    {
        this.instPrefab = instPrefab;
    }
    public NoteScoreItem()
    {
        scoreDic = new Dictionary<int, int>();

        //기본 악기?
        //this.instrument = instrument; 
    }

    /// <summary>
    /// 악기 생성 후 저장된 포지션 값 셋팅
    /// </summary>
    public void MakeInstrument()
    {
        inst = Instantiate(instPrefab);
        inst.transform.position = position;
    }

    /// <summary>
    /// 게임 오브젝트의 
    /// </summary>
    public void SaveInstPos()
    {
        position = instPrefab.transform.position;
    }

}
