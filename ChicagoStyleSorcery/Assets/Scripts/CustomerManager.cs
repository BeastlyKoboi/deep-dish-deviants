using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        //This script will manage all of the customers and keep track of them
        //It may also contain methods to allow customers to communicate with each other
        //List of customers so they can stand in a line
        //Not needed for this sprint so this will be blank for now
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
        for (int i = 0; i < pickUpStationList.Count; i++) 
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
    }

    /// <summary>
    /// Elevator method for GameManager.ChangeOrder
    /// </summary>
    /// <param name="pizza"></param>
    public void ChangeOrder(List<FoodId> pizza)
    {
        gameManager.ChangeOrder(pizza);
    }
}
