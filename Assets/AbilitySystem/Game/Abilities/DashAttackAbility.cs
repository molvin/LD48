using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayAbilitySystem;

[CreateAssetMenu(menuName = "Ability System/Ability/DashAttackAbility")]
public class DashAttackAbility : GameplayAbility
{
    public override void Activate(AbilitySystem Owner)
    {
        Commit(Owner);

        Debug.Assert(false, "NOT DONE");

        // Find target cell.
        Vector2Int CurrentPos = Owner.OwnerAgent.GridPos;
        Vector2Int TargetPos = Owner.CurrentTarget;
        Vector2Int Delta = TargetPos - CurrentPos;

        Vector2Int Direction = Vector2Int.zero;
        if (Delta.x > 0)
            Direction = Vector2Int.right;
        if (Delta.x < 0)
            Direction = Vector2Int.left;
        if (Delta.y > 0)
            Direction = Vector2Int.up;
        if (Delta.x < 0)
            Direction = Vector2Int.down;

        if (Direction == Vector2Int.zero)
        {
            return;
        }

        // Seek target
        TargetPos = CurrentPos + Delta;
        if (!IsInsideGrid(TargetPos))
        {
            return;
        }

        while (IsInsideGrid(TargetPos))
        {
            if (IsInsideGrid(TargetPos + Delta) && !Grid.IsOccupied((Vector3Int)(TargetPos + Delta)))
            {
                TargetPos += Delta;
            }
            else
                break;
        }

        Owner.CurrentTarget = TargetPos;

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

        if (TargetPos.x < 0 || TargetPos.x >= Grid.xWidth && TargetPos.y < 0 && TargetPos.y >= Grid.zWidth)
        {
            return false;
        }

        return true;
    }
    public IEnumerator Move(AbilitySystem Owner)
    {
        TickAgent OwnerAgent = Owner.OwnerAgent;
        OwnerAgent.Animator.SetInteger("AbilityIndex", AbilityIndex);
        OwnerAgent.Animator.SetTrigger("Ability");

        Vector3 OriginalPos = OwnerAgent.transform.position;
        Vector3 NewPos = Grid.CellToWorld((Vector3Int)Owner.CurrentTarget);

        float time = 0.0f;
        while (time < Ticker.TickVisualTime)
        {
            time += Time.deltaTime;
            OwnerAgent.transform.position = Vector3.Lerp(OriginalPos, NewPos, time / Ticker.TickVisualTime);
            yield return null;
        }
        OwnerAgent.transform.position = NewPos;

        yield return PlayParticleSystemOnSelf(Owner);
    }
}
