using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StageManager : MonoBehaviour
{
    /// <summary>
    /// 스테이지 선택창에서의 스테이지 정보를 보여주는 기능
    /// 최고점수, 클리어 상황 등등
    /// </summary>
    /// 
    private Dictionary<string, int> highestScores = new Dictionary<string, int>();

    public void SetHighScore(string stageName, int score)
    {
        if (highestScores.ContainsKey(stageName))
        {
            if (score > highestScores[stageName])
            {
                highestScores[stageName] = score;
            }
        }
        else
        {
            highestScores.Add(stageName, score);
        }

        SaveData();
    }

    public int GetHighScore(string stageName)
    {
        if (highestScores.ContainsKey(stageName))
        {
            return highestScores[stageName];
        }
        else
        {
            return 0;
        }
    }

    private void SaveData()
    {
        string json = JsonUtility.ToJson(this);
        File.WriteAllText(Application.persistentDataPath + "/stageData.json", json);
    }

    private void LoadData()
    {
        string path = Application.persistentDataPath + "/stageData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }
}
