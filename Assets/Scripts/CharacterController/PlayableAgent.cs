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
    Black,
    Red,
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
    public CharacterColor Color;

    List<LDInputFrame> TimeLine;
    private LDConversionTable Conversion;

    private void Awake()
    {
        Conversion = LDConversionTable.Load();
    }

    public void AppendInput(TypeTag GameplayTag, Vector2Int Position)
    {
        AppendInput(GameplayTag.GetType(), Position);
    }
    public void AppendInput(System.Type GameplayTag, Vector2Int Position)
    {
        int X = GameStateManager.Instance.GetGridManager().xWidth;
        ushort Cell = (ushort)(Position.x + Position.y * X);
        AppendInput(Conversion.GameplayTagToLD(GameplayTag, Cell));
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

        GridPos = (Vector2Int)GameStateManager.Instance.GetGridManager().WorldToCell(transform.position);
        transform.position = GameStateManager.Instance.GetGridManager().CellToWorld((Vector3Int)GridPos);
        GameStateManager.Instance.GetGridManager().setOccupied((Vector3Int)GridPos, true);

        Animator = GetComponentInChildren<Animator>();
        AbilitySystem = new AbilitySystem(this);
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

        CharacterDataTemplate Data = CharacterDataTemplate.Load();

        if (OwningCharacter.attributes == null || OwningCharacter.attributes.Length == 0)
        {
            // NOTE: First time, Playing from start!
            Data.GetStartingAttributes(Role)
                .ForEach(Entry => AbilitySystem.RegisterAttribute(Entry.Attribute, Entry.Value));
        }
        else
        {
            AbilitySystem.RegisterLDAttributes(OwningCharacter.attributes);
        }

        Data.GetAbilities(Role)
            .ForEach(Ability => AbilitySystem.GrantAbility(Ability));

        Data.GetStartingEffects(Role)
            .ForEach(Effect => AbilitySystem.TryApplyEffectToSelf(Effect));
    }

    public override void Tick(int CurrentFrame, bool Scrum)
    {
        AbilitySystem.IsScrumming = Scrum;
        TypeTag Action = Conversion.LDToGameplayTag(TimeLine[CurrentFrame].action);
        int X = GameStateManager.Instance.GetGridManager().xWidth;
        Vector2Int TargetPos = new Vector2Int(TimeLine[CurrentFrame].cell % X, TimeLine[CurrentFrame].cell / X);
        AbilitySystem.CurrentTarget = TargetPos;
        AbilitySystem.TryActivateAbilityByTag(Action);
        AbilitySystem.Tick();
        AbilitySystem.IsScrumming = false;
    }

    public LDCharacter ToLDCharacter()
    {
        return new LDCharacter
        {
            name = Name,
            color = (byte)Color,
            role = (byte)Role,
            attributes = AbilitySystem.GetLDAttributes(),
            timeLine = TimeLine.ToArray(),
        };
    }
}
