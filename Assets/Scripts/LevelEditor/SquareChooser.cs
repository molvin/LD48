using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEditor;


public class SquareChooser : MonoBehaviour
{
    public string prefabFolderPath;
    private string path
    {
        get => Application.dataPath + "/" + prefabFolderPath;
    }
    [HideInInspector]
    private int _listIdx;
    public int listIdx
    {
        get => _listIdx;
        set
        {
            _listIdx = value;
            onListIndexChange();
        }
    }
    [HideInInspector]
    public List<string> MyList = new List<string>();

    public void selectFirst()
    {
        readFromDirectory();
        listIdx = 0;
    }

    protected void onListIndexChange ()
    {
        if (!Application.isEditor)
            return;
        for (int i = transform.childCount - 1; i > -1; i--)
        {
            Object.DestroyImmediate(transform.GetChild(i).gameObject);
        }
        GameObject ob = PrefabUtility.InstantiatePrefab((GameObject)Resources.Load(getSelectedObjectFilePath()), transform) as GameObject;
        Debug.Log("it worked: " + getSelectedObjectFilePath());
    }

    private string getSelectedObjectFilePath()
    {
        return "Squares/" + MyList[_listIdx];
    }

    public void readFromDirectory()
    {
        FileInfo[] files = new DirectoryInfo(path).GetFiles();
        MyList = new List<string>(files.Select(file => file.Name)
            .Where(name => !name.EndsWith(".meta"))
            .Select(file =>
            {
                int index = file.LastIndexOf(".");
                return file.Substring(0, index);
            }));
    }
}
