using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayAbilitySystem
{
    public abstract class GameplayAbility : ScriptableObject 
    {
        public TypeTag AbilityTag;
        public GameplayEffect AppliedEffect;
        public GameplayEffect Cost;
        public GameplayEffect Cooldown;

        public List<TypeTag> BlockedByTags;
        public List<TypeTag> RequiredTags;

        [Header("Animation")]
        public int AbilityIndex;
        public float MomentOfExecution;
        public ParticleSystem ParticleSystem;
        public string SoundEffect;

        public GridGenerator Grid => GameStateManager.Instance.GetGridManager();

        public abstract void Activate(AbilitySystem Owner);
        public abstract bool IsTargetValid(AbilitySystem Owner);

        protected bool IsInsideGrid(Vector2Int TargetPos)
        {
            return (TargetPos.x >= 0 && TargetPos.x < Grid.xWidth && TargetPos.y >= 0 && TargetPos.y < Grid.zWidth);
        }
        protected void ApplyEffectToTarget(AbilitySystem Owner, AbilitySystem Target)
        {
            int? PowerModifier = null;
            if (AbilityTag.Is(TypeTag.StrengthAbility))
            {
                PowerModifier = Owner.GetAttributeValue(Attribute.Strength);
            }
            else if (AbilityTag.Is(TypeTag.DexterityAbility))
            {
                PowerModifier = Owner.GetAttributeValue(Attribute.Dexterity);
            }
            else if (AbilityTag.Is(TypeTag.IntelligenceAbility))
            {
                PowerModifier = Owner.GetAttributeValue(Attribute.Intelligence);
            }

            if (AppliedEffect != null)
            {
                GameplayEffect Instance = Instantiate(AppliedEffect);
                if (PowerModifier.HasValue)
                {
                    if (Instance.InitialAttribute != null)
                    {
                        Instance.InitialValue += PowerModifier.Value / 2;
                    }
                    if (Instance.TickAttribute != null)
                    {
                        Instance.TickValue += PowerModifier.Value / 6;
                    }
                }

                int? AddedDamage = Owner.GetAttributeValue(Attribute.AddedDamage);
                if (AddedDamage.HasValue && Owner != Target)
                {
                    if (Instance.InitialAttribute != null && Instance.InitialAttribute.Is(Attribute.Health))
                    {
                        Instance.InitialValue += AddedDamage.Value;
                    }
                    if (Instance.TickAttribute != null && Instance.TickAttribute.Is(Attribute.Health))
                    {
                        Instance.TickValue += AddedDamage.Value / 3;
                    }
                }

                Target.TryApplyEffectToSelf(Instance);
            }
        }

        protected bool IsTileOccupiedByEnemy(AbilitySystem Owner, Vector2Int TargetPos)
        {
            return GetEnemyInTile(Owner, TargetPos) != null;
        }

        protected AbilitySystem GetEnemyInTile(AbilitySystem Owner, Vector2Int TargetPos)
        { 
            if (TargetPos.x < 0 || TargetPos.x >= Grid.xWidth || TargetPos.y < 0 || TargetPos.y >= Grid.zWidth)
            {
                return null;
            }

            TickAgent TargetAgent = null;
            foreach (TickAgent Agent in Ticker.Instance.tickAgents)
            {
                if (Agent.IsAlive && Agent.GridPos == TargetPos)
                {
                    TargetAgent = Agent;
                    break;
                }
            }

            if (TargetAgent == null || !TargetAgent.IsAlive)
            {
                return null;
            }

            if (Owner.OwnerAgent.GetType() == TargetAgent.GetType())
            {
                return null;
            }

            return TargetAgent.AbilitySystem;
        }
        protected bool IsTileOccupiedByFriendly(AbilitySystem Owner, Vector2Int TargetPos)
        {
            return GetFriendlyInTile(Owner, TargetPos) != null;
        }

        protected AbilitySystem GetFriendlyInTile(AbilitySystem Owner, Vector2Int TargetPos)
        { 
            if (TargetPos.x < 0 || TargetPos.x >= Grid.xWidth || TargetPos.y < 0 || TargetPos.y >= Grid.zWidth)
            {
                return null;
            }

            TickAgent TargetAgent = null;
            foreach (TickAgent Agent in Ticker.Instance.tickAgents)
            {
                if (Agent.IsAlive && Agent.GridPos == TargetPos)
                {
                    TargetAgent = Agent;
                    break;
                }
            }

            if (TargetAgent == null || !TargetAgent.IsAlive)
            {
                return null;
            }

            if (Owner.OwnerAgent.GetType() != TargetAgent.GetType())
            {
                return null;
            }

            return TargetAgent.AbilitySystem;
        }

        protected IEnumerator ApplyEffectVisualized(AbilitySystem Owner, AbilitySystem Target)
        {
            Owner.OwnerAgent.Animator.SetInteger("AbilityIndex", AbilityIndex);
            Owner.OwnerAgent.Animator.SetTrigger("Ability");

            yield return new WaitForSeconds(MomentOfExecution);

            AudioSystem.Play(SoundEffect);
            if (ParticleSystem != null)
            {
                //Vector3 WorldPos = Grid.CellToWorld((Vector3Int)Owner.OwnerAgent.GridPos);
                //Vector3 TargetWorldPos = Grid.CellToWorld((Vector3Int)Target.OwnerAgent.GridPos);
                Vector3 WorldPos = Owner.OwnerAgent.transform.position;
                Vector3 TargetWorldPos = Target.OwnerAgent.transform.position;
                ParticleSystem Instance = Instantiate(ParticleSystem, WorldPos, Quaternion.identity);
                Instance.transform.forward = TargetWorldPos - WorldPos;
                Instance.Play();

                while(Instance.isPlaying)
                {
                    yield return null;
                }
                Destroy(Instance.gameObject);
            }

            ApplyEffectToTarget(Owner, Target);
        }

        protected IEnumerator PlayParticleSystem(AbilitySystem Owner)
        {
            Vector3 WorldPos = Owner.OwnerAgent.transform.position;
            ParticleSystem Instance = Instantiate(ParticleSystem, WorldPos, Quaternion.identity);
            Instance.Play();

            while(Instance.isPlaying)
            {
                yield return null;
            }
            Destroy(Instance.gameObject);
        }

        public void Commit(AbilitySystem Owner)
        {
            if (Cost != null)
            {
                Debug.Assert(Owner.TryApplyEffectToSelf(Cost));
            }
            if (Cooldown != null)
            {
                Debug.Assert(Owner.TryApplyEffectToSelf(Cooldown));
            }
        }    

        public bool CanActivate(AbilitySystem Owner, bool IgnoreTarget = false)
        {
            if (Owner.IsBlocked(BlockedByTags) || !Owner.HasRequired(RequiredTags))
            {
                return false;
            }

            if (!IgnoreTarget && !IsTargetValid(Owner))
            {
                return false;
            }

            if (Cost == null)
            {
                return true;
            }

            return Owner.CanApplyFullEffectToSelf(Cost);
        }
    }
}
