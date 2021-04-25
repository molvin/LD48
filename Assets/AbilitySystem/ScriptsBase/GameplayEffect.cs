using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayAbilitySystem
{
    public enum EffectDurationType
    {
        Instant,
        Duration,
    }

    [CreateAssetMenu(menuName = "Ability System/Effect")]
    public class GameplayEffect : ScriptableObject 
    {
        public EffectDurationType EffectType;
        public int Duration;

        [Header("Initial")]
        public Attribute InitialAttribute;
        public int InitialValue;

        [Header("Tick")]
        public Attribute TickAttribute;
        public int TickValue;

        [Header("Tags")]
        public List<TypeTag> AppliedTags;
        public List<TypeTag> BlockedByTags;
        public List<TypeTag> RequiredTags;
    }
}
