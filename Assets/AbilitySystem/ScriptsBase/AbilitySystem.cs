using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameStructure;

namespace GameplayAbilitySystem
{
    public class AbilitySystem
    {
        private Dictionary<Type, int> AttributeSet = new Dictionary<Type, int>();
        private Dictionary<Type, GameplayAbility> GrantedAbilities = new Dictionary<Type, GameplayAbility>();
        private Dictionary<Type, Func<int, int>> AttributeSetCalculations = new Dictionary<Type, Func<int, int>>();
        private Dictionary<Type, Action<int>> OnAttributeChanged = new Dictionary<Type, Action<int>>();
        // Instantiated
        public List<GameplayEffect> ActiveGameplayEffects = new List<GameplayEffect>();
        public Dictionary<Type, int> ActiveTags = new Dictionary<Type, int>();

        public ushort CurrentTarget;
        public bool IsScrumming;

        // Tick
        public void Tick()
        {
            for (int i = ActiveGameplayEffects.Count() - 1; i >= 0; i--)
            {
                GameplayEffect Effect = ActiveGameplayEffects[i];
                Attribute Attribute = Effect.TickAttribute;
                int Value = Effect.TickValue;
                if (Attribute != null)
                {
                    TryApplyAttributeChange(Attribute.GetType(), Value);
                }

                Effect.Duration--;
                if (Effect.Duration <= 0)
                {
                    if (Effect.RevertInitialChangeWhenRemoved && Effect.InitialAttribute != null)
                    {
                        TryApplyAttributeChange(Effect.InitialAttribute.GetType(), -Effect.InitialValue);
                    }

                    foreach (TypeTag Tag in Effect.AppliedTags)
                    {
                        ActiveTags[Tag.GetType()] = ActiveTags[Tag.GetType()] - 1;
                        if (ActiveTags[Tag.GetType()] <= 0)
                        {
                            ActiveTags.Remove(Tag.GetType());
                        }
                    }
                    ActiveGameplayEffects.RemoveAt(i);
                }
            }
        }

        // Register Attribute
        public void RegisterAttribute(Attribute Attribute, int Value)
        {
            RegisterAttribute(Attribute.GetType(), Value);
        }
        public void RegisterAttribute(Type Attribute, int Value)
        {
            AttributeSet.Add(Attribute, Value);
        }

        // Attribute Calculations
        public void RegisterAttributeCalculation(Attribute Attribute, Func<int, int> Calculation)
        {
            RegisterAttributeCalculation(Attribute.GetType(), Calculation);
        }
        public void RegisterAttributeCalculation(Type Attribute, Func<int, int> Calculation)
        {
            if (!AttributeSetCalculations.ContainsKey(Attribute))
            {
                AttributeSetCalculations.Add(Attribute, Calculation);
            }
            else
            {
                AttributeSetCalculations[Attribute] += Calculation;
            }
        }

        // On Attribute Changed
        public void RegisterOnAttributeChanged(Attribute Attribute, Action<int> Callback)
        {
            RegisterOnAttributeChanged(Attribute.GetType(), Callback);
        }
        public void RegisterOnAttributeChanged(Type Attribute, Action<int> Callback)
        {
            if (!OnAttributeChanged.ContainsKey(Attribute))
            {
                OnAttributeChanged.Add(Attribute, Callback);
            }
            else
            {
                OnAttributeChanged[Attribute] += Callback;
            }
        }

        // Get Attribute
        public int? GetAttributeValue(Attribute Attribute)
        {
            return GetAttributeValue(Attribute.GetType());
        }
        public int? GetAttributeValue(Type Attribute)
        {
            if (AttributeSet.ContainsKey(Attribute))
            {
                return AttributeSet[Attribute];
            }
            return null;
        }

        // Blocked
        public bool IsBlocked(List<TypeTag> Tags)
        {
            return Tags.Any(BlockedTag => ActiveTags.Any(ActiveTag => ActiveTag.Key.Is(BlockedTag)));
        }

        // Required
        public bool HasRequired(List<TypeTag> Tags)
        {
            return Tags.All(RequiredTag => ActiveTags.Any(ActiveTag => ActiveTag.Key.Is(RequiredTag)));
        }

