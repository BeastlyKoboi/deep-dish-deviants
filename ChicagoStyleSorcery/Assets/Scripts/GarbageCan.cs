using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCan : Station
{
    // Start is called before the first frame update
    void Start()
    {
        normalColor = Color.white;
        triggerColor = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInteractable)
        {
            GetComponent<SpriteRenderer>().color = normalColor;
        }
    }
  
    public override void onInteract()
    {
        if (player.playerInventory[0] != null)
        {
            player.playerInventory[0] = null;
        }
    }
}
