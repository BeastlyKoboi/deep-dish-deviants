using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Topping : FoodItem
{

    public Topping(FoodId id)
    {
        this.id = id;
    }
    // Start is called before the first frame update
    void Start()
    {
        // checks to make sure that we are this is a topping since
        // topping id numbers are 4 - 11
        // if the id is 3 or less than it is not a topping, this prob wont be an issue
        // but im including it just in case
        if((int) this.id > 3)
        {
            if (this.id == FoodId.beef)
            {
                this.kneadState = KneadState.unkneaded;
                this.cutState = CutState.na;
            } 
            else if(this.id == FoodId.pepperoni)
            {

                this.kneadState = KneadState.na;
                this.cutState = CutState.na;
            }
            else
            {
                this.cutState = CutState.uncut;
                this.kneadState = KneadState.na;
            }
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
