using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridGenerator))]
public class GridGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GridGenerator script = (GridGenerator)target;
        if (GUILayout.Button("Generate"))
        {
            script.GenerateLevel();  
        }
        if (GUILayout.Button("Reset"))
        {
            script.ResetLevel();
        }
        if (GUILayout.Button("Save"))
        {
            script.SaveLevel();
        }

    }
}
