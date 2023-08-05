using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CustomGrid : MonoBehaviour
{
    public CustomGridConfig config;

    public Dictionary<Vector2Int, MapObject> Items = new Dictionary<Vector2Int, MapObject>();

    public Vector3[] horLines;
    public Vector3[] verLines;

    public bool repositon;


    //필드 수정시
    private void OnValidate()
    {
        repositon = true;    
    }

    public Vector2Int GetCellPos(Vector3 worldPos)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPos.x / config.CellSize.x), Mathf.FloorToInt(worldPos.y / config.CellSize.y));
    }
    public  Vector3 GetWorldPos(Vector2Int cellPos)
    {
        return new Vector3(cellPos.x * config.CellSize.x + config.CellSize.x * 0.5f, cellPos.y * config.CellSize.y + config.CellSize.y * 0.5f, 0);
    }

    public void RetreiveAll()
    {
        Items.Clear();

        var mapObjs = FindObjectsOfType<MapObject>();
        if (mapObjs != null)
        {
            for (int i = 0; i < mapObjs.Length; i++)
            {
                Items.Add(mapObjs[i].cellPos, mapObjs[i]);
            }
        }
    }

    public void RefreshPoint()
    {
        int verLineCnt = config.CellCount.x * 2 + 2;
        int horLineCnt = config.CellCount.y * 2 + 2;

        horLines = new Vector3[horLineCnt];
        verLines = new Vector3[verLineCnt];

        for (int i = 0; i <= config.CellCount.x; i++)
        {
            verLines[i * 2] = new Vector3(i*config.CellSize.x,config.CellSize.y*config.CellCount.y,0);
            verLines[i * 2 + 1] = new Vector3(i * config.CellSize.x, 0, 0);
        }

        for (int i = 0; i <= config.CellCount.y; i++)
        {
            horLines[i * 2] = new Vector3(0, i * config.CellSize.y, 0);
            horLines[i * 2 + 1] = new Vector3(config.CellSize.x * config.CellCount.x, i * config.CellSize.y, 0);
        }
    }

    public bool Contains(Vector2Int cellPos)
    {
        return cellPos.x >= 0 && cellPos.x < config.CellCount.x && cellPos.y >= 0 && cellPos.y < config.CellCount.y;
    }

    public bool IsItemExist(Vector2Int cellPos)
    {
        return Items.ContainsKey(cellPos);
    }

    public MapObject GetItem(Vector2Int cellPos)
    {
        if (Items.ContainsKey(cellPos)==false)
        {
            return null;
        }

        return Items[cellPos];
    }
    public MapObject AddItem(Vector2Int cellPos, CustomGridPaletteItem paletteItem)
    {
        if (Items.ContainsKey(cellPos))
        {
            Debug.LogError("already exist!!! delete first!!");
            return null;
        }
        var target = GameObject.Instantiate(paletteItem.targetObject, transform );
        target.transform.position = GetWorldPos(cellPos);
        var comp = target.AddComponent<MapObject>();
        comp.id = paletteItem.id;
        comp.cellPos = cellPos;

        Items.Add(cellPos, comp);

        return comp;
    }


    public void RemoveItem(Vector2Int cellPos)
    {
        if (Items.ContainsKey(cellPos))
        {
            Items.Remove(cellPos);
        }
    }


    public byte[] Serialize()
    {
        byte[] bytes = null;
        using (var ms =new MemoryStream())
        {
            using(var writer = new BinaryWriter(ms))
            {
                writer.Write(Items.Count);

                foreach (var item in Items)
                {
                    writer.Write(item.Key.x);
                    writer.Write(item.Key.y);
                    writer.Write(item.Value.id);
                    Debug.Log("[debug]value id : " + item.Value.id);


                }

                bytes = ms.ToArray();
            }
        }

        return bytes;
    }


    public void Import(byte[] buffer, CustomGridPalette targetPalette)
    {

        foreach (var item in Items)
        {
            GameObject.DestroyImmediate(item.Value.gameObject); 

        }

        Items.Clear();

        using(var ms = new MemoryStream(buffer))
        {
            using(var reader = new BinaryReader(ms))
            {
                int count = reader.ReadInt32();

                for (int i = 0; i < count; i++)
                {
                    var xPos = reader.ReadInt32();
                    var yPos = reader.ReadInt32();
                    var id = reader.ReadInt32();

                    var pos = new Vector2Int(xPos, yPos);
                    AddItem(pos, targetPalette.GetItem(id));
                }
            }
        }
    }
}
