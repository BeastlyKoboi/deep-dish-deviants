using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreIngredientStation : Station
{
    [SerializeField]
    FoodId id;

    [SerializeField]
    CoreIngredient despenserType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void onInteract()
    {
        if (player.playerInventory[0] == null)
        {
            player.playerInventory[0] = despenserType;
        }
    }
}
