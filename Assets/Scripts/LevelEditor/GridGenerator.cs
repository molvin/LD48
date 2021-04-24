using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class GridGenerator : MonoBehaviour
{
    public int xWidth;
    public int zWidth;
    public string Name;
    private string mapFolderPath = "Prefabs/Maps";
    private string _mapFolderPath
    {
        get => Application.dataPath + "/"+ mapFolderPath + "/" + Name + ".prefab";
    }
    [HideInInspector]
    [SerializeField]
    private List<List<GameObject>> childrenByPosition;
    public void GenerateLevel()
    {
        if (!Application.isEditor)
        {
            Debug.LogWarning("You may not use this function outside of edit mode");
            return;
        }
        if (childrenByPosition == null)
        {
            childrenByPosition = new List<List<GameObject>>(xWidth);
        }

        for(int x = 0; x < xWidth; x++)
        {
            
            for(int z = 0; z < zWidth; z++)
            {
                
                GameObject ob = PrefabUtility.InstantiatePrefab((GameObject)Resources.Load("SquareChooser"), transform) as GameObject;
                ob.transform.SetPositionAndRotation(new Vector3(x, 0, z), Quaternion.identity);

                if(childrenByPosition.Count <= x || (childrenByPosition.Count > x && childrenByPosition[x] == null) )
                {
                    childrenByPosition.Insert(x, new List<GameObject>(new GameObject[zWidth]));
                }

                if (childrenByPosition[x][z] == null)
                {
                    childrenByPosition[x].Insert(z, ob);
                    ob.GetComponent<SquareChooser>().selectFirst();
                }
                else
                {
                    Object.DestroyImmediate(ob);
                }
            }
        }
    }

    public void ResetLevel()
    {
        if (Application.isEditor)
        {
            for (int i = transform.childCount -1; i > -1; i--)
            {
                Object.DestroyImmediate(transform.GetChild(i).gameObject);
            }
        } else
        {
            Debug.LogWarning("You may not use this function outside of edit mode");
        }
    }

    public void SaveLevel()
    {
        if(Name == null)
        {
            Debug.LogWarning("You need to name you level");
        }
        PrefabUtility.SaveAsPrefabAsset(this.gameObject, _mapFolderPath);
    }

}
