using GameplayAbilitySystem;
using GameStructure;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager m_Instance;
    public static GameStateManager Instance { get { return m_Instance; } }

    public GameStateAction ActionState;
    public GameStateIdle IdleState;
    public GameStateLoading LoadingState;
    public GameStateGameOver GameOverState;
    public GameStateEndTurn EndTurnState;
    public bool IsScrumming = true;

    private StateMachine m_GameStateMachine;

    public CharacterRole Role;

    public Action<bool> OnHasDoneActionUpdate;

    [HideInInspector] public PlayableAgent PlayerAgent;

    public GridGenerator GetGridManager()
    {
        if (m_GridManager == null)
            m_GridManager = FindObjectOfType<GridGenerator>(true);

        return m_GridManager;
    }
    private GridGenerator m_GridManager;

    public FollowMouse GetFolowMouse() 
    {
        if(m_FollowMouse == null)
            m_FollowMouse = FindObjectOfType<FollowMouse>(true);

        return m_FollowMouse;
    }
    private FollowMouse m_FollowMouse;

    [HideInInspector] public int GridIndex = -1;

    public bool ShouldGoToActionState() { return ShouldDoAction; }

    [HideInInspector] public bool HasDoneAction 
    {   get => m_HasDoneAction; 

        set { 
            if (value != m_HasDoneAction)
            {
                m_HasDoneAction = value;
                OnHasDoneActionUpdate?.Invoke(m_HasDoneAction);
            } 
        } 
    }
    bool m_HasDoneAction = false;
    [HideInInspector] public bool ShouldDoAction;
    [HideInInspector] public bool ShouldEndTurn;
    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (m_Instance != null && m_Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_Instance = this;
        }
    }

    public void Setup()
    {
        foreach (PlayableAgent player in FindObjectsOfType<PlayableAgent>())
        {
            if (player.Role == Role)
            {
                Debug.Log("Found Player Agent");
                PlayerAgent = player;
                break;
            }
        }
    }

    public void GoToActionState(System.Type ability_index)
    {
        ActionState.AbilityType = ability_index;
        ShouldDoAction = true;
    }

    public void GoToEndTurnState()
    {
        ActionState.AbilityType = TypeTag.NoAction;
        ShouldDoAction = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_GameStateMachine = new StateMachine(this, new State[] { ActionState, IdleState, LoadingState, EndTurnState }, LoadingState);
    }

    // Update is called once per frame
    void Update()
    {
        m_GameStateMachine.Tick();
    }
}


