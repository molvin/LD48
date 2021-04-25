using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayAbilitySystem;
using GameStructure;

namespace Testing
{
    public class TestPlayableAgent : MonoBehaviour
    {
        public Ticker Ticker;
        public CharacterRole Controling;

        private void Start()
        {
            LDBlock block = new LDBlock
            {
                characters = new LDCharacter[]
                {
                    // Assassin
                    new LDCharacter
                    {
                        name = "Lola the Assassin",
                        color = 2,
                        role = 1,
                        attributes = new LDAttribute[]
                        {
                            new LDAttribute{ type = 0, value = 80 },
                            new LDAttribute{ type = 1, value = 100 },
                            new LDAttribute{ type = 2, value = 10 },
                            new LDAttribute{ type = 3, value = 50 },
                            new LDAttribute{ type = 4, value = 4 },
                        },
                        timeLine = new LDInputFrame[]
                        {
                            new LDInputFrame { action = 0, cell = 4, },
                            new LDInputFrame { action = 0, cell = 14, },
                            new LDInputFrame { action = 0, cell = 17, },
                        },
                    },
                    // Barbarian 
                    new LDCharacter
                    {
                        name = "Beny the Barbarian",
                        color = 3,
                        role = 0,
                        attributes = new LDAttribute[]
                        {
                            new LDAttribute{ type = 0, value = 80 },
                            new LDAttribute{ type = 1, value = 100 },
                            new LDAttribute{ type = 2, value = 10 },
                            new LDAttribute{ type = 3, value = 50 },
                            new LDAttribute{ type = 4, value = 2 },
                        },
                        timeLine = new LDInputFrame[]
                        {
                            new LDInputFrame { action = 0, cell = 16, },
                            new LDInputFrame { action = 0, cell = 30, },
                            new LDInputFrame { action = 0, cell = 32, },
                        },
                    },
                    // Barbarian 
                    new LDCharacter
                    {
                        name = "Ninni the Necromancer",
                        color = 1,
                        role = 2,
                        attributes = new LDAttribute[]
                        {
                            new LDAttribute{ type = 0, value = 80 },
                            new LDAttribute{ type = 1, value = 100 },
                            new LDAttribute{ type = 2, value = 10 },
                            new LDAttribute{ type = 3, value = 50 },
                            new LDAttribute{ type = 4, value = 1 },
                        },
                        timeLine = new LDInputFrame[]
                        {
                            new LDInputFrame { action = 0, cell = 12, },
                            new LDInputFrame { action = 0, cell = 14, },
                            new LDInputFrame { action = 0, cell = 13, },
                        },
                    },
                },
            };

            Ticker.currentBlock = block;
            Ticker.Initialize();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Ticker.Tick();
            }
        }
    }
}
