using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    /// <summary>
    /// 스테이지당 점수 초기화->StageManager에서 사용
    /// 점수 기록 / 점수 관리 / 현재 점수 리턴 / 특정 점수에 이벤트 트리거
    /// PlayerPref 사용해 점수 저장
    /// </summary>


    private int score;

    public void InitializeScore()
    {
        score = 0;
    }
    
    public void AddScore(int amount)
    {
        score += amount;
        if(score < 0)
        {
            score = 0;
        }
    }

    public int GetScore()
    {
        return score;
    }
}
