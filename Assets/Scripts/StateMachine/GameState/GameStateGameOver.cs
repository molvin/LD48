using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Game State/GameOver")]
public class GameStateGameOver : State
{
    bool IsActive = false;
    bool ShouldExit = true;
    public override void Enter()
    {
        ShouldExit = false;
        IsActive = true;
        TimelineHolder.Instance.SaveCurrentBlock();
        FindObjectOfType<EndScreen>(true).gameObject.SetActive(true);
        CoroutineRunner.Instance.StartCoroutine(EndSoon());
        TimelineHolder.Instance.PushToServer();
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

    private IEnumerator EndSoon()
    {
        yield return new WaitForSeconds(5.0f);
        FindObjectOfType<EndScreen>(true).gameObject.SetActive(false);
        ShouldExit = true;
    }
}

