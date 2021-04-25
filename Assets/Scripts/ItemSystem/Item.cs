using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    [Header("General")]
    public string itemName;
    public Sprite itemImage;
    public string itemDescription;

    public virtual void Use() {}
}