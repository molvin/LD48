using GameStructure;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Game State/LoadingState")]
public class GameStateLoading : State
{
    bool m_IsLoadingScene = true;
    bool m_WaitingForSelection = false;

    public override void Enter()
    {
        m_WaitingForSelection = false;
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
                Debug.LogError("Mardröms rolle vadför scen");
                return null;
            }
            //Waiting for an answer
            if (m_WaitingForSelection && GameStateManager.Instance.IsWaitingForSelection)
                return null;

            //Give the player a character to select or not
            if (!m_WaitingForSelection)
            {
                GameStateManager.Instance.Setup((CharacterRole)player_role);
                m_WaitingForSelection = true;
            }

            //We did not want the character
            if(GameStateManager.Instance.PlayerAgent == null)
            {
                Ticker.Instance.TickToNextCheckpoint(false);
                return null;
            }

            SetUpCharacter((CharacterRole)player_role);
            GameStateManager.Instance.PlayerAgent.RemoveOneInput();
            return GameStateManager.Instance.IdleState;
        }

        return null;
    }

    public IEnumerator LoadScene()
    {
        m_IsLoadingScene = true;
        LDBlock next_block = TimelineHolder.Instance.GenerateNextRelevantBlock();
        SceneManager.LoadScene(next_block.level);

        yield return null;
        Ticker.currentBlock = TimelineHolder.Instance.GetCurrentBlock();
        Ticker.Instance.Initialize();
        Ticker.Instance.TickToNextCheckpoint(false);
        m_IsLoadingScene = false;
    }

    public void SetUpCharacter(CharacterRole role)
    {
        Ticker.currentBlock = TimelineHolder.Instance.GetCurrentBlock();
        int current_frame = Ticker.Instance.GetCurrentTick;
        Ticker.Instance.Initialize();
        Ticker.Instance.Scrum(current_frame - 1);

        FindObjectOfType<GameplayInputUI>().Setup();
    }

    protected override void Initialize() { }
}
