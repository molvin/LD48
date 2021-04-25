using GameplayAbilitySystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GridGenerator;

public class FollowMouse : MonoBehaviour
{
    public GridGenerator gridGen;
    public GameObject selectionGrid;
    public Action<Vector3Int> CellSelected;
    public System.Type AbilityType;
    // Update is called once per frame

    private void OnEnable()
    {
        selectionGrid.SetActive(true);
    }
    private void OnDisable()
    {
        selectionGrid.SetActive(false);
    }
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    void Update()
    {
        
        Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int iso = gridGen.WorldToCell(new Vector3(world.x, 0, world.z));
        iso.x = Mathf.Clamp(iso.x, 0, gridGen.xWidth);
        iso.y = Mathf.Clamp(iso.y, 0, gridGen.zWidth);
        bool can_cast = GameStateManager.Instance.PlayerAgent.AbilitySystem.CanActivateTargetAbilityByTag(AbilityType, (Vector2Int)iso);

        if(!can_cast)
        {
            //TODO: Make read instead!
            return;
        }

        transform.position = gridGen.CellToWorld(iso);
        //make callback
        if (Input.GetMouseButtonUp(0))
        {
            CellSelected?.Invoke(iso);
            BlockStatus hej = gridGen.GetCellStatus(new Vector2Int(iso.x, iso.y));
            Debug.Log($"for iso x:{iso.x} y:{iso.y}  i got:{hej}");
        }
    }

}
