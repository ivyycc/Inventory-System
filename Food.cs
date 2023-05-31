using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool Class", menuName = "Item/Food")]
public class Food : ItemClass
{
    [Header("Consumables")]
    public Food_Type foodtype;
    public float health_added;

    public enum Food_Type
    {
        apples,
        water,
        energydrink,
        sandwhich,
        chocolate

    }
    public override ItemClass GetItem()
    {
        return this; //inherits from item class
    }

    public override Tools GetTools()
    {
        return null;
    }

    public override Health GetHealth()
    {
        return null;
    }

    public override Food GetFood()
    {
        return this;
    }
}
