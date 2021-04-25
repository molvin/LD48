using GameStructure;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ticker : MonoBehaviour
{
    public static LDBlock currentBlock;
    public List<TickAgent> tickAgents;
    void Start()
    {
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
            tickAgents[i].Tick();
        }
    }
    public void Scrum(int toFrame)
    {
        for (int i = 0; i < tickAgents.Count; i++)
        {
            tickAgents[i].Scrum(toFrame);
        }
    }
}
