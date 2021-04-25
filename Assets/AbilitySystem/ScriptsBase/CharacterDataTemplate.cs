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
        public CharacterTemplate BlobManA;
        public CharacterTemplate BlobManB;
        public CharacterTemplate CultistA;
        public CharacterTemplate CultistB;
        public CharacterTemplate CultistC;

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
                case MonsterRole.BlobManA: return BlobManA.StartingAbilities;
                case MonsterRole.BlobManB: return BlobManB.StartingAbilities;
                case MonsterRole.CultistA: return CultistA.StartingAbilities;
                case MonsterRole.CultistB: return CultistB.StartingAbilities;
                case MonsterRole.CultistC: return CultistC.StartingAbilities;
            }

            Debug.Assert(false, "Bad role");
            return new List<GameplayAbility>();
        }

        public List<AttributeEntry> GetMonsterStartingAttributes(MonsterRole Role)
        {
            switch (Role)
            {
                case MonsterRole.BlobManA: return BlobManA.StartingAttributeSet;
                case MonsterRole.BlobManB: return BlobManB.StartingAttributeSet;
                case MonsterRole.CultistA: return CultistA.StartingAttributeSet;
                case MonsterRole.CultistB: return CultistB.StartingAttributeSet;
                case MonsterRole.CultistC: return CultistC.StartingAttributeSet;
            }

            Debug.Assert(false, "Bad role");
            return new List<AttributeEntry>();
        }

        public List<GameplayEffect> GetMonsterStartingEffects(MonsterRole Role)
        {
            switch (Role)
            {
                case MonsterRole.BlobManA: return BlobManA.StartingEffects;
                case MonsterRole.BlobManB: return BlobManB.StartingEffects;
                case MonsterRole.CultistA: return CultistA.StartingEffects;
                case MonsterRole.CultistB: return CultistB.StartingEffects;
                case MonsterRole.CultistC: return CultistC.StartingEffects;
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