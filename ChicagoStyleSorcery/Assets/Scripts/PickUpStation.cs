using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpStation : Station
{
    [SerializeField]
    GameManager manager;
    [SerializeField]
    int PickupNumber;
    [SerializeField]
    private AudioSource orderCompletedAudio;

    // pick up station can only hold plate objects
    public Plate inventory;

    public Customer currentCustomer;

    // Start is called before the first frame update
    void Start()
    {
        currentCustomer = null;
        normalColor = Color.gray;
        triggerColor = Color.blue;
        orderCompletedAudio.Stop();
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
        if (player.playerInventory[0] != null && player.playerInventory[0].id == FoodId.plate && currentCustomer != null)
        {
            StartCoroutine(PlaySound(orderCompletedAudio));
            inventory = (Plate) player.playerInventory[0];
            player.playerInventory[0] = null;
            currentCustomer.ReviewOrder(inventory);
            currentCustomer = null;
            manager.EmptyOrder(PickupNumber);
            ClearCounter();
        }
    }

    //This could be importatnt later if we want the pizzas to stay on the coutner for a while then disapear
    public void ClearCounter()
    {
        // destroys all of the food on the plate
       for(int i = 0; i < inventory.coreFoodlist.Count; i++)
       {
            Destroy(inventory.coreFoodlist[i].gameObject);
       }
       //destroys the plate
       Destroy(inventory.gameObject);
    }

    /// <summary>
    /// plays sound for specific time
    /// </summary>
    /// <param name="sound"></param>
    public IEnumerator PlaySound(AudioSource sound)
    {
        float elapsedTime = 0f;
        float duration = 5f;

        sound.Play();
        while (elapsedTime < duration)
        {

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        sound.Stop();
    }
}
