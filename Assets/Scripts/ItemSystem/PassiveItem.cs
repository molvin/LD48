using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Passive Item", menuName = "Passive Item")]
public class PassiveItem : Item
{
    [Header("Variables")]
    public int maxHealthChange = 0;
    public int manaChange = 0;
    [Space(20)]
    public int healthRegenChange = 0;
    public int manaRegenChange = 0;
    [Space(20)]
    public int strengthChange = 0;
    public int dexterityChange = 0;
    public int intelligenceChange = 0;
    [Space(20)]
    public int movementSpeedChange = 0;
    [Space(20)]
    public bool debuffImmunity = false;
    public int debuffDurationChange = 0;
    [Space(20)]
    public int meleeDamageChange = 0;
    public int poisonDamageChange = 0;
    public int magicDamageChange = 0;
    [Space(20)]
    public int incomingDamageChange = 0;
    public int incomingRangedDamageChange = 0;
    public int incomingFireDamageChange = 0;
    [Space(20)]
    public int addedFireDamage = 0;
    public int addedPhysicalDamage = 0;
    public int addedLigthningDamage = 0;
    [Space(20)]
    public int abilityRangeChange = 0;
    [Space(20)]
    public int itemSlotsChange = 0;
    [Space(20)]
    public bool isCool = false;
    public bool isBland = false;
    public bool isBanana = false;
    [Space(20)]
    public bool silenceWhenDamaged = false;
    public bool thirdAbilityActivatedTwice = false;
    public bool empowerOnMagicHit = false;
    public bool rerollConsumables = false;
    public bool blindOnMeleeDamage = false;
    public bool addedCleaveAttack = false;
    public bool itemEffectTripled = false;
    public bool onlyActEveryOtherRound = false;
    public bool immuneToBlind = false;
    public bool depowerEveryThirdRound = false;

    public void OnPickup()
    {
        //Inventory.instance.player.maxHealth += maxHealthIncrease;
    }

    public void OnPutdown()
    {
        //Inventory.instance.player.maxHealth -= maxHealthIncrease;
    }
}