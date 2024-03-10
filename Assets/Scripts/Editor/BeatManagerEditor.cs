using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(BeatManager))]
public class BeatManagerEditor : Editor
{
    private BeatManager _targetRef;
    
  

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("악기 위치 저장"))
        {
            SaveInstsPos();    
        }
        
        base.OnInspectorGUI();    
    }

    /// <summary>
    /// 악기 오브젝트 위치 정보 저장
    /// </summary>
    private void SaveInstsPos()
    {
        foreach (var VARIABLE in _targetRef.noteScore.instItems)
        {
            VARIABLE.SaveInstPos();
        }
    }
}
