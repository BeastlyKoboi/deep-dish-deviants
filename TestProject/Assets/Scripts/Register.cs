using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Register : Station
{

    public Customer currentCustomer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInteractable)
        {
            GetComponent<SpriteRenderer>().color = Color.magenta;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }

    public override void onInteract()
    {
      if(currentCustomer != null)
        {
            List<FoodId> list = currentCustomer.getOrder(0);
        }
    }
}
