using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using UnityEngine;

public class TouchInputManager : MonoBehaviour
{
    [SerializeField] 
    private Transform cameraTransform;

    [SerializeField] 
    private float zoomSpeed = 1.0f;
    // Update is called once per frame
    private void Update()
    {
        
    }

    private void CameraZoomMode()
    {
        //확대 축소만 기능
        if (Input.touchCount != 2)
        {
            return;
        }

        Touch firstTouch      = Input.GetTouch(0);
        Touch secondTouch     = Input.GetTouch(1);

        //직전 터치 위치 구하기 : 현재 위치 - 위치 변화량
        Vector2 firstTouchPrePosition      = firstTouch.position - firstTouch.deltaPosition;
        Vector2 secondTouchPrePosition     = secondTouch.position - secondTouch.deltaPosition;

        //이전 입력
        float previousPositionDistance = (firstTouchPrePosition - secondTouchPrePosition).magnitude;
        
        //현재 입력
        float currentPositionDistance = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude;

        //이전-현재의 위치 변화량 * 줌 수치
        float zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomSpeed;

        //줌 아웃
        if (previousPositionDistance > currentPositionDistance)
        {
            cameraTransform.position += Vector3.back * zoomModifier * Time.deltaTime;
        }
        //줌 인
        else if (previousPositionDistance < currentPositionDistance)
        {
            cameraTransform.position += Vector3.forward * zoomModifier * Time.deltaTime;
        }
    }

    public void ObjectTouch()
    {
        
    }
}
