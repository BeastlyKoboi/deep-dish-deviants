using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public FoodItem[] playerInventory;
    // Start is called before the first frame update
    void Start()
    {
        playerInventory = new FoodItem[1];
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
    }
}
