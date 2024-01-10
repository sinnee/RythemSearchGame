using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentManager : MonoBehaviour
{
    /// <summary>
    /// 씬에 존재하는 악기를 선택할 경우, Play 상태값을 입력
    /// 악기를 일정 이상 연주하면 Finale 상태값을 입력
    /// 연주 완료하면 Finish 상태값을 입력
    /// </summary>
    private Dictionary<string, InstrumentState> instrumentStates = new Dictionary<string, InstrumentState>();

    private void Awake()
    {
        
    }

    /// <summary>
    /// 악기 이름 / 원하는 악기 상태 입력. 
    /// Dict에 존재하지 않으면 해당 상태를 가진채로 저장
    /// 이미 존재하는 경우 상태를 업데이트
    /// </summary>
    /// <param name="instrumentName"></param>
    /// <param name="state"></param>
    public void SetInstrumentState(string instrumentName, InstrumentState state)
    {
        if(!instrumentStates.ContainsKey(instrumentName))
        {
            instrumentStates.Add(instrumentName, state);
        }
        else
        {
            instrumentStates[instrumentName] = state;
        }
    }

    /// <summary>
    /// 악기 현재 상태를 리턴
    /// InstrumentStates에 존재하는 키인 경우 상태를 리턴
    /// </summary>
    /// <param name="instrumentName"></param>
    /// <returns></returns>
    public InstrumentState GetInstrumentState(string instrumentName)
    {
        if(instrumentStates.ContainsKey(instrumentName))
        {
            return instrumentStates[instrumentName];
        }
        else
        {
            return InstrumentState.Play;
        }
    }
}
