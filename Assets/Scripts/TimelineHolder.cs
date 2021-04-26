using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStructure;
using GameplayAbilitySystem;

public class TimelineHolder : MonoBehaviour
{
    [Tooltip("The scene you want to start att -1 = means the ticker chooses")]
    public int SceenIndex;

    public LDTimeLine TimeLine;
    public static TimelineHolder Instance;

    private LDBlock m_TestBlock;

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

        TimeLine = Server.RequestTimeLine();

        LDConversionTable LDConversionTable = LDConversionTable.Load();
        LDInputFrame input_frame = new LDInputFrame { action = LDConversionTable.GameplayTagToLDID(TypeTag.DeathAction), cell = 0 };

        m_TestBlock.characters = new LDCharacter[3];
        m_TestBlock.characters[0].role = (byte)CharacterRole.Assassin;
        m_TestBlock.characters[0].timeLine = new LDInputFrame[1];
        m_TestBlock.characters[0].timeLine[0] = input_frame;

        m_TestBlock.characters[1].role = (byte)CharacterRole.Barbarian;
        m_TestBlock.characters[1].timeLine = new LDInputFrame[1];
        m_TestBlock.characters[1].timeLine[0] = input_frame;

        m_TestBlock.characters[2].role = (byte)CharacterRole.Necromancer;
        m_TestBlock.characters[2].timeLine = new LDInputFrame[1];
        m_TestBlock.characters[2].timeLine[0] = input_frame;
    }

    public int GetNextSceneIndex()
    {
        if(SceenIndex == -1)
        {
            //TODO: Add the sceen getting thing from server
            return 1;
        }
        return SceenIndex;
    }

    public LDBlock GetCurrentBlock()
    {
        return m_TestBlock;
    }
}
