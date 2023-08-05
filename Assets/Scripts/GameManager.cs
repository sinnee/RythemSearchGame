using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Ease testEase;
    
    [SerializeField] float dragSpeed = 10.0f;   // 화면 움직임 속도
    private Vector3 firstClickPoint;

    
    public BeatManager beatManager; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //inst select
        if (Input.GetMouseButtonDown(0))
        {
            //checking hit inst
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("hit");
                beatManager.selectedInstIndex=hit.transform.gameObject.GetComponent<InstrumentController>().index;
                Camera.main.transform.DOMove(hit.transform.position + Vector3.back*10,1.0f).SetEase(testEase);
            }


            firstClickPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
        
        if (Input.GetMouseButton(0))
        {
            Vector2 position = Camera.main.ScreenToViewportPoint(Camera.main.ScreenToViewportPoint(Input.mousePosition) - firstClickPoint);
            Vector2 move = position * (Time.deltaTime * dragSpeed);

            Camera.main.transform.Translate(-move);
        }
        
      


    }
    
    
}
