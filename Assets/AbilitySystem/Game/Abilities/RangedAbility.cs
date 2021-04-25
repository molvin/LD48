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
        Debug.LogError("Not Implemented");
    }

    public override bool IsTargetValid(AbilitySystem Owner)
    {
        Vector2Int TilePos = Owner.OwnerAgent.GridPos;
        Vector2Int TargetPos = Owner.CurrentTarget;

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
