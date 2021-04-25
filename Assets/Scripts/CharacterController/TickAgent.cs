using GameStructure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TickAgent : MonoBehaviour
{
    public int initiative = 0;
    public abstract void Initialize(LDBlock data);
    public abstract void Tick();
    public abstract void Scrum(int toFrame);
}
