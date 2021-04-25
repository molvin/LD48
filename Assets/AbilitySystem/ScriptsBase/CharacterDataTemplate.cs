using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayAbilitySystem
{
    [CreateAssetMenu(menuName = "Resources/CharacterDataTemplate")]
    public class CharacterDataTemplate : ScriptableObject 
    {
        public static CharacterDataTemplate Load() => Resources.Load<CharacterDataTemplate>("CharacterDataTemplate");

        [Header("Playable Characters")]
        public CharacterTemplate Barbarian;
        public CharacterTemplate Assassin;
        public CharacterTemplate Necromanacer;

        [Header("Monsters")]
        public CharacterTemplate BlobMan;
        public CharacterTemplate Cultist;

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

        public List<AttributeEntry> GetStartingAttributes(CharacterRole Role)
        {
            switch (Role)
            {
                case CharacterRole.Barbarian: return Barbarian.StartingAttributeSet;
                case CharacterRole.Assassin: return Assassin.StartingAttributeSet;
                case CharacterRole.Necromancer: return Necromanacer.StartingAttributeSet;
            }
            Debug.Assert(false, "Bad role");
            return new List<AttributeEntry>();
        }

        public List<GameplayEffect> GetStartingEffects(CharacterRole Role)
        {
            switch (Role)
            {
                case CharacterRole.Barbarian: return Barbarian.StartingEffects;
                case CharacterRole.Assassin: return Assassin.StartingEffects;
                case CharacterRole.Necromancer: return Necromanacer.StartingEffects;
            }
            Debug.Assert(false, "Bad role");
            return new List<GameplayEffect>();
        }

        public List<GameplayAbility> GetMonsterAbilities(MonsterRole Role)
        {
            switch (Role)
            {
                case MonsterRole.BlobMan: return BlobMan.StartingAbilities;
                case MonsterRole.Cultist: return Cultist.StartingAbilities;
            }

            Debug.Assert(false, "Bad role");
            return new List<GameplayAbility>();
        }

        public List<AttributeEntry> GetMonsterStartingAttributes(MonsterRole Role)
        {
            switch (Role)
            {
                case MonsterRole.BlobMan: return BlobMan.StartingAttributeSet;
                case MonsterRole.Cultist: return Cultist.StartingAttributeSet;
            }

            Debug.Assert(false, "Bad role");
            return new List<AttributeEntry>();
        }

        public List<GameplayEffect> GetMonsterStartingEffects(MonsterRole Role)
        {
            switch (Role)
            {
                case MonsterRole.BlobMan: return BlobMan.StartingEffects;
                case MonsterRole.Cultist: return Cultist.StartingEffects;
            }

            Debug.Assert(false, "Bad role");
            return new List<GameplayEffect>();
        }
    }

    [System.Serializable]
    public struct CharacterTemplate
    {
        public List<GameplayAbility> StartingAbilities;
        public List<GameplayEffect> StartingEffects;
        public List<AttributeEntry> StartingAttributeSet;
    }

    [System.Serializable]
    public struct AttributeEntry
    {
        public Attribute Attribute;
        public int Value;
    }

}