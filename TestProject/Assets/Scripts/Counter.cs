using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : Station
{
    public FoodItem[] inventory;
    
    // Start is called before the first frame update
    void Start()
    {
        inventory = new FoodItem[1];
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public override void onInteract()
    {

    }
 
}
