using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="New Passive", menuName = "Passive Item")]
public class Passive : Item
{
    public override void Use()
    {
        Debug.Log(itemName);
    }
}