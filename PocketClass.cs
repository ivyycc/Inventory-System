using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PocketClass
{
    [SerializeField] private ItemClass item;
    [SerializeField] private int quantity;

    public PocketClass(PocketClass pocket)
    {
        item = pocket.item;
        quantity = pocket.quantity;
    }

    public PocketClass()
    {
        item = null;
        quantity = 0;

    }

    public PocketClass(ItemClass _item, int _quantity)
    {
        item = _item;
        quantity = _quantity;

    }

    public void Clear()
    {
        this.item = null;
        this.quantity = 0;
    }
    public ItemClass getItem() { return item; }
    public int getQuantity() { return quantity; }

    public void addQuantity(int _quant) { quantity += _quant; }
    public void MinusQuantity(int _quant) { quantity -= _quant; }

    public void addItem(ItemClass item, int quant) { this.item = item; this.quantity = quant; }


    public bool SpaceLeftInStack(int amountToAdd)
    {
        if(quantity +amountToAdd <= item.maxStackSize)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool SpaceLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = item.maxStackSize - quantity;
        return SpaceLeftInStack(amountToAdd);
    }


}
