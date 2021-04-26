using GameplayAbilitySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game State/EndTurn")]
public class GameStateEndTurn : State
{
    bool m_HasTicked;
    protected override void Initialize() { }
    public override void Enter()
    {
    }
    public override void Tick()
    {
        if (Ticker.Instance.IsTicking)
            return;

        m_HasTicked = true;
    }
    public override void Exit()
    {

    }
    public override State SelectTransition()
    {
        if (!m_HasTicked)
            return null;

        bool FirstTime = !GameStateManager.Instance.HasDoneAction;

        if (!GameStateManager.Instance.HasDoneAction)
        {
            GameStateManager.Instance.PlayerAgent.AppendInput(TypeTag.NoAction, new Vector2Int());
            Ticker.Instance.TickCurrent(FirstTime);
        }

        Ticker.Instance.CurrentDone();
        Ticker.Instance.TickUntilPlayableTurn(false);

        GameStateManager.Instance.HasDoneAction = false;
        GameStateManager.Instance.ShouldEndTurn = false;
        return GameStateManager.Instance.IdleState;
    }
}
