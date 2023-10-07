using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreIngredientStation : Station
{
    [SerializeField]
    FoodId id;

    [SerializeField]
    CoreIngredient despenserType;

    [SerializeField]
    private Icon icon;
    
    private Color stationColor;
    // Start is called before the first frame update
    void Start()
    {
        icon = Instantiate<Icon>(icon);
        icon.transform.position = gameObject.transform.position ;

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
        icon.SetIconType(despenserType.id);

        if (!isInteractable)
        {
            //GetComponent<SpriteRenderer>().color = stationColor;
            GetComponent<SpriteRenderer>().color = Color.gray;
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
            player.playerInventory[0] = Instantiate<CoreIngredient>(despenserType);
        }
    }
}
