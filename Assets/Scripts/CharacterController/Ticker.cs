using GameplayAbilitySystem;
using GameStructure;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ticker : MonoBehaviour
{
    public static LDBlock currentBlock;
    public static Ticker Instance;
    public static float TickVisualTime = 1.0f;
    public static bool ShouldVisualize;

    public CharacterRole? CheckpointRole = null;

    public List<TickAgent> tickAgents;

    private int CurrentTick = 0;
    private int CurrentActor = 0;

    bool m_IsTicking;
    public bool IsTicking { get => m_IsTicking; }

    public void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }

    public void Initialize()
    {
        CurrentActor = 0;
        CurrentTick = 0;
        tickAgents = FindObjectsOfType<TickAgent>().ToList();
        tickAgents.OrderBy((x) => { return x.initiative; });
        for (int i = 0; i < tickAgents.Count; i++)
        {
            tickAgents[i].Initialize(currentBlock);
        }
        for(int i = tickAgents.Count - 1; i >= 0; i--)
        {
            if(tickAgents[i] == null)
                tickAgents.RemoveAt(i);
        }
    }
    public void TickCurrent()
    {
        Debug.Assert(tickAgents[CurrentActor] == GameStateManager.Instance.PlayerAgent, "Ticking player out of turn!");

        ShouldVisualize = true;
        StartCoroutine(TickOverTime(CurrentActor));
    }
    public void CurrentDone()
    {
        CurrentActor = (CurrentActor + 1) % tickAgents.Count;
        if (CurrentActor == 0)
        {
            CurrentTick++;
        }
    }
    public void Scrum(int toFrame)
    {
        ShouldVisualize = false;
        CurrentTick = 0;
        for (int i = 0; i < tickAgents.Count; i++)
        {
            tickAgents[i].Initialize(currentBlock);
        }
        for (int i = 0; i < toFrame; i++)
        {
            for(int a = 0; a < tickAgents.Count; a++)
            {
                tickAgents[a].Tick(CurrentTick, true);
            }
            CurrentTick++;
        }
    }
    public IEnumerator TickOverTime(int ActorIndex)
    {
        m_IsTicking = true;

        TickAgent Agent = tickAgents[ActorIndex];
        Agent.Tick(CurrentTick, !ShouldVisualize);

        yield return new WaitForSeconds(TickVisualTime);

        m_IsTicking = false;
        GameStateManager.Instance.ShouldEndTurn = true;
    }

    public void TickUntilPlayableTurn(bool Scrum)
    {
        ShouldVisualize = !Scrum;
        StartCoroutine(TickUtilPlayableTurn(ShouldVisualize ? TickVisualTime : 0.0f));
    }

    private IEnumerator TickUtilPlayableTurn(float TickTime)
    {
        m_IsTicking = true;
        while(true)
        {
            TickAgent CurrentAgent = tickAgents[CurrentActor];
            if (CurrentAgent is EnemyAgent)
            {
                EnemyAgent Enemy = (EnemyAgent)CurrentAgent;
                if (Enemy.IsAlive)
                {
                    Enemy.Tick(CurrentTick, !ShouldVisualize);
                    yield return new WaitForSeconds(TickTime);
                }
            }
            else
            {
                PlayableAgent Player = (PlayableAgent)CurrentAgent;
                Debug.Log(Player.Role);
                if (Player.IsAlive)
                {
                    if (Player.HasInput(CurrentTick))
                    {
                        Debug.Log("Doing other player stuff");
                        Player.Tick(CurrentTick, !ShouldVisualize);
                    }
                    else if (Player == GameStateManager.Instance.PlayerAgent)
                    {
                        m_IsTicking = false;
                        yield break;
                    }
                    else
                    {
                        Debug.LogWarning("Player is frozen in time!");
                    }
                }
            }
            CurrentActor = (CurrentActor + 1) % tickAgents.Count;
            if (CurrentActor == 0)
            {
                CurrentTick++;
            }
        }
    }

    public void TickToNextCheckpoint(bool Scrum)
    {
        ShouldVisualize = !Scrum;
        StartCoroutine(TickToNextCheckpoint(Scrum ? 0.0f : TickVisualTime));
    }

    // TODO: ADD PADDING FOR INPUT

    private IEnumerator TickToNextCheckpoint(float TickTime)
    {
        CheckpointRole = null;
        m_IsTicking = true;
        while(true)
        {
            if (!tickAgents.Any(a => a is EnemyAgent && a.IsAlive))
            {
                m_IsTicking = false;
                CurrentTick++;
                yield break;
            }
            TickAgent CurrentAgent = tickAgents[CurrentActor];
            if (CurrentAgent is EnemyAgent)
            {
                EnemyAgent Enemy = (EnemyAgent)CurrentAgent;
                if (Enemy.IsAlive)
                {
                    Enemy.Tick(CurrentTick, !ShouldVisualize);
                    yield return new WaitForSeconds(TickTime);
                }
            }
            else
            {
                PlayableAgent Player = (PlayableAgent)CurrentAgent;
                if (Player.IsAlive)
                {
                    if (Player.HasInput(CurrentTick) && Player.GetInput(CurrentTick).Is(TypeTag.DeathAction))
                    {
                        CheckpointRole = Player.Role;
                        m_IsTicking = false;
                        yield break;
                   
                    }
                    else if(Player.HasInput(CurrentTick))
                    {
                        Debug.Log("Doing other player stuff");
                        Player.Tick(CurrentTick, !ShouldVisualize);
                    }
                    else
                    {
                        Debug.Log("Player is Frozen");
                    }
                }
            }
            CurrentActor = (CurrentActor + 1) % tickAgents.Count;
            if (CurrentActor == 0)
            {
                CurrentTick++;
            }
        }
    }
}
