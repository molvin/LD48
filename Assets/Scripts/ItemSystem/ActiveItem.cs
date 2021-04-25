using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Active Item", menuName = "Active item")]
public class ActiveItem : Item
{
    [Header("Variables")]
    public bool consumable = false;
    [Space(20)]
    public int heal = 0;
    public int outgoingDamage = 0;
    public int incomingDamage = 0;
    public int time = 0;
    [Space(20)]
    public int cooldown = 0;
    public int areaOfEffect = 0;
    public int range = 0;
    public int stunDuration = 0;
    public int incomingDamageChange = 0;
    [Space(20)]
    public bool isProjectile = false;
    public bool isLobbed = false;
    [Space(20)]
    public bool oneShotsDemons = false;
    public bool resetCooldowns = false;
    public bool cleansesDebuffs = false;
    public bool splitOutgoingDamage = false;
    public bool empowers = false;
    public bool delayedEfffect = false;
    public int delayedEffectTime = 0;

    public override void Use()
    {
        if(consumable)
            Inventory.instance.Remove(this);
    }
}