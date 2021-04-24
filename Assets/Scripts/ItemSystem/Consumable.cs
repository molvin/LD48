using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="New Consumable", menuName = "Consumable item")]
public class Consumable : Item
{
    public int heal = 0;
    public int damage = 0;


    public override void Use()
    {
        Debug.Log(itemName);
        Inventory.instance.Remove(this);
    }
}