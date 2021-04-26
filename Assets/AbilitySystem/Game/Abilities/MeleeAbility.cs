using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayAbilitySystem;

[CreateAssetMenu(menuName = "Ability System/Ability/MeleeAttack")]
public class MeleeAbility : GameplayAbility
{
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

        if (!IsTileOccupiedByEnemy(Owner, TargetPos))
        {
            return false;
        }

        Vector2Int Dist = (TargetPos - TilePos);
        return Mathf.Abs(Dist.x) + Mathf.Abs(Dist.y) == 1;
    }
}