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
