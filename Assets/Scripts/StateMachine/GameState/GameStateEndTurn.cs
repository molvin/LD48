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
        //Add all enemies moves

        EnemyAgent[] enemies = FindObjectsOfType<EnemyAgent>();

        foreach(EnemyAgent enemy in enemies)
        {
            enemy.AddActions();
        }

       // Tick tick
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
