using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStructure;
using System;

namespace GameplayAbilitySystem
{
    [CreateAssetMenu(menuName = "Resources/ConversionTable")]
    public class LDConversionTable : ScriptableObject
    {
        public static LDConversionTable Load() => Resources.Load<LDConversionTable>("LDConversionTable");

        [Header("DON'T CHANGE TO ORDER!")]
        public List<Attribute> Attributes;
        public List<TypeTag> GameplayTags;

        public LDAttribute AttributeToLD(Type attribute, int value)
        {
            int ID = -1;
            for (int i = 0; i < Attributes.Count; i++)
            {
                if (Attributes[i].GetType() == attribute)
                {
                    ID = i; 
                }
            }

            return new LDAttribute { type = (byte)ID, value = (ushort)value };
        }

        public Attribute LDToAttribute(int LDAttribute)
        {
            return Attributes[LDAttribute];
        }

        public LDInputFrame GameplayTagToLD(Type tag, ushort cell)
        {
            int ID = -1;
            for (int i = 0; i < GameplayTags.Count; i++)
            {
                if (GameplayTags[i].GetType() == tag)
                {
                    ID = i; 
                }
            }

            return new LDInputFrame { action = (byte)ID, cell = cell };
        }

        public TypeTag LDToGameplayTag(byte LDAction)
        {
            return GameplayTags[LDAction];
        }
    }
}

