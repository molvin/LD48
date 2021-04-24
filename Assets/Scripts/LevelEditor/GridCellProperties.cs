using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridCellProperties : MonoBehaviour
{
    public enum props
    {
        Walkable = 1 << 0,
        Water = 1 << 1,
        Fire = 1 << 2,
        Obstacle = 1 << 3,
    }
    [HideInInspector]
    public props flags;

 
}
