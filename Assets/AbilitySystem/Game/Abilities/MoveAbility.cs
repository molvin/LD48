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
        Vector3 NewPos = new Vector3((Pos % 10) * 10, 0, (Pos / 10) * 10);
        if (Ticker.ShouldVisualize)
        {
            CoroutineRunner.Instance.StartCoroutine(Move(Owner.OwnerAgent, NewPos));
        }
        else
        {
            Owner.OwnerAgent.transform.position = NewPos;
        }
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
