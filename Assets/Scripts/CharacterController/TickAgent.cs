using GameStructure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TickAgent : MonoBehaviour
{
    [HideInInspector]
    public int GridID;

    public int initiative = 0;
    public abstract void Initialize(LDBlock data);
    public abstract void Tick(int Frame, bool Scrum);
}
