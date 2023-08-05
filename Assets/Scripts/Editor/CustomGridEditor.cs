using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomGrid))]
public class CustomGridEditor : Editor
{
    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
    static void DrawHandles(CustomGrid grid, GizmoType type)
    {
        if (grid.repositon)
        {
            grid.RefreshPoint();
            grid.repositon = false;
        }

        Handles.DrawLines(grid.verLines);
        Handles.DrawLines(grid.horLines);
    }
}
