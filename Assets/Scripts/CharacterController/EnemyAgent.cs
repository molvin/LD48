using System.Collections;
using System.Collections.Generic;
using GameStructure;
using UnityEngine;
using GameplayAbilitySystem;

public enum MonsterRole
{
    BlobMan,
    Cultist,
}

public class EnemyAgent : TickAgent
{
    public MonsterRole Role;

    public override void Initialize(LDBlock _data)
    {
        Vector3Int Pos = GameStateManager.Instance.GetGridManager().WorldToCell(transform.position);
        GridPos = new Vector2Int(Pos.x, Pos.z);
        Animator = GetComponentInChildren<Animator>();
        AbilitySystem = new AbilitySystem(this);
        CharacterDataTemplate Data = CharacterDataTemplate.Load();
        Data.GetMonsterStartingAttributes(Role)
            .ForEach(Entry => AbilitySystem.RegisterAttribute(Entry.Attribute, Entry.Value));
        Data.GetMonsterAbilities(Role)
            .ForEach(Ability => AbilitySystem.GrantAbility(Ability));
        Data.GetMonsterStartingEffects(Role)
            .ForEach(Effect => AbilitySystem.TryApplyEffectToSelf(Effect));
    }

    public override void Tick(int Frame, bool Scrum)
    {
        Debug.LogError("This monster doesn't have a brain, very sadge :(");
    }
}
