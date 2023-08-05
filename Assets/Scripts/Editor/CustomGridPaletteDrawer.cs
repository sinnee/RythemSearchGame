using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGridPaletteDrawer
{
    public CustomGridPalette TargetPalette;
    Vector2 slotSize = new Vector2(100, 100);
    Vector2 scrollPos;

    int selectedIdx;

    public CustomGridPaletteItem SeletedItem
    {
        get
        {
            if (selectedIdx == -1)
            {
                return null;
            }

            return TargetPalette.items[selectedIdx];
        }
    }

    public void Draw(Vector2 winSize)
    {
        if (TargetPalette == null || TargetPalette.items.Count == 0)
        {
            EditorHelper.DrawCenterLabel(new GUIContent("데이터 없음"), Color.red, 15, FontStyle.Bold);
            return;
        }

        if (selectedIdx == -1)
        {
            selectedIdx = 0;
        }

        scrollPos = EditorHelper.DrawGridItems(scrollPos, 10, TargetPalette.items.Count, winSize.x, slotSize, (idx) =>
        {
            bool selected = CustomGridPaletteItemDrawer.Draw(slotSize, selectedIdx == idx, TargetPalette.items[idx]);

            if (selected)
            {
                selectedIdx = idx;
            }
        });
    }
}
