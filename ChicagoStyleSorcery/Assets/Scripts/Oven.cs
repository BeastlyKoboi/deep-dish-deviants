using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : Station
{

    // this is a non magic station that takes 10 seconds to cook a pizza
    // inventory is a plate variable since it can only store plates
    private Plate inventory;

    // amount of time it takes to cook the food
    [SerializeField]
    float timer;


    // Start is called before the first frame update
    void Start()
    {
        timer = 10;
        triggerColor = Color.green;
        normalColor = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory == null)
        {
            timer = 10;
        }
        else if(inventory != null)
        {
            timer -= Time.deltaTime;
        }
        // will cook pizza after 9 seconds
        if(timer < 1 && inventory != null)
        {
            for(int i= 0; i < inventory.coreFoodlist.Count; i++)
            {
                if (inventory.coreFoodlist[0].foodState == CookState.raw)
                {
                    inventory.coreFoodlist[0].foodState = CookState.cooked;
                }
            }
        }
        // if timer is less than -15, pizza has been in over for to long and will be burnt
        if(timer < -15 && inventory != null)
        {
            for (int i = 0; i < inventory.coreFoodlist.Count; i++)
            {
                inventory.coreFoodlist[i].foodState = CookState.burnt;
            }
        }

    }

    public override void onInteract()
    {
        // if player holding a plate, and oven is empty, put plate in oven
        if (inventory == null && player.playerInventory[0] != null && player.playerInventory[0].id == FoodId.plate)
        {
            inventory = (Plate)player.playerInventory[0];
            player.playerInventory[0] = null;
        }
        // after 10 seconds if player has nothing and oven has something player takes what is in oven
        if (timer <= 0 && inventory != null && player.playerInventory[0] == null)
        {
            player.playerInventory[0] = inventory;
            inventory= null;
        }
       
    }
}
