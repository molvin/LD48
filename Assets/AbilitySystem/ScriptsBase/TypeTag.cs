using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayAbilitySystem
{
    public abstract class TypeTag : ScriptableObject
    {
        public bool Is(TypeTag Other) => Other.GetType().IsAssignableFrom(GetType());
        public bool Is(Type Other) => Other.IsAssignableFrom(GetType());

        public static Type MoveAbility => typeof(MoveAbilityTag);

        public Sprite Icon;
        public string Name;
        public string Description;

        public static Type NoAction => typeof(NoActionTag);
        public static Type StrengthAbility => typeof(StrengthAbilityTag); 
        public static Type DexterityAbility => typeof(DexterityAbilityTag); 
        public static Type IntelligenceAbility => typeof(IntelligenceAbilityTag); 
    }

    public abstract class StrengthAbilityTag : TypeTag { }
    public abstract class DexterityAbilityTag : TypeTag { }
    public abstract class IntelligenceAbilityTag : TypeTag { }
}
