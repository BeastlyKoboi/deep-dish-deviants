using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpStation : Station
{
    // pick up station can only hold plate objects
    public Plate inventory;

    public Customer currentCustomer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInteractable)
        {
            GetComponent<SpriteRenderer>().color = Color.gray;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }

    public override void onInteract()
    {
       if(player.playerInventory[0].id == FoodId.plate)
        {
            inventory =(Plate) player.playerInventory[0];
            player.playerInventory[0] = null;
        }
    }
}
