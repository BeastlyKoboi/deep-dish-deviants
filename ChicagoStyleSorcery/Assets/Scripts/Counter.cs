using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Counter : Station
{
    public FoodItem[] inventory;
    private List<Icon> iconList;

    [SerializeField]
    private Icon icon;

    private Icon icon2;
    private Icon icon3;
    private Icon icon4;
    private Icon icon5;
    private Icon icon6;
    private Icon icon7;
    //private Icon FirstIcon;

    // Start is called before the first frame update
    void Start()
    {
        inventory = new FoodItem[1];
        icon7 = Instantiate(icon);
        icon6 = Instantiate(icon);
        icon5 = Instantiate(icon);
        icon4 = Instantiate(icon);
        icon3 = Instantiate(icon);
        icon2 = Instantiate(icon);
        icon = Instantiate(icon);
        icon.transform.position = gameObject.transform.position;
        icon2.transform.position = gameObject.transform.position;
        icon3.transform.position = gameObject.transform.position;
        icon4.transform.position = gameObject.transform.position;
        icon5.transform.position = gameObject.transform.position;
        icon6.transform.position = gameObject.transform.position;
        icon7.transform.position = gameObject.transform.position;
        iconList = new List<Icon>();

        normalColor = Color.blue;
        triggerColor = Color.red;
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
        if (inventory[0] != null && inventory[0].id == FoodId.plate && player.playerInventory[0] != null) 
        {

            Plate tempPlate = (Plate)inventory[0];
            if (tempPlate.AddToPlate(player.playerInventory[0]))//Method adds item if can otherwise we want player to keep
            {
                inventory[0] = tempPlate;
                player.playerInventory[0] = null;
            }            
        }
        else if (inventory[0] != null && player.playerInventory[0] == null)
        {
            player.playerInventory[0] = inventory[0];
            inventory[0] = null;
        }
        else if (inventory[0] == null && player.playerInventory[0] != null)
        {
            inventory[0] = player.playerInventory[0];
            player.playerInventory[0] = null;
        }
        else if(inventory[0] != null && player.playerInventory[0] != null)
        {
            CoreIngredient tempIngredient = (CoreIngredient) inventory[0];
            inventory[0] = player.playerInventory[0];
            player.playerInventory[0] = tempIngredient;
        }

        SetIcons();
    }

    /// <summary>
    /// Sets the icons for the given counter, moved out of Interact method for organization and flexibility
    /// </summary>
    public void SetIcons()
    {
        if (iconList.Count == 0) //Must happen here or else original icon will not have time to instantiate, unfortunately means this is hardcoded until I find a better solution
        {
            iconList.Add(icon);
            icon2.GetComponent<SpriteRenderer>().sortingOrder = 2;
            iconList.Add(icon2);
            icon3.GetComponent<SpriteRenderer>().sortingOrder = 3;
            iconList.Add(icon3);
            icon4.GetComponent<SpriteRenderer>().sortingOrder = 4;
            iconList.Add(icon4);
            icon2.GetComponent<SpriteRenderer>().sortingOrder = 5;
            iconList.Add(icon5);
            icon3.GetComponent<SpriteRenderer>().sortingOrder = 6;
            iconList.Add(icon6);
            icon4.GetComponent<SpriteRenderer>().sortingOrder = 7;
            iconList.Add(icon7);
        }

        for (int i = 0; i < iconList.Count; i++)//Invis all before reset
        {
            iconList[i].Invisible();
        }

        if (inventory[0] == null)//all invis
        {
            //Leave invis
        }
        else if (inventory[0].id != FoodId.plate)//All but 1 invis
        {
            iconList[0].SetIconType(inventory[0]);
        }
        else//Fill out plate icons
        {
            Plate tempPlate = (Plate)inventory[0];
            iconList[0].SetIconType(FoodId.plate);
            for (int i = 0; i < tempPlate.coreFoodlist.Count; i++)
            {
                if (i < iconList.Count)//Temp saftey net
                    iconList[i + 1].SetIconType(tempPlate.coreFoodlist[i]);
            }
        }
    }
}
