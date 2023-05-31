using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class Inventory_Manager : MonoBehaviour
{
    [SerializeField] private GameObject cursor;

    [SerializeField] private GameObject pocket;   //Serialize will make it available in the inspector but private will make it only accessible in this script
    [SerializeField] private ItemClass itemToAdd; //Select item to add with mouse and click to add to backpack...?
    [SerializeField] private ItemClass itemToRemove; //select item with mouse and press X button
    [SerializeField] private GameObject shopHolder;



    [SerializeField] private PocketClass[] startItem;
    [SerializeField] private PocketClass[] chestStartItem;

    public PocketClass[] items;
    private PocketClass[] shopItems;

    public GameObject[] arr_pocket;
    public GameObject[] arr_shop;

    private PocketClass movingPocket;
    private PocketClass tempPocket;
    private PocketClass ogPocket;
  

    bool isMovingItem;

    [SerializeField] private GameObject hotBarSelector;
    [SerializeField] private int selectedSlotIndex = 0;
    public ItemClass selectedItem;


    public ShopTemplate[] shopPanelItem;
    public ShopItem item_shop;



    public void Start()
    {
        arr_pocket = new GameObject[pocket.transform.childCount]; //using an array
        arr_shop = new GameObject[shopHolder.transform.childCount];

       items = new PocketClass[arr_pocket.Length];
       


       for (int i = 0; i<arr_shop.Length; i++)
       {
            arr_shop[i] = shopHolder.transform.GetChild(i).gameObject;
       }


       for (int i = 0; i < items.Length; i++)
        {
            items[i] = new PocketClass();

            /*if(i>9)
            {
                items[i] = chestStartItem[i];
            }
            */
        }


        for (int i = 0; i < startItem.Length; i++)
        {
            
            items[i] = startItem[i];

        }


        for (int i = 0; i < pocket.transform.childCount; i++)
        {
            arr_pocket[i] = pocket.transform.GetChild(i).gameObject;
        }



        RefreshUI();
        // RefreshChestUI();

        Add(itemToAdd, 1);//adds item from item class
                          // AddToChest(itemToAddChest, 1);

        Remove(itemToRemove);
        // RemoveFromChest(itemToRemoveChest);

    }


    private void Update()
    {
        Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 ChestPos = new Vector2(3, 0);
        Vector2 BackpackPos = new Vector2(-3, 0);


        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(screenPos);



        cursor.SetActive(isMovingItem);
        cursor.transform.position = mouseWorldPos;


        if (isMovingItem)
        {
            cursor.GetComponent<Image>().sprite = movingPocket.getItem().itemIcon;


        }


        if (Input.GetMouseButtonDown(0))//we left clicked
        {

            if (isMovingItem)
            {
                endItemMove();
             
            }
            else if (!isMovingItem)
            {
                BeginItemMove();

            }

        }

        if (Input.GetMouseButtonDown(1))//right clicked
        {
            if (isMovingItem)
            {
                endItemMove_single();
   
            }
            else if (!isMovingItem)
            {
                BeginItemMove_half();
 
             
            }


        }

        if(Input.GetAxis("Mouse ScrollWheel") > 0)//scrolling the mouse up 
        {
            selectedSlotIndex = Mathf.Clamp(selectedSlotIndex+1, 0, arr_shop.Length-1);
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0)//scrolling the mouse down
        {
            selectedSlotIndex = Mathf.Clamp(selectedSlotIndex - 1, 0, arr_shop.Length - 1);
        }
        hotBarSelector.transform.position = arr_shop[selectedSlotIndex].transform.position;
        selectedItem = items[(selectedSlotIndex + (arr_shop.Length * 6)) + 1].getItem();

        Debug.Log(selectedItem.itemName);
        Debug.Log(shopPanelItem[4].titleText.text);


    }

    #region Inventory Utils
    public void RefreshUI()//determines if item is there or not, looks through each game object
    {
        for (int i = 0; i < arr_pocket.Length; i++)
        {
            try
            {
                arr_pocket[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                arr_pocket[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].getItem().itemIcon;

                if (items[i].getItem().isStackable)
                {
                    arr_pocket[i].transform.GetChild(1).GetComponent<TMP_Text>().text = items[i].getQuantity() + "";

                }
                else
                {
                    arr_pocket[i].transform.GetChild(1).GetComponent<TMP_Text>().text = "";

                }

            }
            catch
            {
                arr_pocket[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                arr_pocket[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                arr_pocket[i].transform.GetChild(1).GetComponent<TMP_Text>().text = "";



            }
        }

        RefreshShopHotbar();
    }

    public void RefreshShopHotbar()
    {
        for (int i = 0; i < arr_shop.Length; i++)
        {
            try
            {
                arr_shop[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                arr_shop[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i + (arr_shop.Length*6)+1].getItem().itemIcon;

                if (items[i + (arr_shop.Length * 6)+1].getItem().isStackable)
                {
                    arr_shop[i].transform.GetChild(1).GetComponent<TMP_Text>().text = items[i + (arr_shop.Length * 6)+1].getQuantity() + "";

                }
                else
                {
                    arr_shop[i].transform.GetChild(1).GetComponent<TMP_Text>().text = "";

                }

            }
            catch
            {
                arr_shop[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                arr_shop[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                arr_shop[i].transform.GetChild(1).GetComponent<TMP_Text>().text = "";



            }
        }
    }

   

    public bool Add(ItemClass item, int quantity)//adding item to backpack function, REMEMBER TO DO ADDITIONAL FUNCTION TO CHECK IF THE ITEM IS A STACKABLE ITEM OR NOT 
    {
        //check entire inventory to check if inventory has item 

        PocketClass pocket = hasItem(item);
        if (pocket != null && pocket.getItem().isStackable)
        {
            pocket.addQuantity(quantity);
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].getItem() == null)
                {
                    items[i].addItem(item, quantity);

                    break;
                }
            }

        }

        RefreshUI();
        return true;

    }


    public bool Remove(ItemClass item)//removes item
    {

        PocketClass temp_pocket = hasItem(item);

        if (temp_pocket != null)
        {
            if (temp_pocket.getQuantity() > 1)
            {
                temp_pocket.MinusQuantity(1);
            }
            else
            {
                int slotToRemoveIndex = 0;
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].getItem() == item)
                    {
                        slotToRemoveIndex = i;
                        break;
                    }
                }

                items[slotToRemoveIndex].Clear();


            }

        }
        else
        {
            return false;
        }


        RefreshUI();
        return true;
    }


    public PocketClass hasItem(ItemClass item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].getItem() == item)
            {
                return items[i];
            }
        }

        return null;
    }
    
    #endregion Inventory Utils

    #region MovingBackpack stuff

    private bool BeginItemMove()
    {
        ogPocket = getClosestPocket();
        if (ogPocket == null || ogPocket.getItem() == null)
        {
            return false;
        }

        movingPocket = new PocketClass(ogPocket);
        ogPocket.Clear();
        RefreshUI();
        isMovingItem = true;
        return true;
    }
    private bool BeginItemMove_half()
    {
        ogPocket = getClosestPocket();
        if (ogPocket == null || ogPocket.getItem() == null)
        {
            return false;
        }

        movingPocket = new PocketClass(ogPocket.getItem(), Mathf.CeilToInt(ogPocket.getQuantity() / 2f));
        ogPocket.MinusQuantity(Mathf.CeilToInt(ogPocket.getQuantity() / 2f));
        if (ogPocket.getQuantity() == 0)
        {
            ogPocket.Clear();
        }
        RefreshUI();
        isMovingItem = true;

        return true;

    }


    private bool endItemMove()
    {

        ogPocket = getClosestPocket();

        if (ogPocket == null)
        {
            Add(movingPocket.getItem(), movingPocket.getQuantity());
            movingPocket.Clear();
        }

        else
        {
            if (ogPocket.getItem() != null)
            {
                if (ogPocket.getItem() == movingPocket.getItem())
                {
                    //items should stack
                    if (ogPocket.getItem().isStackable)
                    {
                        ogPocket.addQuantity(movingPocket.getQuantity());
                        movingPocket.Clear();
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    //swop items
                    tempPocket = new PocketClass(ogPocket);//a = b
                    ogPocket.addItem(movingPocket.getItem(), movingPocket.getQuantity());//now we add the item // b = c
                    movingPocket.addItem(tempPocket.getItem(), tempPocket.getQuantity());//a = c
                    RefreshUI();
                    return true;
                }

            }
            else//place item as usual
            {
                ogPocket.addItem(movingPocket.getItem(), movingPocket.getQuantity());
                movingPocket.Clear();
            }

        }


        isMovingItem = false;
        RefreshUI();
        return true;


    }

    private bool endItemMove_single()
    {
        ogPocket = getClosestPocket();
        if (ogPocket == null)
        {
            return false;
        }
        if ((ogPocket.getItem() != null) && (ogPocket.getItem() != movingPocket.getItem()))
        {
            return false;
        }

        movingPocket.MinusQuantity(1);
        if (ogPocket.getItem() != null && ogPocket.getItem() == movingPocket.getItem() && movingPocket.getItem().isStackable == false)
        {
            ogPocket.addQuantity(1);
        }
        else
        {
            ogPocket.addItem(movingPocket.getItem(), 1);
        }


        if (movingPocket.getQuantity() < 1)
        {
            isMovingItem = false;
            movingPocket.Clear();
        }
        else
        {
            isMovingItem = true;
        }

        RefreshUI();
        return true;


    }


    private PocketClass getClosestPocket()
    {
        //Debug.Log(Input.mousePosition);

        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        // Debug.Log("Mouse Pos:  " + worldPosition);


        for (int j = 0; j < arr_pocket.Length; j++)
        {
            //Debug.Log("Object pos:  " + arr_pocket[i].transform.position);

            if (Vector2.Distance(arr_pocket[j].transform.position, worldPosition) <= 0.5)
            {
                return items[j];
            }

        }


        return null;
    }

    #endregion MovingBackpack stuff

    public void AddToCart()
    {
        Debug.Log("testing adding");
        for(int i  = 0; i<shopPanelItem.Length; i++)
        {
            if (shopPanelItem[i].titleText.text == selectedItem.itemName)
            {
               
                Add(selectedItem, 1);
                AddShopItem(items[i].getItem(), item_shop, 1);
                
                //Debug.Log("item bought");
            }
        }
        
        
    }

    public void RemoveFromCart()
    {
        Debug.Log("testing adding");

        for (int i = 0; i < shopPanelItem.Length; i++)
        {
            if (shopPanelItem[i].titleText.text == selectedItem.itemName)
            {
                Remove(selectedItem);
                //Debug.Log("item bought");
            }
        }


    }



    public bool AddShopItem(ItemClass item, ShopItem item_s, int quantity)//adding item to backpack function, REMEMBER TO DO ADDITIONAL FUNCTION TO CHECK IF THE ITEM IS A STACKABLE ITEM OR NOT 
    {
        //check entire inventory to check if inventory has item 

        PocketClass pocket = hasItem(item);

        if (item_s.itemName == item.itemName)
        {
            if (pocket != null && pocket.getItem().isStackable)
            {
                pocket.addQuantity(quantity);
            }
            else
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].getItem() == null)
                    {
                        items[i].addItem(item, quantity);

                        break;
                    }
                }

            }
        }

        RefreshUI();
        return true;

    }

}//end monobehaviour



