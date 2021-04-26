using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StateMachine 
{
    public Action OnStateChanged;

    private Dictionary<Type, State> StateMap = new Dictionary<Type, State>();
    private State CurrentState;

    public StateMachine(object Controller, State[] States, State EntryState = null)
    {
        CurrentState = EntryState;
        foreach (State State in States)
        {
            State Instance = UnityEngine.Object.Instantiate(State);
            Instance.Initialize(Controller, this);
            StateMap.Add(Instance.GetType(), Instance);
            CurrentState ??= Instance;
        }
        CurrentState.Enter();
    }

    public void Tick()
    {
        Debug.Log(CurrentState);
        State NewState = CurrentState.SelectTransition();
        if (NewState != null && NewState != CurrentState)
        {
            CurrentState.Exit();
            CurrentState = NewState;
            CurrentState.Enter();

            OnStateChanged?.Invoke();
        }

        CurrentState.Tick();
    }

}
