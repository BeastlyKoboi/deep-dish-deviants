using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreIngredientStation : Station
{
    [SerializeField]
    FoodId id;

    [SerializeField]
    FoodItem despenserType;

    [SerializeField]
    CookState cookState;

    
    private Color stationColor;
    // Start is called before the first frame update
    void Start()
    {
        if(id == FoodId.dough)
        {
            stationColor = Color.cyan;
        }
        if(id == FoodId.cheese) {
            stationColor = Color.yellow;
        }
        if(id == FoodId.sauce)
        {
            stationColor = Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!isInteractable)
        {
            GetComponent<SpriteRenderer>().color = stationColor;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
    }


    public override void onInteract()
    {
        if (player.playerInventory[0] == null)
        {
            player.playerInventory[0] = despenserType;
        }
    }
}
