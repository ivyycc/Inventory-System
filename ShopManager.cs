using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public float coins;
    public TMP_Text coinUI;
    public ShopItem[] shopItem;
    public ShopTemplate[] shopPanels;
    public GameObject[] shopPanelsGo;
    public Button[] myBuyBtn;
    public Button[] sellBtn;

   // public PocketClass[] itemTobuy;

    public ShopItem[] item;

    public Inventory_Manager Inventory;


    void Start()
    {
        item = new ShopItem[Inventory.arr_pocket.Length];

        //Debug.Log(Inventory.items[1].getQuantity());

        for (int i =0; i<shopItem.Length; i++)
        {
            shopPanelsGo[i].SetActive(true);
            
        }

        coinUI.text = "Coins: " + coins.ToString();

        LoadPanels();
        CheckIfCanBuy();
        CheckIfCanSell();
    }
    public void AddCoins()
    {
        coins++;
        coinUI.text = "Coins: " + coins.ToString();
        CheckIfCanBuy();
       CheckIfCanSell();

    }

    public void CheckIfCanBuy()
    {
        for(int i =0; i<shopItem.Length; i++)
        {
            if(coins >= shopItem[i].itemPrice) // i have enough money and if space in inventory
            {
                myBuyBtn[i].interactable = true;
            }
            else
            {
                myBuyBtn[i].interactable = false;
            }
        }
    }

    public void BuyItem(int btnNo)
    {
        if(coins >= shopItem[btnNo].itemPrice)
        {
            
            coins = coins - shopItem[btnNo].itemPrice;
            coinUI.text = "Coins: " + coins.ToString();
            //Inventory.AddToCart();
            CheckIfCanBuy();
            //item[btnNo] = shopItem[btnNo];



        }
    }

    public void LoadPanels()
    {
        for(int i =0; i<shopItem.Length; i++)
        {
            shopPanels[i].titleText.text = shopItem[i].itemName;
            shopPanels[i].descriptionText.text = shopItem[i].description;
            shopPanels[i].itemImage.sprite = shopItem[i].itemIcon;
            shopPanels[i].costText.text = shopItem[i].itemPrice.ToString();
        }
    }



    public void SellItems(int btnNo)
    {
         if (coins <= shopItem[btnNo].itemPrice)
        {
            

            coins = coins + shopItem[btnNo].itemPrice;
            coinUI.text = "Coins: " + coins.ToString();
            CheckIfCanSell();

            //Inventory.RemoveFromCart();

           // Debug.Log("sell item");


        }

    }
 


    public void CheckIfCanSell()
    {
        for (int i = 0; i < shopItem.Length; i++)
        {
            if (shopItem[i].itemName == Inventory.selectedItem.itemName) // if item in cart 
            {
                sellBtn[i].interactable = true;
            }
            else
            {
                sellBtn[i].interactable= false;
            }
        }
    }


}
