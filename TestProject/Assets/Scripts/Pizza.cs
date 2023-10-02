using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : FoodItem 
{
    CoreIngredient[] coreFoodlist;
    // Start is called before the first frame update
    void Start()
    {
        coreFoodlist = new CoreIngredient[3];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // based on the ids of the ingredients. checks to see if pizza was made in the correct order
    private bool isSorted()
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
}
