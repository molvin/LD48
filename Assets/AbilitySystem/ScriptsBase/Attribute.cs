using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStructure;

namespace GameplayAbilitySystem
{
    public abstract class Attribute : ScriptableObject
    {
        public bool Is(Attribute Other) => Other.GetType().IsAssignableFrom(GetType());
        public bool Is(Type Other) => Other.IsAssignableFrom(GetType());

        public static Type Health => typeof(HealthAttribute);
        public static Type MaxHealth => typeof(MaxHealthAttribute);
        public static Type Mana => typeof(ManaAttribute);
        public static Type MaxMana => typeof(MaxManaAttribute);
    }
}
