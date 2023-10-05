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

    //Will find a pickup counter for the customer, will return false if not possible
    public bool SetToPickupCounter(Customer customer)
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
            pickUpStationList[counter].currentCustomer = customer;
            customer.MoveToStation(counter);
            
            return true;
        }
        return false;
    }

    public void AddMoney(float mult)
    {
        //hardcoded to $10 for now
        gameManager.addScore(10, mult);
    }

    public void GenerateCustomer()
    {
        Customer c = Instantiate(customerDefault);
        c.GetComponent<Customer>().customerManager = gameObject.GetComponent<CustomerManager>();
        register.currentCustomer = c;
    }

    public void ChangeOrder(List<FoodId> pizza)
    {
        gameManager.ChangeOrder(pizza);
    }
}
