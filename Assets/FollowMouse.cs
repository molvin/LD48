using GameplayAbilitySystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GridGenerator;

public class FollowMouse : MonoBehaviour
{
    public GridGenerator gridGen;
    public Action<Vector3Int> OnCellSelected;
    public SpriteRenderer MousePointerMarker;
    public GameObject DefaultTilePrefab;
    public System.Type AbilityType;
    private SpriteRenderer[,] tiles;
    // Update is called once per frame
    private void Awake()
    {
        gameObject.SetActive(false);
        tiles = new SpriteRenderer[gridGen.xWidth, gridGen.zWidth];
        for (int z = 0; z < gridGen.zWidth; z++)
        {
            for (int x = 0; x < gridGen.xWidth; x++)
            {
                tiles[x, z] = Instantiate(DefaultTilePrefab).GetComponent<SpriteRenderer>();
                tiles[x, z].transform.position = gridGen.CellToWorld(new Vector3Int(x,z,0));
                tiles[x, z].transform.parent = this.transform;
            }
        }
    }
    void Update()
    {
        //Show here the user can cast their spell!
        ShowAvailiableSquares();

        //Show the reticule!
        Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int iso = gridGen.WorldToCell(new Vector3(world.x, 0, world.z));
        iso.x = Mathf.Clamp(iso.x, 0, gridGen.xWidth - 1);
        iso.y = Mathf.Clamp(iso.y, 0, gridGen.zWidth - 1);
        bool can_cast = GameStateManager.Instance.PlayerAgent.AbilitySystem.CanActivateTargetAbilityByTag(AbilityType, (Vector2Int)iso);
        MousePointerMarker.transform.position = gridGen.CellToWorld(iso);
        MousePointerMarker.color = can_cast ? Color.yellow : Color.red;
        
        //make callback
        if (Input.GetMouseButtonUp(0) && can_cast)
        {
            OnCellSelected?.Invoke(iso);
            BlockStatus hej = gridGen.GetCellStatus(new Vector2Int(iso.x, iso.y));
        }
    }

    void ShowAvailiableSquares()
    {
        for (int z = 0; z < gridGen.zWidth; z++)
        {
            for (int x = 0; x < gridGen.xWidth; x++)
            {
                bool can_cast = GameStateManager.Instance.PlayerAgent.AbilitySystem.CanActivateTargetAbilityByTag(AbilityType, new Vector2Int(x,z));
                tiles[x, z].gameObject.SetActive(can_cast);
            }
        }
    }

}
