using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStructure;
using GameplayAbilitySystem;
using UnityEngine.SceneManagement;

public class TimelineHolder : MonoBehaviour
{
    [Tooltip("The scene you want to start att -1 = means the ticker chooses")]
    public int SceenIndex;

    private LDTimeLine m_ServerTimeLine;
    private List<LDBlock> m_TimelineExtetion;
    private int m_BranchingIndex = 0;
    public static TimelineHolder Instance;

    private LDBlock m_CurrentBlock = new LDBlock();

    LDInputFrame m_DeathInputFrame;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        LDConversionTable LDConversionTable = LDConversionTable.Load();
        m_DeathInputFrame = new LDInputFrame { action = LDConversionTable.GameplayTagToLDID(TypeTag.DeathAction), cell = 0 };

        m_ServerTimeLine = Server.RequestTimeLine();
        if (m_ServerTimeLine.timeLine.Length <= 0)
        {
            m_ServerTimeLine.timeLine = new LDBlock[0];
            m_BranchingIndex = 0;

            m_CurrentBlock.branches = new int[0];
            m_CurrentBlock.characters = new LDCharacter[3];
            m_CurrentBlock.characters[0].role = (byte)CharacterRole.Assassin;
            m_CurrentBlock.characters[0].timeLine = new LDInputFrame[1];
            m_CurrentBlock.characters[0].timeLine[0] = m_DeathInputFrame;

            m_CurrentBlock.characters[1].role = (byte)CharacterRole.Barbarian;
            m_CurrentBlock.characters[1].timeLine = new LDInputFrame[2];
            m_CurrentBlock.characters[1].timeLine[0] = m_DeathInputFrame;

            m_CurrentBlock.characters[2].role = (byte)CharacterRole.Necromancer;
            m_CurrentBlock.characters[2].timeLine = new LDInputFrame[3];
            m_CurrentBlock.characters[2].timeLine[0] = m_DeathInputFrame;
        }  
    }
    public void SaveCurrentBlock()
    {
        if (GameStateManager.Instance.PlayerAgent)
        {
            Debug.LogError("Character is not loaded");
            return;
        }    

        for (int i = 0; i < m_CurrentBlock.characters.Length; i++)
        {
            if (m_CurrentBlock.characters[i].role == (byte)GameStateManager.Instance.PlayerAgent.Role)
            {
                m_CurrentBlock.characters[i] = GameStateManager.Instance.PlayerAgent.AppendTimeline(m_CurrentBlock.characters[i]);
                break;
            }
        }
        m_TimelineExtetion.Add(m_CurrentBlock);
    }
    public void PushToServer()
    {
        SaveCurrentBlock();
        //Push it to the server!
        LDTimeLineBranchRequest temp = new LDTimeLineBranchRequest
        {
            branchBlockIndex = m_BranchingIndex,
            timeLine = m_TimelineExtetion.ToArray()
        };
        Server.PushTimeLine(temp);
    }
    public LDBlock GenerateNextRelevantBlock()
    {
        LDBlock temp_block = new LDBlock();
        //If the sceen index is zero debug level
        if (SceenIndex != -1)
        {
            temp_block.level = (ushort)SceenIndex;
            return temp_block;
        }

        //Select the first branch
        if (m_CurrentBlock.branches.Length > 0)
        {
            //Maybe chosie 
            temp_block = m_ServerTimeLine.timeLine[m_CurrentBlock.branches[0]];
        }
        else //If there are no branches then we create a new one
        {
            temp_block.level = (ushort)Random.Range(1, SceneManager.sceneCountInBuildSettings);
            temp_block.mods = m_CurrentBlock.mods;
            foreach (LDCharacter character in m_CurrentBlock.characters)
            {
                if(GameStateManager.Instance.PlayerAgent != null)
                {
                    if ((byte)GameStateManager.Instance.PlayerAgent.Role == character.role)
                    {
                        LDCharacter current_character = GameStateManager.Instance.PlayerAgent.ToLDCharacter();
                        current_character.timeLine = new LDInputFrame[1];
                        current_character.timeLine[0] = m_DeathInputFrame;
                        temp_block.characters = new LDCharacter[] { current_character };
                        break;
                    }
                }
                else
                {
                    temp_block.characters = new LDCharacter[] { character };
                    break;
                }

        
            }

            temp_block.branches = new int[0];
        }

        m_CurrentBlock = temp_block;

        return m_CurrentBlock;
    }
    public LDBlock GetCurrentBlock()
    {
        return m_CurrentBlock;
    }
}
