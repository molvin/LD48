using GameStructure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayAbilitySystem;

public abstract class TickAgent : MonoBehaviour
{
    public Animator Animator;
    public Vector2Int GridPos;
    public bool IsAlive = true;

    public int initiative = 0;
    public AbilitySystem AbilitySystem;
    public abstract void Initialize(LDBlock data);
    public abstract void Tick(int Frame, bool Scrum);
}
