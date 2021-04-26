using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static GridGenerator;

[System.Serializable]
public class GridCellProperties : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    public BlockStatus flags;

    [HideInInspector]
    public int GridX;
    [HideInInspector]
    public int GridZ;
    [HideInInspector]
    public Vector3 worldPosition
    {
        get => this.transform.position;
    }
    [HideInInspector]
    public GridCellProperties parent;

    [HideInInspector]
    public int cost;
    public void Start()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
    }
}
