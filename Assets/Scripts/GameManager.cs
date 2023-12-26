using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    private Vector3 firstClickPoint;
    
    [SerializeField] 
    public float dragSpeed = 10.0f;   // 화면 움직임 속도
    public Ease cameraMoveEase;
    public BeatManager beatManager; 
   
    void Update()
    {
        //악기 선택 시 해당 위치 카메라 이동
        if (Input.GetMouseButtonDown(0))
        {
            //checking hit inst
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("hit");
                beatManager.selectedInstIndex=hit.transform.gameObject.GetComponent<InstrumentController>().index;
                Camera.main.transform.DOMove(hit.transform.position + Vector3.back*10,1.0f).SetEase(cameraMoveEase);
            }

            firstClickPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
    }
}
