using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreIngredient : FoodItem
{

    public CoreIngredient(FoodId id)
    {
        this.id = id;
    }
    // Start is called before the first frame update
    void Start()
    {
        // dough needs to be needed so if the ingredient is dough we set 
        // its dough state to unkneaded
        if(this.id == FoodId.dough)
        {
            this.kneadState = KneadState.unkneaded;
        }
        // cheese and sauce are not kneaded so they get the not applicable state
        else
        {
            this.kneadState = KneadState.na;
        }
        // none of the core ingredients are cut so they get the not applicable state
        // cheese may be cut but that is not in the plans A.T.M.
        this.cutState = CutState.na;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  
}
