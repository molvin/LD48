using GameStructure;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ticker : MonoBehaviour
{
    public static LDBlock currentBlock;
    public List<TickAgent> tickAgents;

    private int CurrentTick;

    void Start()
    {
        CurrentTick = 0;
        tickAgents = FindObjectsOfType<TickAgent>().ToList();
        for (int i = 0; i < tickAgents.Count; i++)
        {
            tickAgents[i].Initialize(currentBlock);
        }
    }
    public void Tick()
    {
        tickAgents.OrderBy((x) => { return x.initiative; });
        for(int i = 0;i< tickAgents.Count; i++)
        {
            tickAgents[i].Tick(CurrentTick, false);
        }
        CurrentTick++;
    }
    public void Scrum(int toFrame)
    {
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
}
