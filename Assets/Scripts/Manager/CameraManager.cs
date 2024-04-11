using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private bool isCamMove;
    private bool isPlayerControl;

    public void canMoveCam()
    {
        isCamMove = true;
    }

    public void cantMoveCam()
    {
        isCamMove = false;
    }

    public void canPlayerControl()
    {
        isPlayerControl = true;
    }

    public void cantPlayerControl()
    {
        isPlayerControl = false;
    }

    public void InstrumentCameraMove()
    {
        
    }
}
