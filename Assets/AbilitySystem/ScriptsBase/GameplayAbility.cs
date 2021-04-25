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

        public GridGenerator Grid => GameStateManager.Instance.GetGridManager();

        public abstract void Activate(AbilitySystem Owner);
        public abstract bool IsTargetValid(AbilitySystem Owner);

        protected void ApplyEffectToTarget(AbilitySystem Owner, AbilitySystem Target)
        {
            int PowerModifier = 0;
            if (AbilityTag.Is(TypeTag.StrengthAbility))
            {

            }

            if (AppliedEffect != null)
            {
                GameplayEffect Instance = Instantiate(AppliedEffect);

            }
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
