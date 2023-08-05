using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomGrid/Create Palette")]
public class CustomGridPalette : ScriptableObject
{
    public List<CustomGridPaletteItem> items;

    public CustomGridPaletteItem GetItem(int id)
    {
        return items.Find(t => t.id == id);
    }
}
