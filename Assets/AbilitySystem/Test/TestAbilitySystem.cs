using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayAbilitySystem
{
    public class TestAbilitySystem : MonoBehaviour
    {
        AbilitySystem AbilitySystem = new AbilitySystem();

        public GameplayAbility Ability1;
        public GameplayAbility Ability2;
        public GameplayAbility Ability3;
        public GameplayAbility Ability4;

        public GameplayEffect RequiredEffect;
        public GameplayEffect ApplyRequiredEffect;
        public GameplayEffect BlockedEffect;
        public GameplayEffect ApplyBlockedEffect;

        // Start is called before the first frame update
        void Start()
        {
            // Register attributes
            Debug.Assert(AbilitySystem.GetAttributeValue(typeof(TestHealthAttribute)) == null);
            Debug.Assert(AbilitySystem.GetAttributeValue(typeof(TestManaAttribute)) == null);

            AbilitySystem.RegisterAttribute(typeof(TestHealthAttribute), 100);
            AbilitySystem.RegisterAttribute(typeof(TestManaAttribute), 50);
            AbilitySystem.RegisterAttribute(typeof(TestArmorAttribute), 20);

            Debug.Assert(AbilitySystem.GetAttributeValue(typeof(TestHealthAttribute)).Value == 100);
            Debug.Assert(AbilitySystem.GetAttributeValue(typeof(TestManaAttribute)).Value == 50);

            AbilitySystem.RegisterAttributeCalculation(typeof(TestHealthAttribute), ArmorCalculation);
            AbilitySystem.RegisterOnAttributeChanged(typeof(TestHealthAttribute), (x) => Debug.Log("New health: " + x));

            Debug.Assert(!AbilitySystem.TryActivateAbilityByTag(Ability1.AbilityTag));
            AbilitySystem.GrantAbility(Ability1);
            Debug.Assert(AbilitySystem.TryActivateAbilityByTag(Ability1.AbilityTag));

            Debug.Assert(!AbilitySystem.TryActivateAbilityByTag(Ability2.AbilityTag));
            AbilitySystem.GrantAbility(Ability2);
            Debug.Assert(AbilitySystem.TryActivateAbilityByTag(Ability2.AbilityTag));

            AbilitySystem.RevokeAbility(Ability2.AbilityTag);
            Debug.Assert(!AbilitySystem.TryActivateAbilityByTag(Ability2.AbilityTag));

            AbilitySystem.GrantAbility(Ability3);

            Debug.Assert(AbilitySystem.TryActivateAbilityByTag(Ability3.AbilityTag));
            Debug.Assert(AbilitySystem.GetAttributeValue(typeof(TestManaAttribute)).Value == 30);

            Debug.Assert(AbilitySystem.TryActivateAbilityByTag(Ability3.AbilityTag));
            Debug.Assert(AbilitySystem.GetAttributeValue(typeof(TestManaAttribute)).Value == 10);

            Debug.Assert(!AbilitySystem.TryActivateAbilityByTag(Ability3.AbilityTag));

            AbilitySystem.GrantAbility(Ability4);

            Debug.Assert(AbilitySystem.TryActivateAbilityByTag(Ability4.AbilityTag));
            Debug.Assert(AbilitySystem.GetAttributeValue(typeof(TestManaAttribute)).Value == 0);
            AbilitySystem.Tick();

            Debug.Assert(!AbilitySystem.TryActivateAbilityByTag(Ability4.AbilityTag));
            AbilitySystem.Tick();

            Debug.Assert(!AbilitySystem.TryActivateAbilityByTag(Ability4.AbilityTag));
            AbilitySystem.Tick();

            Debug.Assert(AbilitySystem.GetAttributeValue(typeof(TestManaAttribute)).Value == 10);

            Debug.Assert(AbilitySystem.TryActivateAbilityByTag(Ability4.AbilityTag));

            Debug.Assert(!AbilitySystem.TryApplyEffectToSelf(RequiredEffect));
            AbilitySystem.TryApplyEffectToSelf(ApplyRequiredEffect);
            Debug.Assert(AbilitySystem.TryApplyEffectToSelf(RequiredEffect));

            Debug.Assert(AbilitySystem.TryApplyEffectToSelf(BlockedEffect));
            AbilitySystem.TryApplyEffectToSelf(ApplyBlockedEffect);
            Debug.Assert(!AbilitySystem.TryApplyEffectToSelf(BlockedEffect));
        }

        public int ArmorCalculation(int Value)
        {
            int Armor = AbilitySystem.GetAttributeValue(typeof(TestArmorAttribute)).Value;
            return Mathf.Clamp(Value - Armor, 0, Value);
        }
    }

    public class TestArmorAttribute : Attribute { }
}
