using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

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
    public List<string> MyList = new List<string>(new string[] { "niklas", "DanielF", "Daniel≈" });

    
    protected void onListIndexChange ()
    {
        Debug.Log("it worked");
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
