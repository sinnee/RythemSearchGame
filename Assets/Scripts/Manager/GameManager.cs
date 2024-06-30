using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private Vector3 firstClickPoint;

    public ScoreManager scoreManager;
    public StageManager stageManager;
    public InstrumentManager instrumentManager;
    public float dragSpeed = 10.0f; // 화면 움직임 속도
    public Ease cameraMoveEase;
    public BeatManager beatManager;
    public CameraManager cameraManager;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = FindObjectOfType<GameManager>();
                if (obj != null)
                {
                    _instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<GameManager>();
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        scoreManager = new ScoreManager();
        
        
        var objs = FindObjectsOfType<GameManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}