using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game State/GameOver")]
public class GameStateGameOver : State
{
    bool IsActive = false;
    bool ShouldExit = true;
    public override void Enter()
    {
        IsActive = true;
    }
    public override void Tick()
    {
  
    }
    public override void Exit()
    {
        IsActive = false;
    }
    public override State SelectTransition()
    {
        if(ShouldExit)
            return GameStateManager.Instance.IdleState;

        return null;
    }
}

