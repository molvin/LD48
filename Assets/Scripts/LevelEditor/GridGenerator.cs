using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    private SquareChooser[,] childrenByPosition;

    private int gridSizeX;
    private int gridSizeZ;
    private List<SquareChooser> path;
    private float nodeGizRadius = 0.5f;
    
    public void GenerateLevel()
    {

        if (!Application.isEditor)
        {
            Debug.LogWarning("You may not use this function outside of edit mode");
            return;
        }

        gridSizeX = xWidth;
        gridSizeZ = zWidth;
        childrenByPosition = new SquareChooser[xWidth, zWidth];
        for (int x = 0; x < xWidth; x++)
        {
            
            for(int z = 0; z < zWidth; z++)
            {
                
                GameObject ob = PrefabUtility.InstantiatePrefab((GameObject)Resources.Load("SquareChooser"), transform) as GameObject;
                ob.transform.SetPositionAndRotation(new Vector3(x, 0, z), Quaternion.identity);
                SquareChooser gCPRef = ob.GetComponent<SquareChooser>();
                gCPRef.GridX = x;
                gCPRef.GridZ = z;
                childrenByPosition[x, z] = gCPRef;
                gCPRef.onListIndexChange();
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

    public void test()
    {
        SquareChooser start = childrenByPosition[Mathf.FloorToInt(Random.Range(0,xWidth-1)), Mathf.FloorToInt(Random.Range(0, zWidth - 1))];
        SquareChooser target = childrenByPosition[Mathf.FloorToInt(Random.Range(0, xWidth - 1)), Mathf.FloorToInt(Random.Range(0, zWidth - 1))];
        Debug.Log(start.GridX + " " + start.GridZ);
        Debug.Log(target.GridX + " " + target.GridZ);
        findPath(start, target);

        path.ForEach(step => Debug.Log(step.GridX + " " + step.GridZ));
    }


    private List<SquareChooser> getNeighbors(SquareChooser node)
    {
        List<SquareChooser> neighbors = new List<SquareChooser>();

        //checks and adds top neighbor
        if (node.node.GridX >= 0 && node.node.GridX < gridSizeX && node.node.GridZ + 1 >= 0 && node.node.GridZ + 1 < gridSizeZ)
            neighbors.Add(childrenByPosition[node.node.GridX, node.node.GridZ + 1]);
        
        //checks and adds bottom neighbor
        if (node.node.GridX >= 0 && node.node.GridX < gridSizeX && node.node.GridZ - 1 >= 0 && node.node.GridZ - 1 < gridSizeZ)
            neighbors.Add(childrenByPosition[node.node.GridX, node.node.GridZ - 1]);
        
        //checks and adds right neighbor
        if (node.node.GridX + 1 >= 0 && node.node.GridX + 1 < gridSizeX && node.node.GridZ >= 0 && node.node.GridZ < gridSizeZ)
            neighbors.Add(childrenByPosition[node.node.GridX + 1, node.node.GridZ]);
        
        //checks and adds left neighbor
        if (node.node.GridX - 1 >= 0 && node.node.GridX - 1 < gridSizeX && node.node.GridZ >= 0 && node.node.GridZ < gridSizeZ)
            neighbors.Add(childrenByPosition[node.node.GridX - 1, node.node.GridZ]);
        
        return neighbors;
    }


    public List<SquareChooser> findPath(Vector2Int start, Vector2Int target)
    {
        return findPath(childrenByPosition[start.x, start.y], childrenByPosition[target.x, target.y]);
    }


    public List<SquareChooser> findPath(SquareChooser start, SquareChooser target)
    {
        List<SquareChooser> openSet = new List<SquareChooser>();
        HashSet<SquareChooser> closedSet = new HashSet<SquareChooser>();
        openSet.Add(start);

        //calculates path for pathfinding
        while (openSet.Count > 0)
        {
            SquareChooser node = openSet[0];
            openSet.Remove(node);
            closedSet.Add(node);

            //If target found, retrace path
            if (node == target)
            {
                RetracePath(start, target);
                return path;
            }

            //adds neighbor nodes to openSet
            Debug.Log(node);
            foreach (SquareChooser neighbour in getNeighbors(node))
            {
                if (neighbour.node.obstacle || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour =  GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.node.cost || !openSet.Contains(neighbour))
                {
                    
                    neighbour.node.cost = GetDistance(neighbour, target);
                    neighbour.node.parent = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
        return null;
    }

    int GetDistance(SquareChooser nodeA, SquareChooser nodeB)
    {
        int dstX = Mathf.Abs(nodeA.node.GridX - nodeB.node.GridX);
        int dstY = Mathf.Abs(nodeA.node.GridZ - nodeB.node.GridZ);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

    void RetracePath(SquareChooser startNode, SquareChooser endNode)
    {
        List<SquareChooser> newPath = new List<SquareChooser>();
        SquareChooser currentNode = endNode;

        while (currentNode != startNode)
        {
            newPath.Add(currentNode);
            currentNode = currentNode.node.parent;
        }
        newPath.Reverse();

        path = newPath;

    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSizeX, 1, gridSizeZ));

        if (childrenByPosition != null)
        {
            foreach (SquareChooser n in childrenByPosition)
            {
                if (n.node.obstacle)
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = Color.white;

                if (path != null && path.Contains(n))
                    Gizmos.color = Color.black;
                Vector3 abitUp = n.transform.position;
                abitUp.y += 0.5f;
                Gizmos.DrawCube(abitUp, Vector3.one * (nodeGizRadius));

            }
        }
    }

}
