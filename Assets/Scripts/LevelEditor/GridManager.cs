using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private static GridManager m_Instance;
    public static GridManager Instance { get { return m_Instance; } }

    [SerializeField] private string m_Path;
    private List<string> m_GridPaths;

    public GridGenerator CurrentGrid;

    private void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_Instance = this;
        }
    }

    public void LoadGridByIndex(int grid_index)
    {
        if (CurrentGrid)
            Destroy(CurrentGrid.gameObject);

        if (grid_index < 0)
            grid_index = Random.Range(0, m_GridPaths.Count());

        string current_path = m_GridPaths[grid_index];
        current_path = m_Path.Replace("Resources/", "") + "/" + current_path;
        Debug.Log(current_path);
        GameObject new_game_object = (GameObject)Resources.Load(current_path);
        GameObject grid_gameobject = Instantiate(new_game_object, transform);
        CurrentGrid = grid_gameobject.GetComponent<GridGenerator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        FileInfo[] files = new DirectoryInfo(Application.dataPath + "/" + m_Path).GetFiles();
        m_GridPaths = new List<string>(files.Select(file => file.Name)
            .Where(name => !name.EndsWith(".meta")).Select(file =>
            {
                int index = file.LastIndexOf(".");
                return file.Substring(0, index);
            }));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
