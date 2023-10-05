using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlateDespenser : Station
{
    [SerializeField]
    Plate plate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInteractable)
        {
            GetComponent<SpriteRenderer>().color = Color.black;
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
