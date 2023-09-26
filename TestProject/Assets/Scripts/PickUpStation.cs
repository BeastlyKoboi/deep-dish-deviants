using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpStation : Station
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInteractable)
        {
            GetComponent<SpriteRenderer>().color = Color.gray;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }

    public override void onInteract()
    {
        
    }
}
