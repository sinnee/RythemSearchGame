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
    public int selectedInstIndex;

    private InstrumentController[] instContrlArray;
    
    public Vector3 initBeatBarPostion = new Vector3(-173, 0, 0);
    private Vector3 judgeLine;
    private Vector3 endLine;
    private float startJudgeDistance;
    private float judgeEndDistance;
    private Queue<GameObject> beatBarQueue=new Queue<GameObject>();
    private bool isNothing=false;

    private int playingIndex;
    public int PlayingIndex
    {
        get
        {
            return playingIndex;
        }
        set
        {
            if (value>instContrlArray[0].instrumentBeatList.Count-1)
            {
                playingIndex = instContrlArray[0].instrumentBeatList.Count-1;
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
    bool isPlayingFinish = false;
    void Start()
    {
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
        selectedInstIndex = 999;
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
        judgeLine = new Vector3(uiRectTransform.rect.width-leftMargin,uiRectTransform.transform.position.y,0);
        endLine = new Vector3(uiRectTransform.rect.width+leftMargin,uiRectTransform.transform.position.y,0);
        startJudgeDistance= Vector3.Distance(initBeatBarPostion, judgeLine);
        judgeEndDistance = Vector3.Distance(judgeLine, endLine);
        judgeLineBeatBar.transform.position = judgeLine;
        
        isPlayingFinish = false;
        
    }

    
    
    // Update is called once per frame
    void Update()
    {
        if (selectedInstIndex == 999)
        {
            return;
        }
        
        //draw Line
        Debug.DrawLine(judgeLine-Vector3.up*100,judgeLine+Vector3.up*100,Color.green);
        Debug.DrawLine(endLine-Vector3.up*100,endLine+Vector3.up*100,Color.yellow);
        
        
        
        //Create Beat!
        if ((currentTime>=((instContrlArray[selectedInstIndex].instrumentBeatList[PlayingIndex].Item1)-beatMoveDuration))&&!isPlayingFinish)
        {
            //Debug.Log($"real time is {currentTime} \nplayingIndex : {playingIndex} is playing when {instContrlArray[0].instrumentBeatList[PlayingIndex].Item1} ");
            CreateBeatBar();
            
        }
        
        //input check
        if (Input.GetKeyDown(KeyCode.Space))
        {
            judgeLineBeatBar.transform.DOScale(1.5f, 0.5f).From();
            
            float timingDifference = Mathf.Abs(currentTime - instContrlArray[selectedInstIndex].instrumentBeatList[JudgeIndex].Item1);

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
                isNothing = true;
            }

            if (!isNothing)
            {
                removeBeatBar();
                JudgeIndex++;
                isNothing = false;
            }

        }
        //fail check
        else if(currentTime > instContrlArray[selectedInstIndex].instrumentBeatList[JudgeIndex].Item1+BAD_RANGE)
        {
            Debug.Log($"judge index : {judgeIndex}\n current time : {currentTime}");
            Debug.Log("fail!");
            removeBeatBar();
            JudgeIndex++;
        }
        
        //score reset
        if (currentTime>instContrlArray[selectedInstIndex].instrumentBeatList[PlayingIndex].Item1+BAD_RANGE)
        {
            playingIndex = 0; judgeIndex = 0; currentTime =0;
            currentTime-=beatMoveDuration;
            isPlayingFinish = false;
        }
        
        currentTime += Time.deltaTime;
    }
    
    #region BeatBarPool
    void CreateBeatBar()
    {
        GameObject beatBar=GetBeatBarFromPool(initBeatBarPostion);
        
        var tween = beatBar.transform.DOMove(judgeLine, beatMoveDuration).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                beatBar.transform.DOMove(endLine, ((judgeEndDistance*beatMoveDuration)/startJudgeDistance)).SetEase(Ease.Linear);
            });

        beatBarQueue.Enqueue(beatBar);
        PlayingIndex++;
        return;
    }
    void removeBeatBar()
    {
        try
        {
            var beatBar = beatBarQueue.Dequeue();
            ReturnBeatBarToPool(beatBar);
            DOTween.Kill(beatBar.transform);
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
        beatBar.SetActive(false);
    }
    #endregion
    
    private void AddScore(int score)
    {
        
    }
}
