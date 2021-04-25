using GameplayAbilitySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game State/EndTurn")]
public class GameStateEndTurn : State
{
    protected override void Initialize() { }
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
        if (!GameStateManager.Instance.HasDoneAction)
        {
            GameStateManager.Instance.PlayerAgent.AppendInput(TypeTag.NoAction, new Vector2Int());
            Ticker.Instance.Tick();
        }

        if (!GameStateManager.Instance.HasMoved)
        {
            GameStateManager.Instance.PlayerAgent.AppendInput(TypeTag.NoAction, new Vector2Int());
            Ticker.Instance.Tick();
        }

        GameStateManager.Instance.HasDoneAction = false;
        GameStateManager.Instance.HasMoved      = false;
        GameStateManager.Instance.ShouldEndTurn = false;
        return GameStateManager.Instance.IdleState;
    }
}
