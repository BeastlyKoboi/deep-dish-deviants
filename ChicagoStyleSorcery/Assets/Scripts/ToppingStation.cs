using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppingStation : Station
{
    [SerializeField]
    FoodId id;

    [SerializeField]
    Topping despenserType;

    [SerializeField]
    private Icon icon;

    private Color stationColor;
    // Start is called before the first frame update
    void Start()
    {
       icon = Instantiate<Icon>(icon);
       icon.transform.position = gameObject.transform.position;
        normalColor = Color.white;
        triggerColor = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
       icon.SetIconType(despenserType.id);
        if (!isInteractable)
        {
            GetComponent<SpriteRenderer>().color = normalColor;
        }
    }

    public override void onInteract()
    {
        if (player.playerInventory[0] == null)
        {
            player.playerInventory[0] = Instantiate<Topping>(despenserType);
        }
    }
}
