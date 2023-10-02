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
    /*
    [SerializeField]
    protected Plate order
    */
    [SerializeField]
    protected CustomerManager customerManager;

    public float patience;
    private AiState state;

    void Start()
    {
        state = AiState.Entering;
    }

    // Update is called once per frame
    void Update()
    {
        patience -= Time.deltaTime;
        if (patience <= 0 ) 
        {
            state = AiState.Leaving;
        }

        //AI
        switch ( state )
        {
            case AiState.Entering:
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
                //Pathing for later
                break;
        }
    }

    /*
    /// <summary>
    /// Reviews a received pizza order
    /// </summary>
    /// <param name="pizza">Pizza given</param>
    /// <returns>A percentile score out of 100</returns>
    public float ReviewOrder(Plate pizza)
    {
        float successPercentile = 1;
        for(int i = 0; i < pizza.ingredients.length; i++)
        {
            if (i > order.ingredients.length)//extra unexpected toping
            {
                //Reduce percentile
                successPercentile -= 1 / order.ingredients.length;
                continue;
            }
            for (int j = 0; j < order.ingredients.length; j++)
            {
                if (pizza.ingredients[j] == order.ingredients[i])//topping still exists on pizza
                {
                    continue;
                }
            }
            //This occurs if the topping does not exist
            successPercentile -= 1 / order.ingredients.length;
        }
        if (order.ingredients.length > pizza.ingredients.length)//Missing ingredients
        {
            successPercentile -= (1 / order.ingredients.length / 2) * (order.ingredients.length - pizza.ingredients.length);
        }
        if (successPercentile < 0)
            successPercentile = 0;

        return successPercentile * 100;
    }
    */
}
