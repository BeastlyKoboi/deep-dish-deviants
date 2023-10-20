using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class CustomerManager : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    Register register;
    [SerializeField]
    List<PickUpStation> pickUpStationList;
    [SerializeField]
    Customer customerDefault;

    List<Customer> customerList;
    List<FoodId> toppings;

    // Start is called before the first frame update
    void Start()
    {
        //This script will manage all of the customers and keep track of them
        //It may also contain methods to allow customers to communicate with each other

        toppings = new List<FoodId>() {FoodId.onion, FoodId.mushroom, FoodId.olive, FoodId.pepper, FoodId.pepperoni, FoodId.beef, FoodId.bacon ,FoodId.pineapple };

        //List of customers so they can stand in a line
        customerList = new List<Customer>();

        GenerateCustomer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Will find a pickup counter for the customer, will return false if not possible
    /// </summary>
    /// <param name="customer">The customer in question</param>
    /// <returns>If it is possible to find an open counter</returns>
    public bool FindPickupCounter(Customer customer)
    {
        int counter = -1;
        //Find an open register, will choose last open one
        for (int i = pickUpStationList.Count - 1; i >= 0; i--) 
        {
            if (pickUpStationList[i].currentCustomer == null)
            {
                counter = i;
            }
        }
        if (counter == -1)
        {
            return false;
        }

        if (customer != null)
        {
            customer.MoveToStation(counter);
            gameManager.ChangeOrder(customer.SeeOrder(), counter + 1);

            return true;
        }
        return false;
    }


    /// <summary>
    /// Link customer to a given pickup counter
    /// </summary>
    /// <param name="counter">Counter to link to</param>
    /// <param name="c">Customer to link</param>
    public void SetToPickupCounter(int counter, Customer c)
    {
        if (pickUpStationList[counter].currentCustomer == null) //Should always be true, but just in case
            pickUpStationList[counter].currentCustomer = c;
    }

    /// <summary>
    /// Link customer to the register
    /// </summary>
    /// <param name="c">The customer in question</param>
    public void SetToRegister(Customer c)
    {
        register.currentCustomer = c;
    }

    /// <summary>
    /// Add money based on order
    /// </summary>
    /// <param name="mult">Multiplier for money gained</param>
    public void AddMoney(float mult)
    {
        //hardcoded to $10 for now
        gameManager.addScore(10, mult);
    }

    /// <summary>
    /// Make a new cutomer
    /// </summary>
    public void GenerateCustomer()
    {
        Customer c = Instantiate(customerDefault);
        c.GetComponent<Customer>().customerManager = gameObject.GetComponent<CustomerManager>();
        List<FoodId> order = new List<FoodId>() {FoodId.dough, FoodId.cheese, FoodId.sauce };
        // code for generating random pizza orders.
        for(int i = 0; i < 3; i++)
        {
            //for now there is only a 50% chance to get topping which is checked 3 times
            if(UnityEngine.Random.Range(1,11) > 6)
            {
                // gets a random topping
                FoodId toppingToAdd = toppings[UnityEngine.Random.Range(0, toppings.Count)];
                // if the pizza already has that topping on it, it will reroll till it gets a new topping
                // that the pizza does not have
                while (!CheckOrder(toppingToAdd, order))
                {
                    toppingToAdd = toppings[UnityEngine.Random.Range(0, toppings.Count)];
                }
                // adds the topping to the pizza
                order.Add(toppingToAdd); 
            }
        }
        c.SetOrder(order);
        customerList.Add(c);
    }

    /// <summary>
    /// Elevator method for GameManager.ChangeOrder
    /// </summary>
    /// <param name="pizza"></param>
    public void ChangeOrder(List<FoodId> pizza, int station)
    {
        gameManager.ChangeOrder(pizza, station);
    }

    /// <summary>
    /// checks if the a pizza already contains a specific food item. necessary to prevent random
    /// pizza generator from ordering a pizza with duplicate ingredients
    /// </summary>
    /// <returns></returns>
    public bool CheckOrder(FoodId itemToCheck, List<FoodId> pizza)
    {
        for(int i = 0; i < pizza.Count; i++)
        {
            if (pizza[i] == itemToCheck)
            {
                return false;
            }
        }
        return true;
    }
}
