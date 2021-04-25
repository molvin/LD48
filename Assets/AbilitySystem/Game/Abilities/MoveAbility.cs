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

        Vector3 WorldPos = Grid.CellToWorld((Vector3Int)Owner.CurrentTarget);

        if (Ticker.ShouldVisualize)
        {
            CoroutineRunner.Instance.StartCoroutine(Move(Owner.OwnerAgent, WorldPos));
        }
        else
        {
            Owner.OwnerAgent.transform.position = WorldPos;
        }

        Grid.setOccupied((Vector3Int)Owner.OwnerAgent.GridPos, false);

        Owner.OwnerAgent.GridPos = Owner.CurrentTarget;

        Grid.setOccupied((Vector3Int)Owner.OwnerAgent.GridPos, true);
    }

    public override bool IsTargetValid(AbilitySystem Owner)
    {
        if (Owner.OwnerAgent.GridPos == Owner.CurrentTarget)
        {
            return false;
        }

        int MoveDistance = Owner.GetAttributeValue(Attribute.MovementDistance).Value;
        List<Vector3Int> Path = Grid.findPath((Vector3Int)Owner.OwnerAgent.GridPos, (Vector3Int)Owner.CurrentTarget);

        if (Path == null || Path.Count > MoveDistance)
        {
            return false;
        }

        return true;
    }

    public IEnumerator Move(TickAgent OwnerAgent, Vector3 NewPos)
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
        OwnerAgent.transform.position = NewPos;
        OwnerAgent.Animator.SetFloat("Velocity", 0.0f);
    }
}