        // Apply Effect
        public bool CanApplyFullEffectToSelf(GameplayEffect Effect)
        {
            if (IsBlocked(Effect.BlockedByTags) || !HasRequired(Effect.RequiredTags))
            {
                return false;
            }

            if (Effect.InitialAttribute == null)
            {
                return true;
            }

            if (!AttributeSet.ContainsKey(Effect.InitialAttribute.GetType()))
            {
                return false;
            }

            int? Value = GetAttributeValue(Effect.InitialAttribute);
            if (!Value.HasValue)
            {
                return false;
            }

            return Value.Value - Effect.InitialValue >= 0;
        }

        public bool TryApplyEffectToSelf(GameplayEffect Effect)
        {
            if (IsBlocked(Effect.BlockedByTags) || !HasRequired(Effect.RequiredTags))
            {
                return false;
            }

            if (Effect.InitialAttribute != null)
            {
                TryApplyAttributeChange(Effect.InitialAttribute.GetType(), Effect.InitialValue);
            }

            foreach (TypeTag Tag in Effect.AppliedTags)
            {
                if (!ActiveTags.ContainsKey(Tag.GetType()))
                {
                    ActiveTags.Add(Tag.GetType(), 1);
                }
                else
                {
                    ActiveTags[Tag.GetType()] = ActiveTags[Tag.GetType()] + 1;
                }
            }

            if (Effect.EffectType == EffectDurationType.Duration)
            {
                ActiveGameplayEffects.Add(UnityEngine.Object.Instantiate(Effect));
            }

            return true;
        }

        private bool TryApplyAttributeChange(Type Attribute, int Value)
        {
            if (AttributeSet.ContainsKey(Attribute))
            {
                // Hijack calculation
                if (AttributeSetCalculations.ContainsKey(Attribute))
                {
                    Func<int, int> Calculations = AttributeSetCalculations[Attribute];
                    foreach (var Calculation in Calculations.GetInvocationList())
                    {
                        Value = ((Func<int, int>)Calculation)(Value);
                    }
                }
                AttributeSet[Attribute] -= Value;
                if (OnAttributeChanged.ContainsKey(Attribute))
                {
                    OnAttributeChanged[Attribute].Invoke(AttributeSet[Attribute]);
                }
            }

            return false;
        }

        public void GrantAbility(GameplayAbility Ability)
        {
            GrantedAbilities[Ability.AbilityTag.GetType()] = Ability;
        }

        public void RevokeAbility(TypeTag AbilityTag)
        {
            RevokeAbility(AbilityTag.GetType());
        }
        public void RevokeAbility(Type AbilityTag)
        {
            GrantedAbilities.Remove(AbilityTag);
        }

        public bool TryActivateAbilityByTag(TypeTag Tag)
        {
            return TryActivateAbilityByTag(Tag.GetType());
        }
        public bool TryActivateAbilityByTag(Type Tag)
        {
            GameplayAbility Ability;
            if (GrantedAbilities.TryGetValue(Tag, out Ability))
            {
                if (!IsBlocked(Ability.BlockedByTags) && HasRequired(Ability.RequiredTags))
                {
                    if (Ability.CanActivate(this))
                    {
                        Ability.Activate(this);
                        return true;
                    }
                }
            }

            return false;
        }

        // LD Conversions
        public LDAttribute[] GetLDAttributes()
        {
            LDConversionTable Conversion = LDConversionTable.Load();
            List<LDAttribute> LDAttributes = new List<LDAttribute>();
            foreach (var AttributePair in AttributeSet)
            {
                LDAttributes.Add(Conversion.AttributeToLD(AttributePair.Key, AttributePair.Value));
            }
            return LDAttributes.ToArray();
        }

        public void RegisterLDAttributes(LDAttribute[] LDAttributes)
        {
            LDConversionTable Conversion = LDConversionTable.Load();
            foreach (LDAttribute LDAttribute in LDAttributes)
            {
                Attribute attribute = Conversion.LDToAttribute(LDAttribute.type);
                RegisterAttribute(attribute, LDAttribute.value);
            }
        }
    }
}
