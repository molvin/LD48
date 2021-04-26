using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayAbilitySystem;
using Testing;

[CreateAssetMenu(menuName = "Ability System/Ability/RangeAttack")]
public class RangedAbility : GameplayAbility
{
    public bool CanTargetEnemy = true; 
    public bool CanTargetFriendy = true;
    public bool CanBeBlockedByObstacle;
    public int Range;
    public GameObject Projectile;
    public float ProjectileFlyTime;
    public float ProjectileArcHeight;

    public override void Activate(AbilitySystem Owner)
    {
        Commit(Owner);
        AbilitySystem Target = GetEnemyInTile(Owner, Owner.CurrentTarget);
       
        
        if (Ticker.ShouldVisualize)
        {
            if (Projectile != null)
            {
                CoroutineRunner.Instance.StartCoroutine(EffectVisualized(Owner, Target));
            }
            else if (FromSelfToTargetParticleSystem != null)
            {
                CoroutineRunner.Instance.StartCoroutine(ApplyEffectVisualized(Owner, Target));
            } 
            else
            {
                ApplyEffectToTarget(Owner, Target);
            }
        }
        else if (Target != null)
        {
            ApplyEffectToTarget(Owner, Target);
        }
    }





    IEnumerator EffectVisualized(AbilitySystem Owner, AbilitySystem Target)
    {
        Owner.OwnerAgent.Animator.SetInteger("AbilityIndex", AbilityIndex);
        Owner.OwnerAgent.Animator.SetTrigger("Ability");
        yield return new WaitForSeconds(MomentOfExecution);
        AudioSystem.Play(SoundEffect);
        
        float time = 0;
        Vector3 OriginalPos = Owner.OwnerAgent.transform.position;
        Vector3 NewPos = Grid.CellToWorld((Vector3Int)Owner.CurrentTarget);
        GameObject projectile = null;
        RenderLayer RL = null;
        if(Projectile != null)
        {
            projectile = Instantiate(Projectile, OriginalPos, Quaternion.identity);
            RL = projectile.GetComponent<RenderLayer>(); ;
        }
        while (time < ProjectileFlyTime)
        {
            time += Time.deltaTime;
            float heigt = Mathf.Lerp(0, ProjectileArcHeight, time / ProjectileFlyTime);
            if(projectile != null && RL != null)
            {
                projectile.transform.position = Vector3.Lerp(OriginalPos, NewPos, time / ProjectileFlyTime) + Vector3.forward * heigt;
                if (RL != null) { RL.Height = heigt; }
            }
            yield return null;
        }
        CoroutineRunner.Instance.StartCoroutine(PlayParticleSystemOnTarget(Owner));
        if(Target != null)
        {
            ApplyEffectToTarget(Owner, Target);
        }
        
    }

    public override bool IsTargetValid(AbilitySystem Owner)
    {
        Vector2Int TilePos = Owner.OwnerAgent.GridPos;
        Vector2Int TargetPos = Owner.CurrentTarget;

        //SelfTarget
        if (TilePos == TargetPos)
        {
            return false;
        }
        //Is Cardinal
        if (TilePos.x != TargetPos.x && TilePos.y != TargetPos.y)
        {
            return false;
        }
        //Can target enemy
        if (IsTileOccupiedByEnemy(Owner, TargetPos) && !CanTargetEnemy)
        {
            return false;
        }
        //Can target friendly
        if (IsTileOccupiedByFriendly(Owner, TargetPos) && !CanTargetFriendy)
        {
            return false;
        }

        //Blocked by obsticle
        Vector2Int direction = (TargetPos - TilePos);
        int targetRange = (int)Vector2Int.Distance(TilePos, TargetPos);
        if (direction.x != 0)
            direction.x = (int)Mathf.Sign(direction.x);
        if (direction.y != 0)
            direction.y = (int)Mathf.Sign(direction.y);
        if (FlagsAlongLine(TilePos, direction, targetRange, GridGenerator.BlockStatus.Obstacle) && CanBeBlockedByObstacle)
            return false;

        //Is in range
        return targetRange <= Range;
    }
   
}
