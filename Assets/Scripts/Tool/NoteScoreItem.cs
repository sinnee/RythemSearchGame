using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NoteScoreItem
{
    public Dictionary<int, int> scoreDic;
    public GameObject instrument;

    public NoteScoreItem(List<int[]> scoreArray, GameObject instrument)
    {
        this.instrument = instrument;
    }
    public NoteScoreItem()
    {
        scoreDic = new Dictionary<int, int>();

        //기본 악기?
        //this.instrument = instrument; 
    }

}
