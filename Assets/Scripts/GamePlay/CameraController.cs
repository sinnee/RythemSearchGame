using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float dragSpeed = 3.0f;
    [SerializeField] LayerMask objectLayer;

    private bool isDragging = false;
    private bool isTargeting = false;

    private int targetId;
    Vector2 prevPoint;
    private Ray ray;
    RaycastHit hit;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit,objectLayer)&&hit.transform.CompareTag("InteractiveObject"))
            {
                hit.transform.TryGetComponent(out InteractiveObject io);
                targetId = io.getId();
                isTargeting = true;
                Debug.Log("ObjectDown");
            }
            else
            {
                prevPoint = Input.mousePosition;
                isDragging = true;
                Debug.Log("DragDown");
            }
        }

        if (Input.GetMouseButton(0)) {
            if(isDragging)
            {
                Vector3 mp = Input.mousePosition;
                Vector3 dir
                    = Camera.main.ScreenToViewportPoint((Vector2)mp - prevPoint).normalized;
                dir.z = dir.y;
                dir.y = 0;


                Vector3 move = dir * (Time.deltaTime * dragSpeed);

                gameObject.transform.position -= move;
                prevPoint = Input.mousePosition;
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            if (isTargeting)
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, objectLayer)&&hit.transform.TryGetComponent(out InteractiveObject io)&&io.getId()==targetId)
                {
                    if (io.checkIsKeepable())
                    {
                        Debug.Log("KE");
                        Inventory.InvenInstance().AddObject(hit.collider.gameObject); 
                        Destroy(hit.collider.gameObject);
                    }
                    else if (io.checkIsTargetPosition())
                    {
                        GameObject targetItem = Inventory.InvenInstance().PutObject(io.getId());
                        if (!object.ReferenceEquals(targetItem, null))
                        {
                            targetItem.transform.position = io.transform.position;
                            targetItem.TryGetComponent(out InteractiveObject ioTemp);
                            ioTemp.Interact();
                        }
                    }
                    else
                    {
                        Destroy(hit.collider.gameObject);
                    }
                }
                
            }

            isTargeting = false;
            isDragging = false;
        }
    
    }

}
