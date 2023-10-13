using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{

    public FoodItem[] playerInventory;
    private List<Icon> iconList;
    public float fireCoolDown;
    public bool fireCoolDownActive;
    public float kneedCoolDown;
    public bool kneedCoolDownActive;
    public float cutCoolDown;
    public bool cutCoolDownActive;

    [SerializeField] private List<Counter> counterScripts;

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
        isInteracting = false;
        icon4 = Instantiate(icon);
        icon3 = Instantiate(icon);
        icon2 = Instantiate(icon);
        icon = Instantiate(icon);
        icon.transform.position = gameObject.transform.position;
        icon2.transform.position = gameObject.transform.position;
        icon3.transform.position = gameObject.transform.position;
        icon4.transform.position = gameObject.transform.position;
        iconList = new List<Icon>();
        fireCoolDown = 0;
        fireCoolDownActive = false;
        kneedCoolDown = 0;
        kneedCoolDownActive = false;
        cutCoolDown = 0;
        cutCoolDownActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        //updates if time if cooldown is active
        if (fireCoolDown > 0 && fireCoolDownActive)
        {
            fireCoolDown -= Time.deltaTime;
        }
        // stops cooldown if it is below or at 0
        if (fireCoolDown <= 0)
        {
            fireCoolDown = 0;
            fireCoolDownActive = false;
        }
        //updates if time if cooldown is active
        if (kneedCoolDown > 0 && kneedCoolDownActive)
        {
            kneedCoolDown -= Time.deltaTime;
        }
        // stops cooldown if it is below or at 0
        if (kneedCoolDown <= 0)
        {
            kneedCoolDown = 0;
            kneedCoolDownActive = false;
        }
        //updates if time if cooldown is active
        if (cutCoolDown > 0 && cutCoolDownActive)
        {
            cutCoolDown -= Time.deltaTime;
        }
        // stops cooldown if it is below or at 0
        if (cutCoolDown <= 0)
        {
            cutCoolDown = 0;
            cutCoolDownActive = false;
        }

        //Set constructed icons to player position each frame
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

        for (int i = 0; i < iconList.Count; i++)//Make all icons invisible so they can be refilled
        {
            iconList[i].Invisible();
        }

        if (playerInventory[0] == null)//Leave all icons invisible because there is nothing
        {
            //Leave invis
        }
        else if (playerInventory[0].id != FoodId.plate)//Only bottom icon is filled
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

        GetComponent<SpriteRenderer>().color = Color.white;
    }

    /// <summary>
    /// Fire magic cooks the ingredient the player is holding
    /// </summary>
    public void FireMagic()
    {
        //check if cooldown is inactive
        if (!fireCoolDownActive)
        {
            // cycle through counters and check which one player is trying to interact with
            for (int i = 0; i < counterScripts.Count; i++)
            {
                if (counterScripts[i].isInteractable && isInteracting && counterScripts[i].inventory[0] != null && counterScripts[i].inventory[0].id == FoodId.plate)
                {
                    //cycle through plate inventory and place new food items in new plate with different cooked states
                    Plate tempPlate = (Plate)counterScripts[i].inventory[0];
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
                        counterScripts[i].inventory[0] = tempPlate;
                        counterScripts[i].SetIcons();
                    }
                    fireCoolDown = 3;
                    fireCoolDownActive = true;
                }
            }
        }
    }

    /// <summary>
    /// Cuts toppings that have the cut function
    /// </summary>
    public void CutMagic()
    {
        if (!cutCoolDownActive)
        {
            for (int i = 0; i < counterScripts.Count; i++)
            {
                if (counterScripts[i].isInteractable && counterScripts[i].inventory[0] != null && isInteracting)
                {
                    /*
                    if (counterScripts[i].inventory[0].id == FoodId.mushroom || 
                        counterScripts[i].inventory[0].id == FoodId.onion || 
                        counterScripts[i].inventory[0].id == FoodId.olive || 
                        counterScripts[i].inventory[0].id == FoodId.pepper  ||
                        counterScripts[i].inventory[0].id == FoodId.pepperoni ||
                        counterScripts[i].inventory[0].id == FoodId.bacon ||
                        counterScripts[i].inventory[0].id == FoodId.pineapple)
                    {
                        // if uncut, cut
                        if (counterScripts[i].inventory[0].cutState == CutState.uncut)
                        {
                            counterScripts[i].inventory[0].cutState = CutState.cut;
                        }
                    }
                    */
                    cutCoolDown = 3;
                    cutCoolDownActive = true;
                }
            }
        }
    }

    /// <summary>
    /// Kneeds dough and beef
    /// </summary>
    public void KneedMagic()
    {
        if (!kneedCoolDownActive)
        {
            //checks if player is interacting with a counter
            for (int i = 0; i < counterScripts.Count; i++)
            {
                if (counterScripts[i].isInteractable && counterScripts[i].inventory[0] != null && isInteracting)
                {
                    /*
                    // checks if ID is beef or dough
                    if (counterScripts[i].inventory[0].id == FoodId.dough ||
                        counterScripts[i].inventory[0].id == FoodId.beef)
                    {
                        // if unkneeded, kneed
                        if (counterScripts[i].inventory[0].kneedState == KneedState.unkneeded)
                        {
                            counterScripts[i].inventory[0].kneedState == KneedState.kneeded;
                        }
                    }
                    */
                    kneedCoolDown = 3;
                    kneedCoolDownActive = true;
                }
            }
        }
    }
}