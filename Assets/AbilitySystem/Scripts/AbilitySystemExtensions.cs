using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayAbilitySystem
{
    public static class AbilitySystemExtensions
    {
        public static bool Is(this Type T, TypeTag Tag) => T.IsAssignableFrom(Tag.GetType());
        public static bool Is(this Type T, Attribute Attribute) => T.IsAssignableFrom(Attribute.GetType());
    }
}
