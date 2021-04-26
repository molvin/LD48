using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game State/LoadingState")]
public class GameStateLoading : State
{
    private bool isLoading;

    public override void Enter()
    {
       isLoading = false;
    }
    public override void Tick()
    {
        //TODO: load a new random scene;
        //isLoading = false;
    }
    public override void Exit()
    {

    }
    public override State SelectTransition()
    {
        if(isLoading)
            return null;
        return GameStateManager.Instance.IdleState;
    }

    protected override void Initialize() { }
}
