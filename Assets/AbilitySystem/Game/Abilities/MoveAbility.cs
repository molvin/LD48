using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayAbilitySystem;

[CreateAssetMenu(menuName = "Ability System/Ability/Move")]
public class MoveAbility : GameplayAbility
{
    public float timePerSquare = 0.3f;
    public override void Activate(AbilitySystem Owner)
    {
        Commit(Owner);
        if (Ticker.ShouldVisualize)
        {
            List<Vector3Int> path = Grid.findPath((Vector3Int)Owner.OwnerAgent.GridPos, (Vector3Int)Owner.CurrentTarget);
            CoroutineRunner.Instance.StartCoroutine(Move(Owner.OwnerAgent, path));
        }
        else
        {
            Vector3 WorldPos = Grid.CellToWorld((Vector3Int)Owner.CurrentTarget);
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

    public IEnumerator Move(TickAgent OwnerAgent, List<Vector3Int> path, int square = 0)
    {
        //Wait a little before running!
        float time = 0.0f;
        while(time < 0.1f)
        {
            time += Time.deltaTime;
        }

        //Start running!
        OwnerAgent.Animator.SetFloat("Velocity", 1.0f);
        for (int i = 0; i < path.Count; i++)
        {
            //Take a step!
            Vector3 NewPos = Grid.CellToWorld(path[i]);
            yield return Step(OwnerAgent, NewPos);
        }

        //clean up
        OwnerAgent.transform.position = Grid.CellToWorld(path[path.Count-1]);
        OwnerAgent.Animator.SetFloat("Velocity", 0.0f);
    }
    public IEnumerator Step(TickAgent OwnerAgent, Vector3 MoveToWorld)
    {
        float time = 0.0f;
        OwnerAgent.Animator.SetFloat("Velocity", 1.0f);
        Vector3 OriginalPos = OwnerAgent.transform.position;
        OwnerAgent.Animator.transform.localRotation = Quaternion.LookRotation((MoveToWorld - OriginalPos), OwnerAgent.Animator.transform.up);
        OwnerAgent.Animator.transform.localRotation = Quaternion.Euler(0, OwnerAgent.Animator.transform.localRotation.eulerAngles.y, 0);
        while (time < timePerSquare)
        {
            time += Time.deltaTime;
            OwnerAgent.transform.position = Vector3.Lerp(OriginalPos, MoveToWorld, time / timePerSquare);
            yield return null;
        }
    }
}
