using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : FoodItem
{
    public List<FoodItem> coreFoodlist;

    public Plate()
    {
        coreFoodlist = new List<FoodItem>();
    }
    // Start is called before the first frame update
    void Start()
    {
        coreFoodlist = new List<FoodItem>();
        id = FoodId.plate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // based on the ids of the ingredients. checks to see if pizza was made in the correct order
    private bool IsSorted()
    {
        bool isSorted = true;
        for(int i = 0; i < 3; i++)
        {
            if (i != 2 &&coreFoodlist[i].id >= coreFoodlist[i + 1].id)
            {
                isSorted = false;
            }
        }
        return isSorted;
    }

    public bool AddToPlate(FoodItem itemToAdd)
    {
        if (itemToAdd.id != id)
        {
            coreFoodlist.Add(itemToAdd);
            return true;
        }
        else
        {
            return false;
        }
    }
}
