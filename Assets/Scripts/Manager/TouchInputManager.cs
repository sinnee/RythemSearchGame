using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputManager : MonoBehaviour
{
    [SerializeField] 
    private Camera mainCamera;
    [SerializeField] 
    private float zoomSpeed = 1.0f;
    
    private bool isDragging = false;
    private Vector2 startMousePos;
    private float mouseStartTime;

    [SerializeField] 
    private float dragHold = 0.1f;

    [SerializeField] 
    private float tapHold = 0.2f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startMousePos = Input.mousePosition;
            mouseStartTime = Time.time;
            isDragging = false;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 currentMousePos = Input.mousePosition;
            if (!isDragging && Vector2.Distance(startMousePos, currentMousePos) >= dragHold)
            {
                isDragging = true;
            }

            if (isDragging)
            {
                Debug.Log("드래그");
                GameManager.Instance.cameraManager.DragCamera((Vector2)Input.mousePosition - startMousePos);
                startMousePos = Input.mousePosition; // 현재 위치를 새로운 시작 위치로 업데이트
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (Time.time - mouseStartTime <= tapHold && !isDragging)
            {
                Debug.Log("터치");
                MouseAndMoveCam();
            }
        }

        if (Input.GetMouseButton(1))
        {
            CameraZoomMode();
        }
    }

    private void MouseAndMoveCam()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Instrument"))
            {
                GameManager.Instance.cameraManager.MoveToTarget(hit.transform.position);

                //애니메이션 실행
                InstrumentController instrumentController = hit.transform.GetComponent<InstrumentController>();
                if (instrumentController != null)
                {
                    //instrumentController.PlayTouchAnimation();
                }
                else
                {
                    //Debug.Log("No Animation");
                }
            }
        }
    }

    private void CameraZoomMode()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        GameManager.Instance.cameraManager.transform.position += Vector3.forward * scroll * zoomSpeed;
    }
}
// using System.Collections;
// using System.Collections.Generic;
// using System.Data.SqlClient;
// using UnityEngine;
//
// public class TouchInputManager : MonoBehaviour
// {
//     [SerializeField] 
//     private float zoomSpeed = 1.0f;
//     
//     private bool isDragging = false;
//     private Vector2 startTouchPos;
//     private float touchStartTime;
//
//     [SerializeField] 
//     private float dragHold = 0.1f;
//
//     [SerializeField] 
//     private float tapHold = 0.2f;
//
//     
//     private void Update()
//     {
//         if (Input.touchCount == 1)
//         {
//             Touch touch = Input.GetTouch(0);
//
//             switch (touch.phase)
//             {
//                 case TouchPhase.Began:
//                     startTouchPos = touch.position;
//                     touchStartTime = Time.time;
//                     isDragging = false;
//                     break;
//                 case TouchPhase.Moved:
//                     //일정거리 이상 이동 -> 드래그 실행
//                     if (!isDragging && Vector2.Distance(startTouchPos, touch.position) >= dragHold)
//                     {
//                         isDragging = true; 
//                     }
//
//                     if (isDragging)
//                     {
//                         Debug.Log("드래그");
//                         GameManager.Instance.cameraManager.DragCamera(touch.deltaPosition);
//                     }
//
//                     break;
//                 case TouchPhase.Ended:
//                     if (Time.time - touchStartTime <= tapHold && !isDragging)
//                     {
//                         TouchAndMoveCam(touch);
//                     }
//
//                     break;
//             }
//         }
//         else if (Input.touchCount == 2)
//         {
//             CameraZoomMode(Input.GetTouch(0), Input.GetTouch(1));
//         }
//     }
//
//
//     private void TouchAndMoveCam(Touch touch)
//     {
//         RaycastHit hit;
//         Ray ray = mainCamera.ScreenPointToRay(touch.position);
//
//         if (Physics.Raycast(ray, out hit))
//         {
//             if (hit.collider.CompareTag("Instrument"))
//             {
//                 GameManager.Instance.cameraManager.MoveToTarget(hit.transform.position);
//
//                 //애니메이션 실행
//                 InstrumentController instrumentController = hit.transform.GetComponent<InstrumentController>();
//                 if (instrumentController != null)
//                 {
//                     instrumentController.PlayTouchAnimation();
//                 }
//                 else
//                 {
//                     Debug.Log("No Animation");
//                 }
//             }
//         }
//     }
//     
//
//     private void CameraZoomMode(Touch firstTouch, Touch secondTouch)
//     {
//         //직전 터치 위치 구하기 : 현재 위치 - 위치 변화량
//         Vector2 firstTouchPrePosition      = firstTouch.position - firstTouch.deltaPosition;
//         Vector2 secondTouchPrePosition     = secondTouch.position - secondTouch.deltaPosition;
//
//         //이전 입력
//         float previousPositionDistance = (firstTouchPrePosition - secondTouchPrePosition).magnitude;
//         
//         //현재 입력
//         float currentPositionDistance = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude;
//
//         //이전-현재의 위치 변화량 * 줌 수치
//         float zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomSpeed;
//
//         //줌 아웃
//         if (previousPositionDistance > currentPositionDistance)
//         {
//             GameManager.Instance.cameraManager.transform.position += Vector3.back * zoomModifier * Time.deltaTime;
//         }
//         //줌 인
//         else if (previousPositionDistance < currentPositionDistance)
//         {
//             GameManager.Instance.cameraManager.transform.position += Vector3.forward * zoomModifier * Time.deltaTime;
//         }
//     }
// }
