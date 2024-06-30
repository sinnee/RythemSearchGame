using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "BeatMaker/Create Score")]
public class NoteScore : ScriptableObject
{
    
    private const float NOTE_SEPARATION = 16f;
    public OrderedDictionary  noteTable;
    public int selectedIndex; 


    public List<NoteScoreItem> instItems=new List<NoteScoreItem>();
    private int bpmValue;
    public int BPMValue
    {
        get { return bpmValue; }
        set
        {
            if (value > 0)
                bpmValue = value;
            else
                bpmValue = 0;
        }
    }

    public int BarCountValue;
    

    private int noteValue;

    public int NoteValue
    {
        get { return noteValue; }
        set
        {
            
        }

    }
    
    public int NoteCountValue;

    private void OnEnable()
    {
        noteTable = new OrderedDictionary();
        noteTable.Add("Whole Note", 4.0f);
        noteTable.Add("Dotted Half Note", 3.0f);
        noteTable.Add("Half Note", 2.0f);
        noteTable.Add("Dotted Quarter Note", 1.5f);
        noteTable.Add("Quarter Note", 1f);
        noteTable.Add("Dotted Eighth Note", 0.75f);
        noteTable.Add("Eighth Note", 0.5f);
        noteTable.Add("Dotted Sixteenth Note", 0.375f);
        noteTable.Add("Sixteenth Note", 0.25f);
    }
    
    public int Scale()
    {
        return (int)((float)noteTable[selectedIndex]*NOTE_SEPARATION);
    }

    public float CaluBtnTime()
    {
        return (60f / BPMValue) / NOTE_SEPARATION;
    }

    /// <summary>
    /// 악기 오브젝트 생성
    /// </summary>
    public void MakeInstruments()
    {
        foreach (var VARIABLE in instItems)
        {
            VARIABLE.MakeInstrument();
        }
    }

    /// <summary>
    /// 악보 연주 시간 반환
    /// </summary>
    public float GetNotePlayingTime()
    {
        return CaluBtnTime() * NoteCountValue * BarCountValue;
    }
}
