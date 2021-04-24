using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject
{
    protected StateMachine StateMachine;
    protected object Owner;

    public void Initialize(object Owner, StateMachine StateMachine)
    {
        this.Owner = Owner;
        this.StateMachine = StateMachine;
        Initialize();
    }

    public virtual void Enter() { }
    public virtual void Tick() { }
    public virtual void Exit() { }
    public virtual State SelectTransition() { return null; }

    protected virtual void Initialize() { }
}
