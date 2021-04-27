using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridGenerator : MonoBehaviour
{
    public enum BlockStatus
    {
        Nan = 0,
        Walkable = 1 << 0,
        Water = 1 << 1,
        Fire = 1 << 2,
        Obstacle = 1 << 3,
        Occupied = 1 << 4,
    }

    public GridLayout Grid;
    public int xWidth;
    public int zWidth;
    public float CellOffset = 0.4f;

    [HideInInspector]
    [SerializeField]
    private GridCellProperties[,] childrenByPosition;
    private List<GridCellProperties> path;

    public float nodeGizRadius = 0.16f;
    public void Awake()
    {
        CreateGridData();
    }
    public void CreateGridData()
    {
        childrenByPosition = new GridCellProperties[xWidth, zWidth];
        GridCellProperties[] children = transform.GetComponentsInChildren<GridCellProperties>();
        foreach (GridCellProperties gp in children)
        {
            childrenByPosition[gp.GridX, gp.GridZ] = gp;
        }
    }
    public void GenerateLevel()
    {

        if (!Application.isEditor)
        {
            Debug.LogWarning("You may not use this function outside of edit mode");
            return;
        }
#if UNITY_EDITOR
        for (int x = 0; x < xWidth; x++)
        {
            for(int z = 0; z < zWidth; z++)
            {
                GameObject ob = PrefabUtility.InstantiatePrefab((GameObject)Resources.Load("GridCell"), transform) as GameObject;
                ob.transform.position = Grid.CellToWorld(new Vector3Int(x,z,0));
                GridCellProperties gCPRef = ob.GetComponent<GridCellProperties>();
                gCPRef.GridX = x;
                gCPRef.GridZ = z;
            }
        }
        CreateGridData();
#endif
    }
    public void ResetLevel()
    {
        if (Application.isEditor)
        {
            for (int i = transform.childCount -1; i > -1; i--)
            {
                Object.DestroyImmediate(transform.GetChild(i).gameObject);
            }
            childrenByPosition = null;
        } else
        {
            Debug.LogWarning("You may not use this function outside of edit mode");
        }
    }
    public void test()
    {
        GridCellProperties start = childrenByPosition[0, 0];
        GridCellProperties target = childrenByPosition[xWidth - 1, zWidth - 1];
        findPath(start, target);

        path.ForEach(step => Debug.Log(step.GridX + " " + step.GridZ));
    }


    //Sick method
    public List<Vector3Int> findPath(Vector3Int start, Vector3Int target)
    {
        List<GridCellProperties> uselessShit = findPath(childrenByPosition[start.x, start.y], childrenByPosition[target.x, target.y]);
        if (uselessShit == null)
            return null;
        return uselessShit.Select((g) => new Vector3Int(g.GridX, g.GridZ, 0)).ToList();
    }
    public Vector3Int WorldToCell(Vector3 worldPos)
    {
        return Grid.WorldToCell(new Vector3(worldPos.x, 0, worldPos.z));
    }
    public Vector3 CellToWorld(Vector3Int cellPos)
    {
        return Grid.CellToWorld(cellPos) + Vector3.forward * CellOffset;
    }
    public BlockStatus GetCellStatus(Vector2Int pos)
    {
        if (pos.x >= xWidth || pos.x < 0 || pos.y >= zWidth || pos.y < 0)
            return BlockStatus.Nan;
        return childrenByPosition[pos.x, pos.y].flags;
    }
    public bool isPathObstructed(List<Vector3Int> path)
    {
        return !path.Select(v => childrenByPosition[v.x, v.y])
                    .Any(GCP => GCP.flags.HasFlag(BlockStatus.Obstacle));
    }
    public void setOccupied(Vector3Int pos, bool occupied)
    {
        if(occupied)
            childrenByPosition[pos.x, pos.y].flags |= BlockStatus.Occupied;
        else
            childrenByPosition[pos.x, pos.y].flags &= ~BlockStatus.Occupied;
    }

    public bool IsOccupied(Vector3Int pos)
    {
        return childrenByPosition[pos.x, pos.y].flags.HasFlag(BlockStatus.Occupied);
    }

    private List<GridCellProperties> getNeighbors(GridCellProperties node)
    {
        List<GridCellProperties> neighbors = new List<GridCellProperties>();

        //checks and adds top neighbor
        if (node.GridX >= 0 && node.GridX < xWidth && node.GridZ + 1 >= 0 && node.GridZ + 1 < zWidth)
            neighbors.Add(childrenByPosition[node.GridX, node.GridZ + 1]);
        
        //checks and adds bottom neighbor
        if (node.GridX >= 0 && node.GridX < xWidth && node.GridZ - 1 >= 0 && node.GridZ - 1 < zWidth)
            neighbors.Add(childrenByPosition[node.GridX, node.GridZ - 1]);
        
        //checks and adds right neighbor
        if (node.GridX + 1 >= 0 && node.GridX + 1 < xWidth && node.GridZ >= 0 && node.GridZ < zWidth)
            neighbors.Add(childrenByPosition[node.GridX + 1, node.GridZ]);
        
        //checks and adds left neighbor
        if (node.GridX - 1 >= 0 && node.GridX - 1 < xWidth && node.GridZ >= 0 && node.GridZ < zWidth)
            neighbors.Add(childrenByPosition[node.GridX - 1, node.GridZ]);
        
        return neighbors;
    }
    private List<GridCellProperties> findPath(GridCellProperties start, GridCellProperties target)
    {
        List<GridCellProperties> openSet = new List<GridCellProperties>();
        HashSet<GridCellProperties> closedSet = new HashSet<GridCellProperties>();
        openSet.Add(start);

        //calculates path for path finding
        while (openSet.Count > 0)
        {
            GridCellProperties node = openSet[0];
            openSet.Remove(node);
            closedSet.Add(node);

            //If target found, retrace path
            if (node == target)
            {
                RetracePath(start, target);
                return path;
            }

            foreach (GridCellProperties neighbour in getNeighbors(node))
            {
                if (neighbour.flags.HasFlag(BlockStatus.Occupied) || !neighbour.flags.HasFlag(BlockStatus.Walkable) || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour =  GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.cost || !openSet.Contains(neighbour))
                {
                    
                    neighbour.cost = GetDistance(neighbour, target);
                    neighbour.parent = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
        return null;
    }
    private int GetDistance(GridCellProperties nodeA, GridCellProperties nodeB)
    {
        int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int dstY = Mathf.Abs(nodeA.GridZ - nodeB.GridZ);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
    private void RetracePath(GridCellProperties startNode, GridCellProperties endNode)
    {
        List<GridCellProperties> newPath = new List<GridCellProperties>();
        GridCellProperties currentNode = endNode;

        while (currentNode != startNode)
        {
            newPath.Add(currentNode);
            currentNode = currentNode.parent;
        }
        newPath.Reverse();
        path = newPath;
    }
    void OnDrawGizmos()
    { 
        if (childrenByPosition != null)
        {
            foreach (GridCellProperties n in childrenByPosition)
            {
                bool obs = n.flags.HasFlag(BlockStatus.Obstacle);
                bool walk = n.flags.HasFlag(BlockStatus.Walkable);
                bool occ = n.flags.HasFlag(BlockStatus.Occupied);
                Gizmos.color = new Color(obs ? 255 : 0, walk ? 255 : 0, occ ? 255 : 0, 200);
                if (path != null && path.Contains(n))
                    Gizmos.color = Color.black;
                Gizmos.DrawWireSphere(n.transform.position, nodeGizRadius);
            }
        }
        else
        {
            CreateGridData();
        }
    }
}
