using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayAbilitySystem;

[CreateAssetMenu(menuName = "Ability System/Ability/RangeAttack")]
public class RangedAbility : GameplayAbility
{
    public uint Range;
    public override void Activate(AbilitySystem Owner)
    {
        Commit(Owner);
        Owner.OwnerAgent.GridID = Owner.CurrentTarget;
        int Pos = Owner.OwnerAgent.GridID;
    }

    public override bool IsTargetValid(AbilitySystem Owner)
    {

        int CurrentTile = Owner.OwnerAgent.GridID;
        Vector2Int TilePos = new Vector2Int(CurrentTile % 10, CurrentTile / 10);
        int TargetTile = Owner.CurrentTarget;
        Vector2Int TargetPos = new Vector2Int(TargetTile % 10, TargetTile / 10);

        return Mathf.Abs(TilePos.x - TargetPos.x) + Mathf.Abs(TilePos.y - TargetPos.y) > Range;
    }
    /*
    public IEnumerator DoRangeAttack(TickAgent OwnerAgent, Vector3 NewPos)
    {
        OwnerAgent.Animator.SetFloat("Velocity", 1.0f);
        Vector3 OriginalPos = OwnerAgent.transform.position;
        float time = 0.0f;
        while (time < Ticker.TickVisualTime)
        {
            time += Time.deltaTime;
            OwnerAgent.transform.position = Vector3.Lerp(OriginalPos, NewPos, time / Ticker.TickVisualTime);
            yield return null;
        }
    }*/
}
