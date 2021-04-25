using GameStructure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayAbilitySystem;

public abstract class TickAgent : MonoBehaviour
{
    public int GridID;

    public int initiative = 0;
    public AbilitySystem AbilitySystem;
    public abstract void Initialize(LDBlock data);
    public abstract void Tick(int Frame, bool Scrum);
}
