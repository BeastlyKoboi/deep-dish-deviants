using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlateDespenser : Station
{
    [SerializeField]
    Plate plate;

    [SerializeField]
    Icon icon;
    // Start is called before the first frame update
    void Start()
    {
        icon = Instantiate<Icon>(icon);
        icon.transform.position = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        icon.SetIconType(FoodId.plate);
        if (!isInteractable)
        {
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
            player.playerInventory[0] = Instantiate<Plate>(plate);
        }
    }
}
