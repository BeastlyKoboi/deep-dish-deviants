using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (inventory[0] != null)
        {
            GetComponent<SpriteRenderer>().color = Color.black;
        }
       
        if (!isInteractable)
        {
            //GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
    
    public override void onInteract()
    {
        if (inventory[0] != null && player.playerInventory[0] == null)
        {
            player.playerInventory[0] = inventory[0];
            inventory[0] = null;
        }
        else if (inventory[0] == null && player.playerInventory[0] != null)
        {
            inventory[0] = player.playerInventory[0];
            player.playerInventory[0] = null;
        }

    }
 
}
