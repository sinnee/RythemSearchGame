using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class HighScoreData
{
    private Dictionary<string, int> highestScores = new Dictionary<string, int>();
}
public class StageManager : MonoBehaviour
{
    /// <summary>
    /// 스테이지 선택창에서의 스테이지 정보를 보여주는 기능
    /// 최고점수, 클리어 상황 등등
    /// </summary>
    /// 
    private HighScoreData highScoreData = new HighScoreData();
    

    public void SetHighScore(string stageName, int score)
    {
        if(highestScores.ContainsKey(stageName)) 
        {
            if(score > highestScores[stageName])
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
        if(highestScores.ContainsKey(stageName))
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
        string filePath = Path.Combine(Application.persistentDataPath, "stageData.json");
        File.WriteAllText(filePath, json);
    }

    private void LoadData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "stageData.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            highScoreData = JsonUtility.FromJson<HighScoreData>(json);
        }
    }
}
