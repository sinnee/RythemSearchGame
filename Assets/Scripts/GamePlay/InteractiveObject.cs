using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    [SerializeField]private bool isKeepable;
    [SerializeField] private bool isTargetPosition;
    [SerializeField]private int id;

    private bool isInteracted = false;

    public int getId()
    {
        return id;
    }

    public bool checkIsKeepable()
    {
        if (isInteracted) return false;
        return isKeepable;
    }

    public bool checkIsTargetPosition()
    {
        return isTargetPosition;
    }

    public void Interact()
    {
        isInteracted = true;
    }
    
}
