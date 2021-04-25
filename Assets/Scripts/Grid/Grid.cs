using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Vector2Int Size;
    public Cell[] Cells;

    public class Cell
    {

    }

    public void Init()
    {

    }

    public void MouseRaycast()
    {
        Plane plane = new Plane(Vector3.up, Vector3.zero);  
    }    
}
