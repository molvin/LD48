using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager m_Instance;
    public static GameStateManager Instance { get { return m_Instance; } }

    public GameStateAction ActionState;
    public GameStateIdle IdleState;


    private StateMachine m_GameStateMachine;

    public bool ShouldGoToActionState() { return ShouldDoAction; }
    public bool ShouldDoAction;

    private void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_Instance = this;
        }
    }

    public void GoToActionState(uint action_index)
    {
        ActionState.ActionIndex = action_index;
        ShouldDoAction = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_GameStateMachine = new StateMachine(this, new State[] { ActionState, IdleState }, IdleState);
    }

    // Update is called once per frame
    void Update()
    {
        m_GameStateMachine.Tick();
    }
}


