using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Game State/MenuState")]
public class GameStateMenuState : State
{
    public bool LeaveMenu;
    public override void Enter()
    {
        SceneManager.LoadScene(1);
    }
    public override void Tick(){}
    public override void Exit(){}
    public override State SelectTransition()
    {
        if (GameStateManager.Instance.LeaveMenu)
            return GameStateManager.Instance.LoadingState;
        else
            return null;
    }
}

