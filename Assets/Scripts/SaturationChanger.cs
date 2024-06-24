using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saturationchanger : MonoBehaviour
{
    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    private float splitValue;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _propBlock = new MaterialPropertyBlock();

        splitValue  = 0f;
    }

    public void ChangeSaturation(float proportion){
        splitValue = proportion;
        _propBlock.SetFloat("_Split_Value", splitValue);
        _renderer.SetPropertyBlock(_propBlock);
    }
}
