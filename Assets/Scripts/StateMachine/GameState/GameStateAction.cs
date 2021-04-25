using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game State/ActionState")]
public class GameStateAction : State
{
    public uint ActionIndex;
    public override void Enter() 
    {
        Debug.Log("Enter action: " + ActionIndex);
    }
    public override void Tick() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Do Action");
            GameStateManager.Instance.ShouldDoAction = false;
        }     
    }
    public override void Exit() 
    {
        
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
