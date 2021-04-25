using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game State/GameOver")]
public class GameStateGameOver : State
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
        return GameStateManager.Instance.IdleState;
    }
}

