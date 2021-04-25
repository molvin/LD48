using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayAbilitySystem
{
    [CreateAssetMenu(menuName = "Resources/CharacterDataTemplate")]
    public class CharacterDataTemplate : ScriptableObject 
    {
        public static CharacterDataTemplate Load() => Resources.Load<CharacterDataTemplate>("CharacterDataTemplate");

        public CharacterTemplate Barbarian;
        public CharacterTemplate Assassin;
        public CharacterTemplate Necromanacer;

        public List<GameplayAbility> GetAbilities(CharacterRole Role)
        {
            switch (Role)
            {
                case CharacterRole.Barbarian: return Barbarian.StartingAbilities;
                case CharacterRole.Assassin: return Assassin.StartingAbilities;
                case CharacterRole.Necromancer: return Necromanacer.StartingAbilities;
            }
            Debug.Assert(false, "Bad role");
            return new List<GameplayAbility>();
        }
    }

    [System.Serializable]
    public struct CharacterTemplate
    {
        public List<GameplayAbility> StartingAbilities;
    }

}