using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(GridCellProperties))]
public class GridCellPropertiesEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GridCellProperties script = (GridCellProperties)target;

        script.flags = (GridCellProperties.props)EditorGUILayout.EnumFlagsField(script.flags);

        if (GUILayout.Button("Print"))
        {
            Debug.Log(script.flags);
        }
    }
}
