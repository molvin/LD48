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
    }
}
