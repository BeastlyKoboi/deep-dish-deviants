using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Station : MonoBehaviour
{
    public bool isInteractable;
    // Start is called before the first frame update
    void Start()
    {
        isInteractable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void onInteract();
    

}
