using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static GridGenerator;

[CustomEditor(typeof(GridCellProperties))]
[CanEditMultipleObjects]
public class GridCellPropertiesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();
        GridCellProperties script = (GridCellProperties)target;
        script.flags = (BlockStatus)EditorGUILayout.EnumFlagsField(script.flags);
        EditorUtility.SetDirty(script);
        if (EditorGUI.EndChangeCheck())
        {
            /*manual assignment here, remember to check that the selected objects
              are in fact of the appropriate type.*/
            foreach (Object obj in targets)
            {
                ((GridCellProperties)obj).flags = script.flags;
                EditorUtility.SetDirty(((GridCellProperties)obj));
            }

        }
    }
}
