using System.Collections;
using System.Collections.Generic;
using GameStructure;
using UnityEngine;
using GameplayAbilitySystem;

public enum MonsterRole
{
    BlobManA,
    BlobManB,
    CultistA,
    CultistB,
    CultistC,
}

public class EnemyAgent : TickAgent
{
    public MonsterRole Role;

    public override void Initialize(LDBlock _data)
    {
        GridPos = (Vector2Int)GameStateManager.Instance.GetGridManager().WorldToCell(transform.position);
        transform.position = GameStateManager.Instance.GetGridManager().CellToWorld((Vector3Int)GridPos);
        GameStateManager.Instance.GetGridManager().setOccupied((Vector3Int)GridPos, true);

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

    public void AddActions()
    {

    }
}
