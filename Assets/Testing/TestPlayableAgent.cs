using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayAbilitySystem;
using GameStructure;

namespace Testing
{
    public class TestPlayableAgent : MonoBehaviour
    {
        public PlayableAgent Barbarian;

        private void Start()
        {
            LDBlock block = new LDBlock
            {
                characters = new LDCharacter[]
                {
                    new LDCharacter
                    {
                        name = "Benny the Barbarian",
                        color = 1,
                        role = 0,
                        attributes = new LDAttribute[]
                        {
                            new LDAttribute{ type = 0, value = 80 },
                            new LDAttribute{ type = 1, value = 100 },
                            new LDAttribute{ type = 2, value = 10 },
                            new LDAttribute{ type = 3, value = 50 },
                        },
                    },
                },
            };

            Barbarian.Initialize(block);

            LDCharacter BarbChar = Barbarian.ToLDCharacter();
        }
    }
}
