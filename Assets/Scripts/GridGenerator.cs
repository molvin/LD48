using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int xWidth;
    public int zWidth;

    public void GenerateLevel()
    {
       
        for(int x = 0; x < xWidth; x++)
        {
            for(int z = 0; z < zWidth; z++)
            {
                GameObject ob = PrefabUtility.InstantiatePrefab((UnityEngine.GameObject)Resources.Load("SquareChooser"),this.gameObject.transform) as GameObject;
                ob.transform.SetPositionAndRotation(new Vector3(x, 0, z), Quaternion.identity);
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

    }

}
