using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float camMoveSpeed = 0.125f;

    private bool isCamMove;
    private Vector3 dragOrigin;

    //드래그 이동
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    dragOrigin = Camera.main.ScreenToWorldPoint(touch.position);
                    dragOrigin.z = 0;
                    break;
                
                case TouchPhase.Moved:
                    Vector3 currentPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    currentPosition.z = 0;
                    Vector3 difference = dragOrigin - currentPosition;

                    transform.position += difference;
                    break;
                    
            }
        }
    }
    
    //카메라 위치를 천천히 이동 (특정 악기 터치 시)
    //InstrumentController에서 호출해 사용, isCamMove 변수 사용하여 이미 이동중이라면 함수가 호출되지 않도록
    public void MoveToTarget(Vector3 targetPosition)
    {
        Vector3 tempPosition = targetPosition;
        Vector3 moveCamPosition = Vector3.Lerp(transform.position, tempPosition, camMoveSpeed);

        transform.position = moveCamPosition;
    }
}
