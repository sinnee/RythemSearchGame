using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();
    private static Inventory _inventory;

    void Awake()
    {
        if (_objects == null)
        {
            _inventory = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public static Inventory InvenInstance()
    {
        if (null==_inventory)
        {
            return null;
        }

        return _inventory;
    }

    public void AddObject(GameObject newObject)
    {
        newObject.gameObject.TryGetComponent(out InteractiveObject io);
        if (io == null) return;
        _objects.Add(io.getId(),newObject);
        Debug.Log(io.gameObject.name);
        
    }

    public GameObject PutObject(int id)
    {
        GameObject output = null;
        if (_objects.ContainsKey(id))
        {
            output = _objects[id];
            _objects.Remove(id);
        }
        return output;
    }
}
