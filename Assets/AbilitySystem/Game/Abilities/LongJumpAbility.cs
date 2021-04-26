using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayAbilitySystem;

[CreateAssetMenu(menuName = "Ability System/Ability/LongJumpAbility")]
public class LongJumpAbility : GameplayAbility
{
    public override void Activate(AbilitySystem Owner)
    {
        Commit(Owner);

        Vector2Int TargetPos = Owner.CurrentTarget;

        if (Ticker.ShouldVisualize)
        {
            CoroutineRunner.Instance.StartCoroutine(Move(Owner));
        }

        AbilitySystem Target1 = GetEnemyInTile(Owner, TargetPos + Vector2Int.left);
        AbilitySystem Target2 = GetEnemyInTile(Owner, TargetPos + Vector2Int.right);
        AbilitySystem Target3 = GetEnemyInTile(Owner, TargetPos + Vector2Int.up);
        AbilitySystem Target4 = GetEnemyInTile(Owner, TargetPos + Vector2Int.down);

        if (Target1 != null)
        {
            ApplyEffectToTarget(Owner, Target1);
        }
        if (Target2 != null)
        {
            ApplyEffectToTarget(Owner, Target2);
        }
        if (Target3 != null)
        {
            ApplyEffectToTarget(Owner, Target3);
        }
        if (Target4 != null)
        {
            ApplyEffectToTarget(Owner, Target4);
        }

    }

    public override bool IsTargetValid(AbilitySystem Owner)
    {
        Vector2Int TilePos = Owner.OwnerAgent.GridPos;
        Vector2Int TargetPos = Owner.CurrentTarget;

        if (Grid.IsOccupied((Vector3Int)TargetPos))
        {
            return false;
        }

        Vector2Int Dist = (TargetPos - TilePos);
        return Mathf.Abs(Dist.x) + Mathf.Abs(Dist.y) == 1;
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
        OwnerAgent.GridPos = Owner.CurrentTarget;

        yield return PlayParticleSystem(Owner);
    }
}