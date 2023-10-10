using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public FoodItem[] playerInventory;
    private List<Icon> iconList;

    [SerializeField]
    private Icon icon;

    private Icon icon2;
    private Icon icon3;
    private Icon icon4;
    // is true when player fires aka presses E or right click
    public bool isInteracting;
    // Start is called before the first frame update
    void Start()
    {
        playerInventory = new FoodItem[1] { null };
        isInteracting= false;
        icon4 = Instantiate(icon);
        icon3 = Instantiate(icon);
        icon2 = Instantiate(icon);
        icon = Instantiate(icon);
        icon.transform.position = gameObject.transform.position;
        icon2.transform.position = gameObject.transform.position;
        icon3.transform.position = gameObject.transform.position;
        icon4.transform.position = gameObject.transform.position;
        iconList = new List<Icon>();
    }

    // Update is called once per frame
    void Update()
    {
        icon.transform.position = gameObject.transform.position;
        icon2.transform.position = gameObject.transform.position;
        icon3.transform.position = gameObject.transform.position;
        icon4.transform.position = gameObject.transform.position;

        if (iconList.Count == 0) //Must happen here or else original icon will not have time to instantiate, unfortunately means this is hardcoded until I find a better solution
        {
            iconList.Add(icon);
            icon2.GetComponent<SpriteRenderer>().sortingOrder = 2;
            iconList.Add(icon2);
            icon3.GetComponent<SpriteRenderer>().sortingOrder = 3;
            iconList.Add(icon3);
            icon4.GetComponent<SpriteRenderer>().sortingOrder = 4;
            iconList.Add(icon4);
        }

        for (int i = 0; i < iconList.Count; i++)//Invis all before reset
        {
            iconList[i].Invisible();
        }

        if (playerInventory[0] == null)//all invis
        {
            //Leave invis
        }
        else if (playerInventory[0].id != FoodId.plate)//All but 1 invis
        {
            iconList[0].SetIconType(playerInventory[0]);
        }
        else//Fill out plate icons
        {
            Plate tempPlate = (Plate)playerInventory[0];
            iconList[0].SetIconType(FoodId.plate);
            for (int i = 0; i < tempPlate.coreFoodlist.Count; i++)
            {
                if (i < iconList.Count)//Temp saftey net
                    iconList[i + 1].SetIconType(tempPlate.coreFoodlist[i]);
            }
        }

        /*
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

        }*/
        GetComponent<SpriteRenderer>().color = Color.white;
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
            else
            {
                Plate tempPlate = (Plate)playerInventory[0];
                foreach (FoodItem f in tempPlate.coreFoodlist)
                {
                    // if raw, made cooked
                    if (f.foodState == CookState.raw)
                    {
                        f.foodState = CookState.cooked;
                    }
                    // if cooked, made burnt
                    else if (f.foodState == CookState.cooked)
                    {
                        f.foodState = CookState.burnt;
                    }
                }
                playerInventory[0] = tempPlate;
            }
        }
    }
}
