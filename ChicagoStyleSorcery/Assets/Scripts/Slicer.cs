using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Slicer : Station
{

    // this is a non magic station that takes 5 seconds to cut the toppings
    // inventory is a topping variable since it can only store cuttable items
    private Topping inventory;

    // amount of time it takes to cook the food
    [SerializeField]
    float timer;

    [SerializeField]
    GameObject timerBackground;

    [SerializeField]
    TextMeshProUGUI timerText;


    // Start is called before the first frame update
    void Start()
    {
        timer = 5;
        triggerColor = Color.green;
        normalColor = Color.cyan;
        timerBackground.SetActive(false);
        timerText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory == null)
        {
            timer = 5;
            timerBackground.SetActive(false);
            timerText.gameObject.SetActive(false);
        }
        else if (inventory != null)
        {
            timer -= Time.deltaTime;
            timerBackground.SetActive(true);
            timerText.gameObject.SetActive(true);
            timerText.text = ((int)timer).ToString();
        }
        // will cut item after 4 seconds
        if (timer < 1 && inventory != null)
        {
            inventory.cutState = CutState.cut;
        }
      
       

    }

    public override void onInteract()
    {
        // if player holding a cuttable fooditem, and slivcer is empty, put topping in slicer
        if (inventory == null && player.playerInventory[0] != null && player.playerInventory[0].cutState != CutState.na )
        {
            inventory = (Topping)player.playerInventory[0];
            player.playerInventory[0] = null;
        }
        // after 5 seconds if player has nothing and oven has something player takes what is in oven
        if (timer <= 0 && inventory != null && player.playerInventory[0] == null)
        {
            player.playerInventory[0] = inventory;
            inventory = null;
        }

    }
}
