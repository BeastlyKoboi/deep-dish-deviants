using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public enum AiState
{
    Entering = 0,
    InLine = 1,
    Ordering = 2,
    Waiting = 3,
    Leaving = 4,
}
public class Customer : MonoBehaviour
{

    [SerializeField]
    protected List<FoodId> order;
    
    [SerializeField]
    public CustomerManager customerManager;

    //In seconds
    [SerializeField]
    private float maxPatience;

    //Value 0 - 1, makes customer more picky at higher values
    [SerializeField]
    private float difficultyFloat;

    [SerializeField]
    private Vector3 registerPosition;
    [SerializeField]
    private Vector3[] pickupPositions;
    [SerializeField]
    private Vector3 doorPosition;

    private Vector3 lerpAnchor= Vector3.zero;
    private static float lerpDurration = 3f;
    private float lerpTimer = 0;

    protected float patience;
    private AiState state;
    protected bool patienceFreeze;
    public int pickupChosen;

    void Start()
    {
        state = AiState.Entering;
        patience = maxPatience;
        patienceFreeze = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!patienceFreeze)
            patience -= Time.deltaTime;
        if (patience <= 0 )
        {//TODO: address issue where this can cause problems in a line if this customer is attached to the register
            state = AiState.Leaving;
            lerpTimer = 0f;
            lerpAnchor = transform.position;
        }

        //AI
        switch ( state )
        {
            case AiState.Entering://Spawn at front Door
                transform.position = doorPosition;
                lerpAnchor = transform.position;
                lerpTimer = 0;
                state = AiState.InLine;
                break;
            case AiState.InLine://Wait in line for register
                if (lerpTimer < lerpDurration)
                {
                    float t = lerpTimer / lerpDurration;
                    t = t * t * (3f - 2f * t);
                    transform.position = Vector3.Lerp(lerpAnchor, registerPosition, t);//Move to destination in an interlopian curve
                    lerpTimer += Time.deltaTime;
                }
                else
                {
                    state = AiState.Ordering;
                    lerpTimer = 0;
                    customerManager.SetToRegister(gameObject.GetComponent<Customer>());
                }
                break;
            case AiState.Ordering://Wait at register
                break;
            case AiState.Waiting://Go to correct pickup station and wait
                if (lerpTimer < lerpDurration)
                {
                    float t = lerpTimer / lerpDurration;
                    t = t * t * (3f - 2f * t);
                    transform.position = Vector3.Lerp(lerpAnchor, pickupPositions[pickupChosen], t);//Move to destination in an interlopian curve
                    lerpTimer += Time.deltaTime;
                }
                else
                {
                    customerManager.SetToPickupCounter(pickupChosen, gameObject.GetComponent<Customer>());
                }
                break;
            case AiState.Leaving://Go to door and despawn
                if (lerpTimer < lerpDurration)
                {
                    float t = lerpTimer / lerpDurration;
                    t = t * t * (3f - 2f * t);
                    transform.position = Vector3.Lerp(lerpAnchor, doorPosition, t);//Move to destination in an interlopian curve
                    lerpTimer += Time.deltaTime;
                }
                else
                    Destroy(gameObject);
                break;
        }
    }

    /// <summary>
    /// Reviews a received pizza order
    /// </summary>
    /// <param name="pizza">Pizza given</param>
    /// <returns>A percentile score out of 100</returns>
    public float ReviewOrder(Plate pizza)
    {
        float successPercentile = 1;
        for(int i = 0; i < pizza.coreFoodlist.Count; i++)
        {
            bool found = false;
            //If uncooked or overcooked
            if (pizza.coreFoodlist[i].foodState == CookState.raw || pizza.coreFoodlist[i].foodState == CookState.burnt)
            {
                successPercentile -= (1f / ((float)order.Count / 4f));
            }
            if (i > order.Count)//extra unexpected toping
            {
                //Reduce percentile
                successPercentile -= 1f / ((float)order.Count / 4f);
                found = true;
            }
            for (int j = 0; j < order.Count; j++)
            {
                if (pizza.coreFoodlist[i].id == order[j])//topping exists on pizza
                {
                    found = true;
                }
            }
            if (!found)
                successPercentile -= 1f / ((float)order.Count / 2f);
        }
        if (order.Count > pizza.coreFoodlist.Count)//Missing ingredients
        {
            successPercentile -= (1f / (((float)order.Count / 4f)));
        }

        //+- 30 based on difference of patience from max patience. Value modified is based on percent of time taken with leniency based off difficulty
        successPercentile += .3f * ((patience + (maxPatience / 2f * (1f - difficultyFloat))) - maxPatience) / maxPatience;

        if (!pizza.IsSorted())
            successPercentile -= .3f;

        if (successPercentile < .05f)
        {
            successPercentile = .05f;
        }

        state = AiState.Leaving;
        lerpTimer = 0f;
        lerpAnchor = transform.position;

        //For now we make a new customer
        customerManager.GenerateCustomer();

        customerManager.AddMoney(successPercentile);
        return successPercentile * 100;
    }

    public List<FoodId> getOrder() 
    {
        if (customerManager.FindPickupCounter(gameObject.GetComponent<Customer>()))
        {
            state = AiState.Waiting;
        }
        else//We can use this null return later to signify that the order cannot be taken
        {
            return null;
        }
        return order; 
    }

    public void MoveToStation(int num)
    {
        if (num < pickupPositions.Length)
        {
            pickupChosen = num;
            lerpAnchor = transform.position;
        }
    }
}
