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
        currentCustomer = null;
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
        if (player.playerInventory[0] != null && player.playerInventory[0].id == FoodId.plate && currentCustomer != null)
        {
            inventory = (Plate) player.playerInventory[0];
            player.playerInventory[0] = null;
            currentCustomer.ReviewOrder(inventory);
            currentCustomer = null;
        }
    }

    //This could be importatnt later if we want the pizzas to stay on the coutner for a while then disapear
    public void ClearCounter()
    {
        inventory = null;
    }
}
