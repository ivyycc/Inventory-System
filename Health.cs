using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool Class", menuName = "Item/Health")]
public class Health : ItemClass
{
    [Header("Health stuff")]
    public Health_Type Healthstuff;
    public float strength;

    public enum Health_Type
    {
        medicine,
        bandages,
        sheild
    }
    public override ItemClass GetItem()
    {
        return this; //inherits from item class
    }
    public override Health GetHealth()
    {
        return this;
    }


    public override Tools GetTools()
    {
        return null;
    }

   
    public override Food GetFood()
    {
        return null;
    }
}
