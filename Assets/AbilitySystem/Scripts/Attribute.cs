using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayAbilitySystem
{
    public abstract class Attribute : ScriptableObject
    {
        public bool Is(Attribute Other) => Other.GetType().IsAssignableFrom(GetType());
        public bool Is(Type Other) => Other.IsAssignableFrom(GetType());

        // EXAMPLE
        // public static Type Health => typeof(HealthAttribute);
    }
}
