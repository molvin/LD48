using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayAbilitySystem;
using GameStructure;
using System.Linq;

public enum CharacterColor
{
    None,
    Blue,
}

public enum CharacterRole
{
    Barbarian,
    Assassin,
    Necromancer,
}

public class PlayableAgent : TickAgent
{
    public CharacterRole Role;

    [HideInInspector]
    public string Name;
    [HideInInspector]
    public int GridID;
    [HideInInspector]
    public CharacterColor Color;

    public AbilitySystem AbilitySystem;
    List<LDInputFrame> TimeLine;
    private LDConversionTable Conversion;

    private void Awake()
    {
        Conversion = LDConversionTable.Load();
    }

    public void AppendInput(LDInputFrame input)
    {
        TimeLine.Add(input);
    }

    public override void Initialize(LDBlock data)
    {
        LDCharacter OwningCharacter = new LDCharacter();
        foreach (LDCharacter Character in data.characters)
        {
            if (Character.role == (byte)Role)
            {
                OwningCharacter = Character;
            }
        }

        AbilitySystem = new AbilitySystem();
        Name = OwningCharacter.name;
        Color = (CharacterColor)OwningCharacter.color;
        if (OwningCharacter.timeLine != null)
        {
            TimeLine = OwningCharacter.timeLine.ToList();
        }
        else
        {
            TimeLine = new List<LDInputFrame>();
        }
        AbilitySystem.RegisterLDAttributes(OwningCharacter.attributes);
        CharacterDataTemplate.
            Load().
            GetAbilities(Role).
            ForEach(Ability => AbilitySystem.GrantAbility(Ability));
    }

    public override void Tick(int CurrentFrame, bool Scrum)
    {
        AbilitySystem.IsScrumming = Scrum;
        TypeTag Action = Conversion.LDToGameplayTag(TimeLine[CurrentFrame].action);
        AbilitySystem.CurrentTarget = TimeLine[CurrentFrame].cell;
        AbilitySystem.TryActivateAbilityByTag(Action);
        AbilitySystem.Tick();
        AbilitySystem.IsScrumming = false;
    }

    public LDCharacter ToLDCharacter()
    {
        return new LDCharacter
        {
            name = Name,
            GridId = GridID,
            color = (byte)Color,
            role = (byte)Role,
            attributes = AbilitySystem.GetLDAttributes(),
            timeLine = TimeLine.ToArray(),
        };
    }
}
