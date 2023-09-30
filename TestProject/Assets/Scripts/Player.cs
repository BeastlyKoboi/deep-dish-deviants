using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public FoodItem[] playerInventory;
    // is true when player fires aka presses E or right click
    public bool isInteracting;
    // Start is called before the first frame update
    void Start()
    {
        playerInventory = new FoodItem[1];
        isInteracting= false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInventory[0] != null)
        {
            switch (playerInventory[0].id)
            {
                case FoodId.dough:
                    GetComponent<SpriteRenderer>().color = Color.cyan;
                    break;
                 case FoodId.cheese:
                    GetComponent<SpriteRenderer>().color = Color.yellow;
                    break;
                 case FoodId.sauce:
                    GetComponent<SpriteRenderer>().color = Color.red;
                    break;
         

            }
        }
         else
        {
           GetComponent<SpriteRenderer>().color = Color.white;

        }
    }
}
