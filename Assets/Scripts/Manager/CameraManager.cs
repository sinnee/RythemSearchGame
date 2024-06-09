using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float camMoveSpeed = 0.125f;
    public float dragSpeed = 0.004f;

    [SerializeField]
    private bool isCamMove = false;
    private Vector3 dragOrigin;
    private float fixedZ = -10;
    

    //드래그 이동
    void Update()
    {

    }
    
    //카메라 위치를 천천히 이동 (특정 악기 터치 시)
    //isCamMove 변수 사용하여 이미 이동중이라면 함수가 호출되지 않도록
    public void MoveToTarget(Vector3 targetPosition)
    {
        if (!isCamMove)
            StartCoroutine(MoveToPosition(targetPosition));
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        isCamMove = true;
        targetPosition.z = fixedZ; //z축고정하기
        while (Vector3.Distance(transform.position, targetPosition) > 0.001f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, camMoveSpeed);
            transform.position = new Vector3(transform.position.x, transform.position.y, fixedZ);
            yield return null;
        }

        transform.position = new Vector3(targetPosition.x, targetPosition.y, fixedZ);
        isCamMove = false;
    }

    //카메라 드래그 이동
    public void DragCamera(Vector2 dragPosition)
    {
        if (isCamMove)
            return;
        //드래그 방향의 반대로 카메라를 이동
        Vector3 dragDirection = new Vector3(-dragPosition.x, -dragPosition.y, 0);

        Vector3 newPosition = transform.position + dragDirection * dragSpeed;
        newPosition.z = fixedZ; // 새로운 위치의 z축을 고정
        transform.position = newPosition;
    }
}