/*public bool AddToChest(ItemClass item, int quantity)//adding item to backpack function, REMEMBER TO DO ADDITIONAL FUNCTION TO CHECK IF THE ITEM IS A STACKABLE ITEM OR NOT 
{
    //check entire inventory to check if inventory has item 

    PocketClass slot = ChesthasItem(item);
    if (slot != null && slot.getItem().isStackable)
    {
        slot.addQuantity(1);
    }
    else
*/


/*
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (chestItems[i].getItem() == null)
                {
                    chestItems[i].addItem(item, quantity);

                    break;
                }
            }

        }

        RefreshChestUI();
        return true;

    }*/

/*public bool RemoveFromChest(ItemClass item)//removes item
{

    PocketClass temp_slot = ChesthasItem(item);

    if (temp_slot != null)
    {
        if (temp_slot.getQuantity() > 1)
        {
            temp_slot.MinusQuantity(1);
        }
        else
        {
            int slotToRemoveIndex = 0;
            for (int i = 0; i < chestItems.Length; i++)
            {
                if (chestItems[i].getItem() == item)
                {
                    slotToRemoveIndex = i;
                    break;
                }
            }

            chestItems[slotToRemoveIndex].Clear();



        }

    }
    else
    {
        return false;
    }


    RefreshChestUI();
    return true;
}*/
/*
public PocketClass ChesthasItem(ItemClass item)
{
    for (int i = 0; i < chestItems.Length; i++)
    {
        if (chestItems[i].getItem() == item)
        {
            return chestItems[i];
            Debug.Log("chest has item");
        }
    }

    return null;
}*/

