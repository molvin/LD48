using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayAbilitySystem;

[CreateAssetMenu(menuName = "Ability System/Ability/RangeAttack")]
public class RangedAbility : GameplayAbility
{
    public int Range;

    public override void Activate(AbilitySystem Owner)
    {
        Commit(Owner);

        AbilitySystem Target = GetEnemyInTile(Owner, Owner.CurrentTarget);
        if (Target == null)
        {
            return;
        }
        
        if (Ticker.ShouldVisualize)
        {
          CoroutineRunner.Instance.StartCoroutine(ApplyEffectVisualized(Owner, Target));
        }
        else
        {
            ApplyEffectToTarget(Owner, Target);
        }
    }

    public override bool IsTargetValid(AbilitySystem Owner)
    {
        Vector2Int TilePos = Owner.OwnerAgent.GridPos;
        Vector2Int TargetPos = Owner.CurrentTarget;

        if (TilePos == TargetPos)
        {
            return false;
        }

        if (TilePos.x != TargetPos.x && TilePos.y != TargetPos.y)
        {
            return false;
        }

        if (!IsTileOccupiedByEnemy(Owner, TargetPos))
        {
            return false;
        }

        return Vector2Int.Distance(TilePos, TargetPos) <= Range;
    }
}
