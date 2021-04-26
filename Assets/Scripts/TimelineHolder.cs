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

    private LDBlock m_CurrentBlock;
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

        m_ServerTimeLine = new LDTimeLine(); //Server.RequestTimeLine();
        m_ServerTimeLine.timeLine = new LDBlock[0];

        if (m_ServerTimeLine.timeLine.Length <= 0)
        {
            m_BranchingIndex = 0;

            LDConversionTable LDConversionTable = LDConversionTable.Load();
            LDInputFrame death_input_frame = new LDInputFrame { action = LDConversionTable.GameplayTagToLDID(TypeTag.DeathAction), cell = 0 };
            LDInputFrame temp_input_frame = new LDInputFrame();


            m_CurrentBlock.characters = new LDCharacter[3];
            m_CurrentBlock.characters[0].role = (byte)CharacterRole.Assassin;
            m_CurrentBlock.characters[0].timeLine = new LDInputFrame[1];

            temp_input_frame.action = LDConversionTable.GameplayTagToLDID(TypeTag.MoveAbility);
            temp_input_frame.cell = 20 * 10 + 10;
            m_CurrentBlock.characters[0].timeLine[0] = temp_input_frame;

            m_CurrentBlock.characters[1].role = (byte)CharacterRole.Barbarian;
            m_CurrentBlock.characters[1].timeLine = new LDInputFrame[2];

            temp_input_frame.cell = 20 * 10 + 8;
            m_CurrentBlock.characters[1].timeLine[0] = temp_input_frame;
            m_CurrentBlock.characters[1].timeLine[1] = death_input_frame;

            m_CurrentBlock.characters[2].role = (byte)CharacterRole.Necromancer;
            m_CurrentBlock.characters[2].timeLine = new LDInputFrame[3];

            temp_input_frame.cell = 20 * 10 + 10;
            m_CurrentBlock.characters[2].timeLine[0] = temp_input_frame;
            temp_input_frame.cell = 20 * 11 + 10;
            m_CurrentBlock.characters[2].timeLine[1] = temp_input_frame;
            temp_input_frame.cell = 20 * 11 + 11;
            m_CurrentBlock.characters[2].timeLine[2] = death_input_frame;


            //m_TimelineExtetion.Add(m_CurrentBlock);
        }  
    }

    public LDBlock GiveNextRelevantBlock()
    {
        //  LDBlock temp_block = new LDBlock();

        m_CurrentBlock.level = (byte)SceenIndex;

        return m_CurrentBlock;
      
    }

    public LDBlock GetCurrentBlock()
    {
        return m_CurrentBlock;
    }
}
