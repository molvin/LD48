using System.Collections;
using System.Collections.Generic;
using GameStructure;
using UnityEngine;
using GameplayAbilitySystem;
using System.Linq;

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

    private TypeTag MainAbility;

    bool IsBlob => Role == MonsterRole.BlobManA || Role == MonsterRole.BlobManB;
    bool IsCultist => !IsBlob;

    public override void Initialize(LDBlock _data)
    {
        GridPos = (Vector2Int)Grid.WorldToCell(transform.position);
        transform.position = Grid.CellToWorld((Vector3Int)GridPos);
        Grid.setOccupied((Vector3Int)GridPos, true);

        Animator = GetComponentInChildren<Animator>();
        AbilitySystem = new AbilitySystem(this);

        CharacterDataTemplate Data = CharacterDataTemplate.Load();
        Data.GetMonsterStartingAttributes(Role)
            .ForEach(Entry => AbilitySystem.RegisterAttribute(Entry.Attribute, Entry.Value));
        Data.GetMonsterAbilities(Role)
            .ForEach(Ability => AbilitySystem.GrantAbility(Ability));
        Data.GetMonsterStartingEffects(Role)
            .ForEach(Effect => AbilitySystem.TryApplyEffectToSelf(Effect));


        AbilitySystem.RegisterOnAttributeChanged(Attribute.Health, OnDamageTaken);
        CurrentHealth = AbilitySystem.GetAttributeValue(Attribute.Health).Value;

        AbilitySystem.RegisterOnAttributeChanged(Attribute.MaxHealth, UpdateMaxHealth);
        MaxHealth = AbilitySystem.GetAttributeValue(Attribute.MaxHealth).Value;

        AbilitySystem
            .GetGrantedAbilityTypes()
            .ForEach(Ability =>
            {
                if (!Ability.Is(TypeTag.MoveAbility))
                {
                    MainAbility = Ability;
                }
            });
    }

    public override void Tick(int _Frame, bool Scrum)
    {
        (System.Type, Vector2Int) Action = SelectAction();

        AbilitySystem.IsScrumming = Scrum;
        AbilitySystem.CurrentTarget = Action.Item2;
        AbilitySystem.TryActivateAbilityByTag(Action.Item1);
        AbilitySystem.Tick();
        AbilitySystem.IsScrumming = false;
    }

    public (System.Type, Vector2Int) SelectAction()
    {
        List<PlayableAgent> Players = FindObjectsOfType<PlayableAgent>().Where(Player => Player.IsAlive).ToList();

        Vector2Int? TargetPos = SelectTargetForAttackFromPosition(Players, GridPos);
        if (TargetPos.HasValue)
        {
            return (MainAbility.GetType(), TargetPos.Value);
        }
        else
        {
            Vector2Int? MoveTarget = null;
            if (IsBlob)
            {
                MoveTarget = BlobMoveTarget(Players);
            }
            else
            {
                MoveTarget = CultistMoveTarget(Players);
            }
                

            if (MoveTarget.HasValue)
            {
                return (TypeTag.MoveAbility, MoveTarget.Value);
            }
            else
            {
                return (TypeTag.NoAction, Vector2Int.zero);
            }
        }
    }

    private Vector2Int? BlobMoveTarget(List<PlayableAgent> Players)
    {
        Vector3Int OwnerPos = (Vector3Int)GridPos;

        PlayableAgent BestAgent = null;
        List<Vector3Int> BestPath = null;
        foreach (PlayableAgent Player in Players)
        {
            Vector3Int PlayerPos = (Vector3Int)Player.GridPos;
            Grid.setOccupied(PlayerPos, false);

            List<Vector3Int> Path = Grid.findPath(OwnerPos, PlayerPos);
            if (Path != null)
            {
                // First or closest
                if (BestPath == null || (Path.Count < BestPath.Count))
                {
                    BestPath = Path;
                    BestAgent = Player;
                }    
                else if (Path.Count == BestPath.Count)
                {
                        int BestHealth = BestAgent.AbilitySystem.GetAttributeValue(Attribute.Health).Value;
                        int NewHealth = Player.AbilitySystem.GetAttributeValue(Attribute.Health).Value;

                        // Lowest health
                        if (NewHealth < BestHealth)
                        {
                            BestPath = Path;
                            BestAgent = Player;
                        }
                        else if (BestHealth == NewHealth)
                        {
                            // First role
                            if (Player.Role < BestAgent.Role)
                            {
                                BestPath = Path;
                                BestAgent = Player;
                            }
                        }
                }
            }
            Grid.setOccupied(PlayerPos, true);
        }

        Vector2Int? TargetPosition = null;
        if (BestPath != null)
        {
            for (int i = BestPath.Count - 1; i >= 0; i--)
            {
                if (AbilitySystem.CanActivateTargetAbilityByTag(TypeTag.MoveAbility, (Vector2Int)BestPath[i]))
                {
                    TargetPosition = (Vector2Int)BestPath[i];
                    break;
                } 
            }
        }
        return TargetPosition;
    }

    private Vector2Int? CultistMoveTarget(List<PlayableAgent> Players)
    {
        for (int z = 0; z < Grid.zWidth; z++)
        {
            for (int x = 0; x < Grid.xWidth; x++)
            {
                Vector2Int TestPosition = new Vector2Int(x, z);
                Vector2Int? SelectPos = SelectTargetForAttackFromPosition(Players, TestPosition);
                if (SelectPos.HasValue)
                {
                    return SelectPos;
                }
            }
        }
        return null;
    }

    private Vector2Int? SelectTargetForAttackFromPosition(List<PlayableAgent> Players, Vector2Int Pos)
    {
        if (!AbilitySystem.CanActivateAbilityByTag(MainAbility))
        {
            return null;
        }

        Vector2Int OriginalPosition = GridPos;
        GridPos = Pos;
        Grid.setOccupied((Vector3Int)OriginalPosition, false);
        Grid.setOccupied((Vector3Int)GridPos, true);

        PlayableAgent TargetAgent = null;
        foreach (PlayableAgent Player in Players)
        {
            // Can target
            if (AbilitySystem.CanActivateTargetAbilityByTag(MainAbility, Player.GridPos))
            {
                // First
                if (TargetAgent == null)
                {
                    TargetAgent = Player;
                }
                else
                {
                    float BestDist = Vector2Int.Distance(GridPos, TargetAgent.GridPos);
                    float NewDist = Vector2Int.Distance(GridPos, Player.GridPos);

                    // Closest
                    if (NewDist < BestDist)
                    {
                        TargetAgent = Player;
                    }
                    else if (NewDist == BestDist)
                    {
                        int BestHealth = TargetAgent.AbilitySystem.GetAttributeValue(Attribute.Health).Value;
                        int NewHealth = Player.AbilitySystem.GetAttributeValue(Attribute.Health).Value;
                        // Lowest health
                        if (NewHealth < BestHealth)
                        {
                            TargetAgent = Player;
                        }
                        else if (BestHealth == NewHealth)
                        {
                            // First role
                            if (Player.Role < TargetAgent.Role)
                            {
                                TargetAgent = Player;
                            }
                        }
                    }
                }
            }
        }

        Grid.setOccupied((Vector3Int)GridPos, false);
        Grid.setOccupied((Vector3Int)OriginalPosition, true);
        GridPos = OriginalPosition;

        if (TargetAgent != null)
        {
            return TargetAgent.GridPos;
        }
        return null;
    }

}
