using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStructure;

public class TimelineHolder : MonoBehaviour
{
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

        m_TestBlock.characters = new LDCharacter[3];
        m_TestBlock.characters[0].role = (byte)CharacterRole.Assassin;
        m_TestBlock.characters[1].role = (byte)CharacterRole.Barbarian;
        m_TestBlock.characters[2].role = (byte)CharacterRole.Necromancer;
    }

    public int GetNextSceneIndex()
    {
        return 0;
    }

    public LDBlock GetCurrentBlock()
    {
        return m_TestBlock;
    }
}
