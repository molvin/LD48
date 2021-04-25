using GameplayAbilitySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game State/ActionState")]
public class GameStateAction : State
{
    public System.Type AbilityType;
    
    public override void Enter() 
    {
        GameStateManager.Instance.GetFolowMouse().gameObject.SetActive(true);
        GameStateManager.Instance.GetFolowMouse().CellSelected += CellSelected;
        GameStateManager.Instance.GetFolowMouse().AbilityType = AbilityType;
        Debug.Log("Enter action");
    }
    public override void Tick() 
    {
    }

    private void CellSelected(Vector3Int pos)
    {
        GameStateManager.Instance.ShouldDoAction = false;
        Debug.Log("Action done");
    }

    public override void Exit() 
    {
        GameStateManager.Instance.GetFolowMouse().CellSelected -= CellSelected;
        GameStateManager.Instance.GetFolowMouse().gameObject.SetActive(false);
    }
    public override State SelectTransition() 
    {
        if(!GameStateManager.Instance.ShouldDoAction)
        {
            return GameStateManager.Instance.IdleState;
        }
        return null;
    }

    protected override void Initialize() { }
}
