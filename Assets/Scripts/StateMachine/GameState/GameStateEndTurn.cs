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
        Ticker.Instance.CurrentDone();
        Ticker.Instance.TickUntilPlayableTurn(false);
    }
    public override void Tick()
    {
    }
    public override void Exit()
    {

    }
    public override State SelectTransition()
    {
        if (!GameStateManager.Instance.PlayerAgent.IsAlive)
            return GameStateManager.Instance.GameOverState;

        if (Ticker.Instance.IsTicking)
            return null;

        if (!GameStateManager.Instance.HasDoneAction)
        {
            GameStateManager.Instance.PlayerAgent.AppendInput(TypeTag.NoAction, new Vector2Int());
            Ticker.Instance.TickCurrent();
        }

        GameStateManager.Instance.HasDoneAction = false;
        GameStateManager.Instance.ShouldEndTurn = false;
        return GameStateManager.Instance.IdleState;
    }
}
