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
        if (!isInteractable)
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
    
    public override void onInteract()
    {

    }
 
}
