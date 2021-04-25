using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridCellProperties : MonoBehaviour
{
    public enum props
    {
        Nan         = 0,
        Walkable    = 1 << 0,
        Water       = 1 << 1,
        Fire        = 1 << 2,
        Obstacle    = 1 << 3,
        Occupied    = 1 << 4,
    }
    [HideInInspector]
    public props flags;

    [HideInInspector]
    public int GridX;
    [HideInInspector]
    public int GridZ;

    [HideInInspector]
    public bool obstacle
    {
        get => flags.HasFlag(props.Obstacle);
    }
    [HideInInspector]
    public Vector3 worldPosition
    {
        get => this.transform.position;
    }
    [HideInInspector]
    public SquareChooser parent;

    [HideInInspector]
    public int cost;
}
