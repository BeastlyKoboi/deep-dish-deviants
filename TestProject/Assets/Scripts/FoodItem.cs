using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodId
{
    dough = 0,
    cheese = 1,
    sauce = 2,
}
public class FoodItem : MonoBehaviour
{

    public FoodId id;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public CoreIngredient generateIngredient(FoodId id)
    {
        return new CoreIngredient(id);
    }
}
