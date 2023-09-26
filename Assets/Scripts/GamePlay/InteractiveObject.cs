using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    [SerializeField]private bool isKeepable;
    [SerializeField] private int id;

    private bool isActive = true;

    public int getId()
    {
        return id;
    }

    public bool checkIsKeepable()
    {
        return isKeepable;
    }

    
}
