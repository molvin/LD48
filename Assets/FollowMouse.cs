using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public GridLayout grid;
    public Action<Vector3Int> CellSelected;
    // Update is called once per frame
    void Update()
    {
        Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int iso = grid.WorldToCell(new Vector3(world.x, 0, world.z));
        transform.position = grid.CellToWorld(iso);

        //make callback
        if (Input.GetMouseButtonUp(0))
            CellSelected?.Invoke(iso);
    }
}
