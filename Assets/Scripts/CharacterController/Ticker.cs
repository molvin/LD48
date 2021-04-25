using GameStructure;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ticker : MonoBehaviour
{
    public static LDBlock currentBlock;
    public static Ticker Instance;
    public static float TickVisualTime = 0.5f;
    public static bool ShouldVisualize;

    public List<TickAgent> tickAgents;

    private int CurrentTick;

    public void Initialize()
    {
        Instance = this;
        CurrentTick = 0;
        tickAgents = FindObjectsOfType<TickAgent>().ToList();
        for (int i = 0; i < tickAgents.Count; i++)
        {
            tickAgents[i].Initialize(currentBlock);
        }
    }
    public void Tick()
    {
        ShouldVisualize = true;
        StartCoroutine(TickOverTime());
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
    public IEnumerator TickOverTime()
    {
        tickAgents.OrderBy((x) => { return x.initiative; });
        for(int i = 0;i< tickAgents.Count; i++)
        {
            tickAgents[i].Tick(CurrentTick, false);
            yield return new WaitForSeconds(TickVisualTime);
        }
        CurrentTick++;
    }
}
