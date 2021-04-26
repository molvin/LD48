using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Game State/LoadingState")]
public class GameStateLoading : State
{
    bool m_IsLoadingScene = true;

    public override void Enter()
    {
        GameStateManager.Instance.StartCoroutine(LoadScene());
    }
    public override void Tick()
    {

    }
    public override void Exit()
    {
      
    }
    public override State SelectTransition()
    {
        if (m_IsLoadingScene)
            return null;

        if (!Ticker.Instance.IsTicking)
        {
            CharacterRole? player_role = Ticker.Instance.CheckpointRole;
            
            if(player_role == null)
            {
                GameStateManager.Instance.StartCoroutine(LoadScene());
                return null;
            }

            GameStateManager.Instance.PlayerAgent.RemoveOneInput();

            return GameStateManager.Instance.IdleState;
        }

        if(!GameStateManager.Instance.IsScrumming)
            return GameStateManager.Instance.IdleState;

        return null;
    }

    public IEnumerator LoadScene()
    {
        m_IsLoadingScene = true;
        SceneManager.LoadScene(TimelineHolder.Instance.GetNextSceneIndex());

        yield return null;

        GameStateManager.Instance.Setup();
        Ticker.currentBlock = TimelineHolder.Instance.GetCurrentBlock();
        Ticker.Instance.Initialize();
        FindObjectOfType<GameplayInputUI>().Setup();
        Ticker.Instance.TickToNextCheckpoint(false);
        m_IsLoadingScene = false;
    }

    protected override void Initialize() { }
}
