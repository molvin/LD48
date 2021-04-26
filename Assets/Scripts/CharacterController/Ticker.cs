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

    public List<TickAgent> tickAgents;

    private int CurrentTick;
    private int CurrentAction => CurrentTick * 2;
    private int CurrentActor;

    bool m_IsTicking;
    public bool IsTicking { get => m_IsTicking; }

    public void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }

    public void Initialize()
    {
        CurrentTick = 0;
        CurrentActor = 0;
        tickAgents = FindObjectsOfType<TickAgent>().ToList();
        tickAgents.OrderBy((x) => { return x.initiative; });
        for (int i = 0; i < tickAgents.Count; i++)
        {
            tickAgents[i].Initialize(currentBlock);
        }
    }
    public void TickCurrent(bool First)
    {
        Debug.Assert(tickAgents[CurrentActor] == GameStateManager.Instance.PlayerAgent, "Ticking player out of turn!");

        ShouldVisualize = true;
        StartCoroutine(TickOverTime(CurrentActor, First ? 0 : 1));
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
    public IEnumerator TickOverTime(int ActorIndex, int TickAction)
    {
        m_IsTicking = true;

        TickAgent Agent = tickAgents[ActorIndex];
        Agent.Tick(CurrentAction + TickAction, !ShouldVisualize);

        yield return new WaitForSeconds(TickVisualTime);

        m_IsTicking = false;
    }

    public void TickUntilPlayableTurn(bool Scrum)
    {
        ShouldVisualize = !Scrum;
        StartCoroutine(TickUtilPlayableTurn(ShouldVisualize ? TickVisualTime : 0.0f));
    }

    private IEnumerator TickUtilPlayableTurn(float TickTime)
    {
        m_IsTicking = true;
        for(;;)
        {
            TickAgent CurrentAgent = tickAgents[CurrentActor];
            if (CurrentAgent is EnemyAgent)
            {
                EnemyAgent Enemy = (EnemyAgent)CurrentAgent;
                if (Enemy.IsAlive)
                {
                    Enemy.AddActions();
                    Enemy.Tick(0, !ShouldVisualize);
                    yield return new WaitForSeconds(TickTime);
                    Enemy.Tick(0, !ShouldVisualize);
                    yield return new WaitForSeconds(TickTime);
                }
            }
            else
            {
                PlayableAgent Player = (PlayableAgent)CurrentAgent;
                if (Player.IsAlive)
                {
                    if (Player.HasInput(CurrentAction))
                    {
                        Player.Tick(CurrentAction, !ShouldVisualize);
                        Player.Tick(CurrentAction + 1, !ShouldVisualize);
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
}
