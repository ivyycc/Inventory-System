using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemClass : ScriptableObject
{
    [Header("Item")]
    public string itemName;
    public Sprite itemIcon;
    public float itemPrice;
    public bool isStackable = true;
    public int maxStackSize = 10;

    public abstract ItemClass GetItem();

    public abstract Tools GetTools();

    public abstract Health GetHealth();

    public abstract Food GetFood();


 
}
