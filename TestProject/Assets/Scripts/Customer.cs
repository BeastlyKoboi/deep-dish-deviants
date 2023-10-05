using System.Collections;
using System.Collections.Generic;
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
    private Vector3[] counterPositions; 

    protected float patience;
    private AiState state;
    protected bool patienceFreeze;

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
            state = AiState.Leaving;

        //AI
        switch ( state )
        {
            case AiState.Entering:
                transform.position = registerPosition;
                state = AiState.Ordering;
                //Pathing for later
                break;
            case AiState.InLine:
                //Pathing for later
                break;
            case AiState.Ordering:
                //Link to ordering counter so order can be received
                break;
            case AiState.Waiting:
                //Link to counter so pizza cn be received
                break;
            case AiState.Leaving:
                Destroy(gameObject);
                //Pathing for later
                break;
        }
    }

    //TODO: integrate ingredient states once they exist
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
            if (i > order.Count)//extra unexpected toping
            {
                //Reduce percentile
                successPercentile -= 1 / order.Count;
                continue;
            }
            for (int j = 0; j < order.Count; j++)
            {
                if (pizza.coreFoodlist[i].id == order[j])//topping exists on pizza
                {
                    continue;
                }
            }
            //This occurs if the topping does not exist
            successPercentile -= 1 / order.Count;
        }
        if (order.Count > pizza.coreFoodlist.Count)//Missing ingredients
        {
            successPercentile -= (1 / (order.Count / 2)) * (order.Count - pizza.coreFoodlist.Count);
        }

        //+- 30 based on difference of patience from max patience. Value modified is based on percent of time taken with leniency based off difficulty
        successPercentile += .3f * ((patience + (maxPatience / 2 * (1 - difficultyFloat))) - maxPatience) / maxPatience;

        if (!pizza.IsSorted())
            successPercentile -= .3f;

        if (successPercentile < 0)
            successPercentile = 0;

        state = AiState.Leaving;

        //For now we make a new customer
        customerManager.GenerateCustomer();

        customerManager.AddMoney(successPercentile);
        return successPercentile * 100;
    }

    public List<FoodId> getOrder() 
    {
        if (customerManager.SetToPickupCounter(this))
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
        if (num < counterPositions.Length)
        {
            transform.position = counterPositions[num];
        }
    }
}
