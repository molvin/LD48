using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game State/IdleState")]
public class GameStateIdle : State
{
    public override void Enter()
    {
       
    }
    public override void Tick()
    {

    }
    public override void Exit()
    {
        
    }
    public override State SelectTransition() 
    {
        if (GameStateManager.Instance.ShouldGoToActionState())
            return GameStateManager.Instance.ActionState;

        return null;
    }

    protected override void Initialize() { }
}