/*
#region MovingChest stuff
private bool BeginItemMoveChest()
{
    ogSlot = getClosestSlot();
    if (ogSlot == null || ogSlot.getItem() == null)//|| ogPocke == null || ogPocket.getItem() == null ||
    {
        return false;
    }

    movingSlot = new PocketClass(ogSlot);
    ogSlot.Clear();
    RefreshChestUI();
    isMovingItem = true;
    return true;
}
private bool BeginItemMoveChest_half()
{
    ogSlot = getClosestSlot();
    if (ogSlot == null || ogSlot.getItem() == null)
    {
        return false;
    }

    movingSlot = new PocketClass(ogSlot.getItem(), Mathf.CeilToInt(ogSlot.getQuantity() / 2f));
    ogSlot.MinusQuantity(Mathf.CeilToInt(ogSlot.getQuantity() / 2f));
    if (ogSlot.getQuantity() == 0)
    {
        ogSlot.Clear();
    }
    RefreshChestUI();
    isMovingItem = true;

    return true;

}


private bool endItemMoveChest()
{
    ogSlot = getClosestSlot();

    if (ogSlot == null)
    {
        AddToChest(movingSlot.getItem(), movingSlot.getQuantity());
        movingSlot.Clear();
    }
    else
    {
        if (ogSlot.getItem() != null)
        {
            if (ogSlot.getItem() == movingSlot.getItem())
            {
                //items should stack
                if (ogSlot.getItem().isStackable)
                {
                    ogSlot.addQuantity(movingSlot.getQuantity());
                    movingSlot.Clear();
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //swop items
                tempSlot = new PocketClass(ogSlot);//a = b
                ogSlot.addItem(movingSlot.getItem(), movingSlot.getQuantity());//now we add the item // b = c
                movingSlot.addItem(tempSlot.getItem(), tempSlot.getQuantity());//a = c
                RefreshChestUI();
                return true;
            }

        }
        else//place item as usual
        {
            ogSlot.addItem(movingSlot.getItem(), movingSlot.getQuantity());
            movingSlot.Clear();
        }

    }
    isMovingItem = false;
    RefreshChestUI();
    return true;


}

private bool endItemMoveChest_single()
{
    ogSlot = getClosestSlot();
    if (ogSlot == null)
    {
        return false;
    }
    if (ogSlot.getItem() != null && ogSlot.getItem() != movingSlot.getItem())
    {
        return false;
    }

    movingSlot.MinusQuantity(1);
    if (ogSlot.getItem() != null && ogSlot.getItem() == movingSlot.getItem() && movingSlot.getItem().isStackable == false)
    {
        ogSlot.addQuantity(1);
    }
    else
    {
        ogSlot.addItem(movingSlot.getItem(), 1);
    }


    if (movingSlot.getQuantity() < 1)
    {
        isMovingItem = false;

        movingSlot.Clear();
    }
    else
    {
        isMovingItem = true;

    }


    RefreshChestUI();
    return true;

}


private PocketClass getClosestSlot()
{
    //Debug.Log(Input.mousePosition);

    Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

    // Debug.Log("Mouse Pos:  " + worldPosition);


    for (int j = 0; j < arr_chestSlot.Length; j++)
    {
        //Debug.Log("Object pos:  " + arr_pocket[i].transform.position);

        if (Vector2.Distance(arr_chestSlot[j].transform.position, worldPosition) <= 0.5)
        {
            return chestItems[j];
        }

    }


    return null;
}
#endregion MovingChest stuff

private int MoveAcross()
{
    Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    Vector2 ChestPos = new Vector2(3, 0);
    Vector2 BackpackPos = new Vector2(-3, 0);
    Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(screenPos);
    cursor.transform.position = mouseWorldPos;



    ogPocket = getClosestPocket();
    ogSlot = getClosestSlot();

    if((Vector2.Distance(cursor.transform.position, ChestPos) > 5) && (cursor.GetComponent<Image>().sprite != null))
    {
        if(ogPocket == null)
        {
            Debug.Log("Can place in backpack");
            RefreshChestUI();
            return 1;
        }


    }

    if ((Vector2.Distance(cursor.transform.position, BackpackPos) > 5) && (cursor.GetComponent<Image>().sprite != null))
    {

        if (ogSlot == null)
        {

            Debug.Log("Can place in chest");
            RefreshChestUI();
            return 0;
        }

    }


    return 2;

}
}
*/
/*
public void RefreshChestUI()
{
   for (int i = 0; i < arr_chestSlot.Length; i++)
   {
       try
       {
           arr_chestSlot[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
           arr_chestSlot[i].transform.GetChild(0).GetComponent<Image>().sprite = chestItems[i].getItem().itemIcon;

           if (chestItems[i].getItem().isStackable)
           {
               arr_chestSlot[i].transform.GetChild(1).GetComponent<TMP_Text>().text = chestItems[i].getQuantity() + "";

           }
           else
           {
               arr_chestSlot[i].transform.GetChild(1).GetComponent<TMP_Text>().text = "";

           }

       }
       catch
       {
           arr_chestSlot[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
           arr_chestSlot[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
           arr_chestSlot[i].transform.GetChild(1).GetComponent<TMP_Text>().text = "";


       }
   }
}*/