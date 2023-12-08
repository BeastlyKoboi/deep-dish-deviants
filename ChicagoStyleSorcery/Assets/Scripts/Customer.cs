using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

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
    protected Canvas patienceCanvas;
    [SerializeField]
    protected Image trackerImage;

    public CashPopup popup;

    [SerializeField]
    protected List<FoodId> order;
    
    [SerializeField]
    public CustomerManager customerManager;

    [SerializeField]
    protected Image face;

    [SerializeField]
    private Sprite happy;
    [SerializeField]
    private Sprite neutral;
    [SerializeField]
    private Sprite sad;

    //In seconds
    [SerializeField]
    public float maxPatience;

    //Value 0 - 1, makes customer more picky at higher values
    [SerializeField]
    private float difficultyFloat;

    [SerializeField]
    private Vector3 doorPosition;

    public List<Vector3> linePositions;
    public int linePosition;

    protected Vector3 lerpAnchor= Vector3.zero;
    protected static float lerpDurration = 3f;
    protected float lerpTimer = 0;

    public List<PickUpStation> pickupStations;
    public Register register;

    public float patience;
    protected AiState state;
    protected bool patienceFreeze;
    public int pickupChosen;
    public int id;

    public AiState State
    {
        get { return state; }
    }

    protected void Start()
    {
        state = AiState.Entering;
        patience = maxPatience;
        patienceFreeze = false;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!patienceFreeze)
        {
            if (state == AiState.InLine || state == AiState.Ordering)//ruduced patience decay while still in line
                patience -= Time.deltaTime/3;
            else
                patience -= Time.deltaTime;
            trackerImage.fillAmount = patience / maxPatience;
        }
        if (patience <= 1 && state != AiState.Leaving)
            Leave();

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
                WaitInLine();
                break;
            case AiState.Ordering://Wait at register
                break;
            case AiState.Waiting://Go to correct pickup station and wait
                if (lerpTimer < lerpDurration)
                {
                    float t = lerpTimer / lerpDurration;
                    t = t * t * (3f - 2f * t);
                    transform.position = Vector3.Lerp(lerpAnchor, pickupStations[pickupChosen].transform.position + Vector3.left, t);//Move to destination in an interlopian curve
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
                {
                    Destroy(gameObject);
                }
                break;
        }
    }

    /// <summary>
    /// Organization method for line
    /// </summary>
    private void WaitInLine()
    {
        if (lerpTimer < lerpDurration)
        {
            float t = lerpTimer / lerpDurration;
            transform.position = Vector3.Lerp(lerpAnchor, linePositions[linePosition] + Vector3.left, t);//Move to destination in an interlopian curve
            lerpTimer += Time.deltaTime;
        }
        else if (linePosition == 0 && state == AiState.InLine)
        {
            state = AiState.Ordering;
            lerpTimer = 0;
            customerManager.SetToRegister(gameObject.GetComponent<Customer>());
        }
    }

    /// <summary>
    /// Make customer check its position and move based on next line position
    /// </summary>
    public void MoveInLine()
    {
        lerpAnchor = transform.position;
        lerpTimer = 0;
        linePosition--;
    }

    /// <summary>
    /// Reviews a received pizza order
    /// </summary>
    /// <param name="pizza">Pizza given</param>
    /// <returns>A percentile score out of 100</returns>
    public float ReviewOrder(Plate pizza)
    {
        float successPercentile = 1;
        float amountFound = 0;

        //variables for tracking mistakes
        MistakeType mistake = MistakeType.None;
        int mistakeWeight = 0; //Measures severity of mistake so more important ones can be prioritized
        int multiMistake = 0;

        for(int i = 0; i < pizza.coreFoodlist.Count; i++)
        {
            bool found = false;
            if (pizza.coreFoodlist[i].foodState == CookState.raw || pizza.coreFoodlist[i].foodState == CookState.burnt) //If uncooked or overcooked
            {
                successPercentile -= (.1f / ((float)order.Count / 3f)); //Reduce percentile
                if (mistakeWeight == 0)
                {
                    if (pizza.coreFoodlist[i].foodState == CookState.raw)
                        mistake = MistakeType.Uncooked;
                    else
                        mistake = MistakeType.Burnt;

                    mistakeWeight = 3;
                    multiMistake++;
                }
            }
            if (pizza.coreFoodlist[i].kneadState == KneadState.unkneaded || pizza.coreFoodlist[i].cutState == CutState.uncut) //If uncut or unkneaded
            {
                successPercentile -= (.1f / ((float)order.Count / 3f)); //Reduce percentile
                if (mistakeWeight == 0)
                {
                    mistake = MistakeType.UncutToppings;
                    mistakeWeight = 3;
                    multiMistake++;
                }
            }
            for (int j = 0; j < order.Count; j++)
            {
                if (pizza.coreFoodlist[i].id == order[j])//topping exists on pizza
                {
                    if (found)//means this topping was already found once
                    {
                        successPercentile -= .05f;
                        if (mistakeWeight <= 1)
                        {
                            mistake = MistakeType.ExtraToppings;//minor reduction
                            mistakeWeight = 1;
                        }
                    }

                    else//First time found, so note that this topping is good
                        amountFound++;
                    found = true;
                }
            }
            for (int k = 0; k < pizza.coreFoodlist.Count; k++)
            {
                if (pizza.coreFoodlist[i] == pizza.coreFoodlist[k] && i != k) //Duplicate topping
                    successPercentile -= .05f;
            }
            if (!found)//unneeded topping
            {
                successPercentile -= .2f / ((float)order.Count / 2f);

                if (mistakeWeight <= 1)
                {
                    mistake = MistakeType.ExtraToppings;//minor reduction
                    mistakeWeight = 2;
                    multiMistake++;
                }
            }
        }
        if (order.Count > amountFound)//Missing ingredients
        {
            successPercentile -= .15f * (order.Count - amountFound);
            if (mistakeWeight <= 2)
            {
                mistake = MistakeType.MissingToppings;//minor reduction
                mistakeWeight = 2;
                multiMistake++;
            }
        }

        //+- 30 based on difference of patience from max patience. Value modified is based on percent of time taken with leniency based off difficulty
        successPercentile += .3f * ((patience + (maxPatience / 2f * (1f - difficultyFloat))) - maxPatience) / maxPatience;

        if (!pizza.IsSorted())
        {
            successPercentile -= .3f;

            if (mistakeWeight <= 2)
            {
                mistake = MistakeType.Order;//minor reduction
                mistakeWeight = 3;
                multiMistake++;
            }
        }

        if (successPercentile < .05f) //Gives player a small amount so it doesn't look like it just didn't work
        {
            successPercentile = UnityEngine.Random.Range(.01f, .05f);
            mistake = MistakeType.Catastrophe;
            mistakeWeight = 10;
        }

        if (multiMistake >= 3)
            mistake = MistakeType.Catastrophe;

        if (pizza.coreFoodlist.Count == 0)
        {
            mistake = MistakeType.PlateFunny;
            mistakeWeight = 10;
        }

        Leave();
        //state = AiState.Leaving;
        //lerpTimer = 0f;
        //lerpAnchor = transform.position;

        //For now we make a new customer
        //customerManager.GenerateCustomer();

        customerManager.AddMoney(successPercentile, mistake);

        //Activate result face
        face.color = Color.white;
        if (successPercentile < .5f) //Set face based on result
            face.sprite = sad;
        else if (successPercentile < .8f)
            face.sprite = neutral;
        else
            face.sprite = happy;

        //Display money popup
        CashPopup c = Instantiate(popup);
        c.money = successPercentile * 10;
        c.transform.position = transform.position + new Vector3(UnityEngine.Random.Range(-.5f, .5f), UnityEngine.Random.Range(.5f, 1f), 0);

        return successPercentile * 100;
    }

    /// <summary>
    /// For the register to take an order
    /// </summary>
    /// <returns>If the order can be taken</returns>
    public bool TakeOrder() 
    {
        if (customerManager.FindPickupCounter(gameObject.GetComponent<Customer>()))
        {
            state = AiState.Waiting;
            lerpAnchor = transform.position;
            linePosition = -1; //No longer consider line
            return true;
        }
        return false;
    }

    /// <summary>
    /// To view the order only
    /// </summary>
    /// <returns>The customer's order</returns>
    public List<FoodId> SeeOrder()
    {
        return order;
    }

    public void MoveToStation(int num)
    {
        if (num < pickupStations.Count)
        {
            pickupChosen = num;
            lerpAnchor = transform.position;
        }
    }

    public void SetOrder(List<FoodId> pizza)
    {
        order = pizza;
    }

    /// <summary>
    /// Forces customer to stay patient and resets their patience
    /// </summary>
    public void ForcePatient()
    {
        patienceFreeze = true;
        patience = maxPatience;
        trackerImage.fillAmount = patience / maxPatience;
        trackerImage.color = Color.cyan;
    }

    /// <summary>
    /// Makes a customer leave
    /// </summary>
    public void Leave()
    {
        state = AiState.Leaving;
        lerpAnchor = transform.position;
        lerpTimer = 0;

        //Activate result face
        face.color = Color.white;
        face.sprite = sad;

        if (linePosition != -1)//Shifts other customers in line when they're gone
        {
            customerManager.ShiftLine(linePosition);
        }
    }

    /// <summary>
    /// Makes a customer leave without fail visuals
    /// </summary>
    public void LeaveQuiet()
    {
        state = AiState.Leaving;
        lerpAnchor = transform.position;
        lerpTimer = 0;

        //remove face visual
        patienceCanvas.enabled = false;

        if (linePosition != -1)//Shifts other customers in line when they're gone
        {
            customerManager.ShiftLine(linePosition);
        }
    }
}
