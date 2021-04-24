using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="New NonConsumable", menuName = "Nonconsumable item")]
public class NonConsumable : Item
{
    public int heal = 0;
    public int damage = 0;

    public int cooldown = 0;

    public override void Use()
    {
        Debug.Log(itemName);


    }
}