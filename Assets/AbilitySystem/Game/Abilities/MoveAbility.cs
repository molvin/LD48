using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayAbilitySystem;

[CreateAssetMenu(menuName = "Ability System/Ability/Move")]
public class MoveAbility : GameplayAbility
{
    public override void Activate(AbilitySystem Owner)
    {
        Owner.OwnerAgent.GridID = Owner.CurrentTarget;
    }

    public override bool IsTargetValid(AbilitySystem Owner)
    {
        int MoveDistance = Owner.GetAttributeValue(Attribute.MovementDistance).Value;
        int CurrentTile = Owner.OwnerAgent.GridID;
        int TargetTile = Owner.CurrentTarget;
        if (Mathf.Abs(CurrentTile - TargetTile) > MoveDistance)
        {
            return false;
        }
        return true;
    }
}
