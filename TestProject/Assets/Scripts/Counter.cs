using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Counter : Station
{
    public FoodItem[] inventory;
    
   
    // Start is called before the first frame update
    void Start()
    {
        inventory = new FoodItem[1];
    }

    // Update is called once per frame
    void Update()
    {
       
       
        if (!isInteractable)
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        if (inventory[0] != null)
        {
            if (inventory[0].id == FoodId.dough)
            {
                GetComponent<SpriteRenderer>().color = Color.cyan;
            }
            if (inventory[0].id == FoodId.cheese)
            {
                GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            if (inventory[0].id == FoodId.sauce)
            {
                GetComponent<SpriteRenderer>().color = Color.red;
            }
            if (inventory[0].id == FoodId.plate)
            {
                GetComponent<SpriteRenderer>().color = Color.black;
            }
            //GetComponent<SpriteRenderer>().color = Color.black;
        }

    }
    
    public override void onInteract()
    {
        if (inventory[0] != null && inventory[0].id == FoodId.plate && player.playerInventory[0] != null) 
        {
            Plate tempPlate = (Plate)inventory[0];
            tempPlate.AddToPlate(player.playerInventory[0]);
            
            inventory[0] = tempPlate;
            player.playerInventory[0] = null;
            
        }
        else if (inventory[0] != null && player.playerInventory[0] == null)
        {
            player.playerInventory[0] = inventory[0];
            inventory[0] = null;
        }
        else if (inventory[0] == null && player.playerInventory[0] != null)
        {
            inventory[0] = player.playerInventory[0];
            player.playerInventory[0] = null;
        }
        else if(inventory[0] != null && player.playerInventory[0] != null)
        {
            CoreIngredient tempIngredient = (CoreIngredient) inventory[0];
            inventory[0] = player.playerInventory[0];
            player.playerInventory[0] = tempIngredient;
        }


    }
 
}
