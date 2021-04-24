using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayAbilitySystem
{
    [CreateAssetMenu(menuName = "Test/GameplayAbility/TestAbility")]
    public class TestGameplayAbility : GameplayAbility
    {
        public override void Activate(AbilitySystem Owner)
        {
            Debug.Assert(CanActivate(Owner));
            Commit(Owner);

            Debug.Log("Activating Test Ability: " + name);
        }
    }
}
