using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockToCell : MonoBehaviour
{
    public GridLayout grid;

    // Update is called once per frame
    void Update()
    {
        Vector3 world = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3Int iso = grid.WorldToCell(world);
        transform.position = grid.CellToWorld(iso);
    }
}
