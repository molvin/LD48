using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game State/IdleState")]
public class GameStateIdle : State
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
        bool all_enemies_killed = FindObjectsOfType<EnemyAgent>().Length > 0;
        foreach (EnemyAgent enemy in FindObjectsOfType<EnemyAgent>())
        {
            if (enemy.IsAlive)
            {
                all_enemies_killed = false;
                break;
            }
        }

        if (all_enemies_killed)
            return GameStateManager.Instance.LoadingState;

        if(!GameStateManager.Instance.PlayerAgent.IsAlive)
        {
            return GameStateManager.Instance.GameOverState;
        }

        if (GameStateManager.Instance.ShouldEndTurn)
            return GameStateManager.Instance.EndTurnState;

        if (GameStateManager.Instance.ShouldGoToActionState())
            return GameStateManager.Instance.ActionState;

        return null;
    }
}
