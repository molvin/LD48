using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEditor;


public class SquareChooser : MonoBehaviour
{
    
    private string path
    {
        get => Application.dataPath + "/Resources/Squares";
    }
    [HideInInspector]
    [SerializeField]
    private int _listIdx = 0;
    public int listIdx
    {
        get => _listIdx;
        set
        {
            if(_listIdx != value)
            {
                _listIdx = value;
                onListIndexChange();
            }
            
        }
    }
    [HideInInspector]
    public List<string> MyList = new List<string>();


    [HideInInspector]
    public GridCellProperties node;
    [HideInInspector]
    public int GridX;
    [HideInInspector]
    public int GridZ;

    public void selectFirst()
    {
        readFromDirectory();
        listIdx = 0;
    }

    public void onListIndexChange ()
    {
        if (!Application.isEditor)
            return;
        Debug.Log("a");
        for (int i = transform.childCount - 1; i > -1; i--)
        {
            Object.DestroyImmediate(transform.GetChild(i).gameObject);
        }
        GameObject ob = PrefabUtility.InstantiatePrefab((GameObject)Resources.Load(getSelectedObjectFilePath()), transform) as GameObject;
        node = ob.GetComponent<GridCellProperties>();
        Debug.Log(node);
        node.GridX = this.GridX;
        node.GridZ = this.GridZ;
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
