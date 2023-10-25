using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : FoodItem
{
    // list that contains all of the ingredients and toppings on any given pizza
    // at the moment max amount of toppings is 3 so this list should be no larger than 6
    public List<FoodItem> coreFoodlist;

    public Plate()
    {
        
        coreFoodlist = new List<FoodItem>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(coreFoodlist.Count == 0)
        {
            coreFoodlist = new List<FoodItem>();
        }
        
        id = FoodId.plate;
        this.cutState = CutState.na;
        this.kneadState= KneadState.na;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // based on the ids of the ingredients. checks to see if pizza was made in the correct order
    public bool IsSorted()
    {
        if (coreFoodlist.Count > 6 || coreFoodlist.Count < 3)
            return false;
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
        if (itemToAdd.id != id && coreFoodlist.Count < 6)
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
