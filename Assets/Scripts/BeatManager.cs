using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Policy;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class BeatManager : MonoBehaviour
{
    private readonly int PERFECT_SCORE=10;
    private readonly int NORMAL_SCORE = 5;
    private readonly int BAD_SCORE = 1;
    private readonly int MISS_SCORE = 0;
    private readonly float PERFECT_RANGE= 0.1f;
    private readonly float NORMAL_RANGE=0.2f;
    private readonly float BAD_RANGE=0.5f;
    private readonly float MISS_RANGE=1f;

    public GameObject judgeLineBeatBar;
    public Transform beatBarPoolInCanvas;
    public int poolSize=10;
    public GameObject[] taggedObjects;
    public GameObject beatBarPrefab;
    public List<GameObject> _beatBarPool;
    public RectTransform uiRectTransform;
    public float leftMargin;
    public float beatMoveDuration;
    public NoteScore noteScore;

    private InstrumentController[] instContrlArray;
    
    public Vector3 initBeatBarPostion = new Vector3(-173, 0, 0);
    private Vector3 _judgeLine;
    private Vector3 _endLine;
    private float _startJudgeDistance;
    private float _judgeEndDistance;
    private Queue<GameObject> beatBarQueue=new Queue<GameObject>();
    private bool _isNothing=false;
    private bool _isSelectedInstrument;
    private List<float> _playingBeatBarList = new List<float>();
    
    private int selectedInstIndex;
    public int SelectedInstIndex
    {
        get => selectedInstIndex;
        set
        {
            ReturnAllBeatBarToPool();
            CalcuPreCreateBeatBar();
            selectedInstIndex = value;
        }
    }

    private int playingIndex;
    public int PlayingIndex
    {
        get => playingIndex;
        set
        {
            if (value>instContrlArray[SelectedInstIndex].instrumentBeatList.Count-1)
            {
                //초기화가 될 때까지 마지막 index 유지
                playingIndex = instContrlArray[SelectedInstIndex].instrumentBeatList.Count-1;
                isPlayingFinish = true;
            }
            else
            {
                playingIndex = value;
            }
        }
    }
    private int judgeIndex;
    public int JudgeIndex
    {
        get
        {
            return judgeIndex;
        }
        set
        {
            if (value>instContrlArray[0].instrumentBeatList.Count-1)
            {
                judgeIndex = instContrlArray[0].instrumentBeatList.Count-1;
                
            }
            else
            {
                judgeIndex = value;
            }
        }
    }
    float currentTime;
    bool isPlayingFinish;
    
    void Start()
    {
        //noteScore.MakeInstruments();
        
        //pool init
        _beatBarPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject beatBar = Instantiate(beatBarPrefab);
            beatBar.transform.SetParent(beatBarPoolInCanvas);
            beatBar.name = $"beat {i}";
            beatBar.SetActive(false);
            _beatBarPool.Add(beatBar);
        }
        
        //inst init
        taggedObjects = GameObject.FindGameObjectsWithTag("Instrument");
        instContrlArray = new InstrumentController[taggedObjects.Length];
        for (int i = 0; i < taggedObjects.Length; i++)
        {
            instContrlArray[i] = taggedObjects[i].GetComponent<InstrumentController>();
            instContrlArray[i].index = i;
        }
        
        //beat judge init
        playingIndex = 0; judgeIndex = 0;
        currentTime -=beatMoveDuration;
        initBeatBarPostion += uiRectTransform.transform.position;
        _judgeLine = new Vector3(uiRectTransform.rect.width-leftMargin,uiRectTransform.transform.position.y,0);
        _endLine = new Vector3(uiRectTransform.rect.width+leftMargin,uiRectTransform.transform.position.y,0);
        _startJudgeDistance= Vector3.Distance(initBeatBarPostion, _judgeLine);
        _judgeEndDistance = Vector3.Distance(_judgeLine, _endLine);
        judgeLineBeatBar.transform.position = _judgeLine;
        
        isPlayingFinish = false;
        
    }

    void Update()
    {
        if (!_isSelectedInstrument)
        {
            return;
        }
        
        //draw Line
        Debug.DrawLine(_judgeLine-Vector3.up*100,_judgeLine+Vector3.up*100,Color.green);
        Debug.DrawLine(_endLine-Vector3.up*100,_endLine+Vector3.up*100,Color.yellow);
        
        
        
        Debug.Log($"selectedInstIndex : {SelectedInstIndex} / playingIndex : {playingIndex}");
        //Create Beat!
        if ((currentTime>=((instContrlArray[SelectedInstIndex].instrumentBeatList[PlayingIndex].Item1)-beatMoveDuration))&&!isPlayingFinish)
        {
            //Debug.Log($"real time is {currentTime} \nplayingIndex : {playingIndex} is playing when {instContrlArray[0].instrumentBeatList[PlayingIndex].Item1} ");
            CreateBeatBar();
            
        }

        HandlePlayerInput();
        
        //악보 처음 부터 시작
        if (currentTime>instContrlArray[SelectedInstIndex].instrumentBeatList[PlayingIndex].Item1+BAD_RANGE)
        {
            playingIndex = 0; judgeIndex = 0; currentTime =0;
            currentTime-=beatMoveDuration;
            isPlayingFinish = false;
        }
        
        currentTime += Time.deltaTime;
    }

    /// <summary>
    /// 미리 생성해야되는 비트바 목록 확인
    /// 비트바 이동 시간 때문에 비트바는 미리 생성되어야 한다
    /// </summary>
    private void CalcuPreCreateBeatBar()
    {
        _playingBeatBarList.Clear();
        Queue<float> preBeatbarQueue = new Queue<float>();
        float preMakeTime;
        
        foreach (var VARIABLE in instContrlArray[SelectedInstIndex].instrumentBeatList)
        {
            preMakeTime = VARIABLE.Item1 - beatMoveDuration;
            _playingBeatBarList.Add(preMakeTime);
            if (preMakeTime < 0)
            {
                preBeatbarQueue.Enqueue(noteScore.GetNotePlayingTime()+preMakeTime);
            }
        }

        for (int i = 0; i < preBeatbarQueue.Count; i++)
        {
            _playingBeatBarList.Add(preBeatbarQueue.Dequeue());
        }
        Debug.Log("end");
    }
 
    /// <summary>
    /// 몇번째 박을 연주해야되는지 셋팅
    /// </summary>
    private void SetPlayingIndex()
    {
        
    }

    /// <summary>
    /// 플레이어 입력 처리
    /// </summary>
    private void HandlePlayerInput()
    {
        //input check
        if (Input.GetKeyDown(KeyCode.Space))
        {
            judgeLineBeatBar.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.1f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => ResetButtonScale());
            
            
            float timingDifference = Mathf.Abs(currentTime - instContrlArray[SelectedInstIndex].instrumentBeatList[JudgeIndex].Item1);

            if (timingDifference <= PERFECT_RANGE) //perfect 타이밍
            {
                Debug.Log("Perfect!");
                GameManager.Instance.scoreManager.AddScore(PERFECT_SCORE);
            }
            else if (timingDifference <= NORMAL_RANGE) //normal 타이밍
            {
                Debug.Log("normal!");
                GameManager.Instance.scoreManager.AddScore(NORMAL_SCORE);
            }
            else if (timingDifference <= BAD_RANGE) //bad 타이밍
            {
                Debug.Log("bad!");
                GameManager.Instance.scoreManager.AddScore(BAD_SCORE);
            }
            else if(timingDifference<= MISS_RANGE)//Miss 타이밍
            {
                Debug.Log("Miss!");
                GameManager.Instance.scoreManager.AddScore(MISS_SCORE);
            }
            else//NOTHING 타이밍, 버튼 클리 시 판정 영역에 도달조차 안한 단계 
            {
                Debug.Log("Nothing");
                _isNothing = true;
            }

            if (!_isNothing)
            {
                removeBeatBar();
                JudgeIndex++;
                _isNothing = false;
            }

        }
        //fail check
        else if(currentTime > instContrlArray[SelectedInstIndex].instrumentBeatList[JudgeIndex].Item1+BAD_RANGE)
        {
            Debug.Log($"judge index : {judgeIndex}\n current time : {currentTime}");
            Debug.Log("fail!");
            removeBeatBar();
            JudgeIndex++;
        }
    }
    
    void ResetButtonScale()
    {
        // Reset the button scale after the animation is complete
        judgeLineBeatBar.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.InQuad);
    }

    /// <summary>
    /// 현재 재생 중인 악보의 시간을 확인하여 앞으로 재생되야할 playingIndex 값을 설정한다.
    /// </summary>
    private void SetCurrentPlayingIndex()
    {
        
    }
    
    #region BeatBarPool
    void CreateBeatBar()
    {
        GameObject beatBar=GetBeatBarFromPool(initBeatBarPostion);
        
        var tween = beatBar.transform.DOMove(_judgeLine, beatMoveDuration).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                beatBar.transform.DOMove(_endLine, ((_judgeEndDistance*beatMoveDuration)/_startJudgeDistance)).SetEase(Ease.Linear);
            });

        beatBarQueue.Enqueue(beatBar);
        PlayingIndex++;
    }
    
    void removeBeatBar()
    {
        try
        {
            var beatBar = beatBarQueue.Dequeue();
            ReturnBeatBarToPool(beatBar);
            
        }
        catch (InvalidOperationException)
        {
            Debug.LogWarning("Queue is empty. Cannot dequeue.");
        }
    }
    
    public GameObject GetBeatBarFromPool(Vector3 position)
    {
        foreach (GameObject beatBar in _beatBarPool)
        {
            if (!beatBar.activeInHierarchy)
            {
                beatBar.transform.position = position;
                beatBar.SetActive(true);
                
                return beatBar;
            }
        }

        // 풀에 사용 가능한 총알이 없으면 새로 생성
        GameObject newBeatBar = Instantiate(beatBarPrefab, position, new Quaternion());
        newBeatBar.transform.SetParent(beatBarPoolInCanvas);
        _beatBarPool.Add(newBeatBar);
        
        return newBeatBar;
    }

    public void ReturnBeatBarToPool(GameObject beatBar)
    {
        Debug.Log($"{beatBar.name} is return to pool");
        DOTween.Kill(beatBar.transform);
        beatBar.SetActive(false);
    }

    public void ReturnAllBeatBarToPool()
    {
        foreach (var VARIABLE in _beatBarPool)
        {
            ReturnBeatBarToPool(VARIABLE);
        }   
    }
    #endregion
    
}
