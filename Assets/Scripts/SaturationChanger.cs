using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSVchanger : MonoBehaviour
{
    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    private float value;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _propBlock = new MaterialPropertyBlock();

        value  = 0f;
    }

    public void SaturationChange(float v){
        value = v;
        _propBlock.SetFloat("_Split_Value", value);
        _renderer.SetPropertyBlock(_propBlock);
    }
}
