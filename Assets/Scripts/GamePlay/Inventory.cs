using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    private Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();
    private List<int> _id;
    private static Inventory _inventory;

    [SerializeField] private GameObject standardGameObject;

    private int size = 0;
    private int pointing = 0;
    
    void Awake()
    {
        if (_inventory == null)
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
        if (object.ReferenceEquals(null,_inventory))
        {
            return null;
        }

        return _inventory;
    }

    public void AddObject(GameObject newObject)
    {
        newObject.gameObject.TryGetComponent(out InteractiveObject io);
        if (object.ReferenceEquals(io,null)) return;

        GameObject temp = Instantiate(newObject) as GameObject;
        temp.SetActive(false);
        temp.transform.SetParent(gameObject.transform);
        temp.transform.position.Set(0,0,0);
        temp.transform.localScale.Set(50,50,50);
        
        _objects.Add(io.getId(),temp);
        size = _objects.Count;
        _id = _objects.Keys.ToList();
        Debug.Log(io.gameObject.name);
        
    }

    public void changePointing()
    {
        if (size <= 0)
        {
            standardGameObject.SetActive(true);
            return;
        }
        _objects[_id[pointing]].SetActive(false);
        pointing++;
        if (pointing >= size) pointing = 0;
        _objects[_id[pointing]].SetActive(true);
    }


    public GameObject PutObject(int id)
    {
        GameObject output = null;
        Debug.Log(id);
        if (id-1000 == _id[pointing])
        {
            output = _objects[_id[pointing]];
            _id.Remove(pointing);
            
            output.transform.SetParent(null);
            output.transform.localScale.Set(1,1,1);
            
            size = _objects.Count;
            changePointing();
        }

        return output;
    }
    
    
}
