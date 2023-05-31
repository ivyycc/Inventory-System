using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "Shop Menu", menuName = "Scriptable Objects/New Shop Item")]
public class ShopItem : ScriptableObject
{
    [Header("Item")]
    public string itemName;
    public string description;
    public Sprite itemIcon;
    public float itemPrice;




    //public bool isStackable = true;

    /*
    public abstract ItemClass GetItem();

    public abstract Tools GetTools();

    public abstract Health GetHealth();

    public abstract Food GetFood();*/



}
