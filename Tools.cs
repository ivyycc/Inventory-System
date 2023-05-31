using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Tool Class", menuName = "Item/Tool" )]
public class Tools : ItemClass
{
    //it also a scriptable object because it inherits from the item class (scriptable object)

    [Header("Tools")]
    public Tool_Type ToolType;
    public float damage;
    public enum Tool_Type
    {
        sword,
        axe,
        hammer

    }
    public override ItemClass GetItem()
    {
        return this; //inherits from item class
    }

    public override Tools GetTools()
    {
        return this;
    }

    public override Health GetHealth()
    {
        return null;
    }

    public override Food GetFood()
    {
        return null;
    }





}
