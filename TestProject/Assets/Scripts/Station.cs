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
    private void OnTriggerEnter2D(Collider2D other)
    {
        isInteractable = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isInteractable = false;
    }
    public abstract void onInteract();
    

}
