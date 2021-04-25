using GameplayAbilitySystem;
using GameStructure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager m_Instance;
    public static GameStateManager Instance { get { return m_Instance; } }

    public GameStateAction ActionState;
    public GameStateIdle IdleState;
    public GameStateLoading LoadingState;
    public GameStateLoading GameOverState;

    private StateMachine m_GameStateMachine;

    public CharacterRole Role;

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
    [HideInInspector] public bool ShouldDoAction;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += Setup;

        if (m_Instance != null && m_Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_Instance = this;
        }
    }

    public void Setup(Scene scene, LoadSceneMode scene_mode)
    {
        LDBlock block = new LDBlock
        {
            characters = new LDCharacter[]
             {
                    // Barbarian 
                    new LDCharacter
                    {
                        name = "Beny the Barbarian",
                        color = 3,
                        role = 0,
                        attributes = new LDAttribute[]
                        {
                            new LDAttribute{ type = 0, value = 80 },
                            new LDAttribute{ type = 1, value = 100 },
                            new LDAttribute{ type = 2, value = 10 },
                            new LDAttribute{ type = 3, value = 50 },
                            new LDAttribute{ type = 4, value = 2 },
                        },
                        timeLine = new LDInputFrame[]
                        {
                            new LDInputFrame { action = 0, cell = 16, },
                            new LDInputFrame { action = 0, cell = 30, },
                            new LDInputFrame { action = 0, cell = 32, },
                        },
                    },
                    // Barbarian 
                    new LDCharacter
                    {
                        name = "Ninni the Necromancer",
                        color = 1,
                        role = 2,
                        attributes = new LDAttribute[]
                        {
                            new LDAttribute{ type = 0, value = 80 },
                            new LDAttribute{ type = 1, value = 100 },
                            new LDAttribute{ type = 2, value = 10 },
                            new LDAttribute{ type = 3, value = 50 },
                            new LDAttribute{ type = 4, value = 1 },
                        },
                        timeLine = new LDInputFrame[]
                        {
                            new LDInputFrame { action = 0, cell = 12, },
                            new LDInputFrame { action = 0, cell = 14, },
                            new LDInputFrame { action = 0, cell = 13, },
                        },
                    },
             },
        };

        Ticker.currentBlock = block;
        Ticker.Instance.Initialize();

        foreach (PlayableAgent player in FindObjectsOfType<PlayableAgent>())
        {
            if (player.Role == Role)
            {
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

    // Start is called before the first frame update
    void Start()
    {
        m_GameStateMachine = new StateMachine(this, new State[] { ActionState, IdleState, LoadingState }, LoadingState);
    }

    // Update is called once per frame
    void Update()
    {
        m_GameStateMachine.Tick();
    }
}


