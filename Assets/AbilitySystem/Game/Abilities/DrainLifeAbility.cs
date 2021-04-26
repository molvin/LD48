using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayAbilitySystem;

[CreateAssetMenu(menuName = "Ability System/Ability/DrainLifeAbility")]
public class DrainLifeAbility : GameplayAbility
{
    public int Range;

    public override void Activate(AbilitySystem Owner)
    {
        Commit(Owner);

        AbilitySystem Target = GetEnemyInTile(Owner, Owner.CurrentTarget);
        
        if (Ticker.ShouldVisualize)
        {
            CoroutineRunner.Instance.StartCoroutine(ApplyEffectVisualized(Owner, Target));
        }
        else if(Target != null)
        {
            ApplyEffectToTarget(Owner, Target);
        }
    }

    public override bool IsTargetValid(AbilitySystem Owner)
    {
        Vector2Int TilePos = Owner.OwnerAgent.GridPos;
        Vector2Int TargetPos = Owner.CurrentTarget;

        return Vector2Int.Distance(TilePos, TargetPos) <= Range;
    }
}
