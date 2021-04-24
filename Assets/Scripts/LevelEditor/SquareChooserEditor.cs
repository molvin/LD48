using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SquareChooser))]
public class SquareChooserEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SquareChooser script = (SquareChooser)target;
        if (GUILayout.Button("Reload"))
        {
            script.readFromDirectory();
        }
        GUIContent list = new GUIContent("MyList");
        script.listIdx = EditorGUILayout.Popup(list, script.listIdx, script.MyList.ToArray());
    }
}
