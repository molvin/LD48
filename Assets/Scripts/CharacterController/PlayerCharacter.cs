using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayAbilitySystem;
using GameStructure;

public enum CharacterColor
{
    None,
}

public enum CharacterRole
{
    None,
    Barbarian,
    Assassin,
    Necromancer,
}

public class PlayerCharacter : MonoBehaviour
{
    public string Name;
    public int ID;
    public int GridID;
    public CharacterColor Color;
    public CharacterRole Role;

    public AbilitySystem AbilitySystem = new AbilitySystem();
    List<LDInputFrame> TimeLine;

    public void AppendInput(LDInputFrame input)
    {
        TimeLine.Add(input);
    }

    public LDCharacter ToLDCharacter()
    {
        return new LDCharacter
        {
            name = Name,
            Id = ID,
            GridId = GridID,
            color = (byte)Color,
            role = (byte)Role,
            attributes = AbilitySystem.GetLDAttributes(),
            timeLine = TimeLine.ToArray(),
        };
    }
}
