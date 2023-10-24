using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Register : Station
{
    [SerializeField]
   // GameManager manager;

    public Customer currentCustomer;
    // Start is called before the first frame update
    void Start()
    {
        normalColor = Color.white;
        triggerColor= Color.yellow; 
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
        if(currentCustomer != null)
        {
            if (currentCustomer.TakeOrder())
                currentCustomer = null;
        }
    }
}
