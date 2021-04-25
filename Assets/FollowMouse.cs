using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public GridLayout grid;
    public GridGenerator gridGen;
    public Action<Vector3Int> CellSelected;
    // Update is called once per frame
    void Update()
    {
        Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int iso = grid.WorldToCell(new Vector3(world.x, 0, world.z));
        transform.position = grid.CellToWorld(iso);

        //make callback
        if (Input.GetMouseButtonUp(0))
        {
            CellSelected?.Invoke(iso);
            GridCellProperties.props hej = gridGen.GetSquareType(new Vector2Int(iso.x, iso.y));
            Debug.Log($"for iso x:{iso.x} y:{iso.y}  i got:{hej}");
        }
    }
}
