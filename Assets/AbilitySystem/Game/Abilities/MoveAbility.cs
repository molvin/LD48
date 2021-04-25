using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayAbilitySystem;

[CreateAssetMenu(menuName = "Ability System/Ability/Move")]
public class MoveAbility : GameplayAbility
{
    public override void Activate(AbilitySystem Owner)
    {
        Commit(Owner);
        Owner.OwnerAgent.GridID = Owner.CurrentTarget;
        int Pos = Owner.OwnerAgent.GridID;
        Owner.OwnerAgent.transform.position = new Vector3((Pos % 10) * 10, 0, (Pos / 10) * 10);
    }

    public override bool IsTargetValid(AbilitySystem Owner)
    {
        int MoveDistance = Owner.GetAttributeValue(Attribute.MovementDistance).Value;
        int CurrentTile = Owner.OwnerAgent.GridID;
        Vector2Int TilePos = new Vector2Int(CurrentTile % 10, CurrentTile / 10);
        int TargetTile = Owner.CurrentTarget;
        Vector2Int TargetPos = new Vector2Int(TargetTile % 10, TargetTile / 10);
        if (Mathf.Abs(TilePos.x - TargetPos.x) + Mathf.Abs(TilePos.y - TargetPos.y) > MoveDistance)
        {
            return false;
        }
        return true;
    }
}
