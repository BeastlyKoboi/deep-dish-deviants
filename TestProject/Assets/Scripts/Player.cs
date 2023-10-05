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
        playerInventory = new FoodItem[1] { null };
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
                    if (playerInventory[0].foodState == CookState.raw)
                    {
                        GetComponent<SpriteRenderer>().color = Color.cyan;
                    }
                    else if (playerInventory[0].foodState == CookState.cooked)
                    {
                        GetComponent<SpriteRenderer>().color = Color.gray;
                    }
                    else if (playerInventory[0].foodState == CookState.burnt)
                    {
                        GetComponent<SpriteRenderer>().color = Color.magenta;
                    }
                    break;
                 case FoodId.cheese:
                    if (playerInventory[0].foodState == CookState.raw)
                    {
                        GetComponent<SpriteRenderer>().color = Color.yellow;
                    }
                    else if (playerInventory[0].foodState == CookState.cooked)
                    {
                        GetComponent<SpriteRenderer>().color = Color.gray;
                    }
                    else if (playerInventory[0].foodState == CookState.burnt)
                    {
                        GetComponent<SpriteRenderer>().color = Color.magenta;
                    }
                    break;
                 case FoodId.sauce:
                    if (playerInventory[0].foodState == CookState.raw)
                    {
                        GetComponent<SpriteRenderer>().color = Color.red;
                    }
                    else if (playerInventory[0].foodState == CookState.cooked)
                    {
                        GetComponent<SpriteRenderer>().color = Color.gray;
                    }
                    else if (playerInventory[0].foodState == CookState.burnt)
                    {
                        GetComponent<SpriteRenderer>().color = Color.magenta;
                    }
                    break;
                case FoodId.plate:
                    if (playerInventory[0].foodState == CookState.raw)
                    {
                        GetComponent<SpriteRenderer>().color = Color.black;
                    }
                    else if (playerInventory[0].foodState == CookState.cooked)
                    {
                        GetComponent<SpriteRenderer>().color = Color.gray;
                    }
                    else if (playerInventory[0].foodState == CookState.burnt)
                    {
                        GetComponent<SpriteRenderer>().color = Color.magenta;
                    }
                    break;
            }
        }
        
        else
        {
           GetComponent<SpriteRenderer>().color = Color.white;

        }
    }

    /// <summary>
    /// Fire magic cooks the ingredient the player is holding
    /// </summary>
    public void FireMagic()
    {
        //check is player is holding something
        if (playerInventory[0] != null)
        {
            //if it is not plate, then it may proceed
            if (playerInventory[0].id == FoodId.dough || playerInventory[0].id == FoodId.sauce || playerInventory[0].id == FoodId.cheese)
            {
                // if raw, made cooked
                if (playerInventory[0].foodState == CookState.raw)
                {
                    playerInventory[0].foodState = CookState.cooked;
                }
                // if cooked, made burnt
                else if (playerInventory[0].foodState == CookState.cooked)
                {
                    playerInventory[0].foodState = CookState.burnt;
                }
            }
        }
    }
}
