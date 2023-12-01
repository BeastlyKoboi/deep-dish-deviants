using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.SceneManagement;

public class CustomerManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private Register register;
    [SerializeField]
    private List<PickUpStation> pickUpStationList;
    [SerializeField]
    private Customer customerDefault;
    [SerializeField]
    private Customer customerGrump;
    [SerializeField]
    private Customer customerChill;
    [SerializeField]
    private SnitchCustomer snitch;
    [SerializeField]
    private CashPopup popup;
    [SerializeField]
    private MistakePopup mistakePopup;
    [SerializeField]
    private List<Vector3> linePositions;
    [SerializeField]
    private AudioSource doorbellAudio;
    [SerializeField]
    private AudioSource orderReceivedAudio;

    List<Customer> customerList;
    List<FoodId> toppings;

    int customerTicker = 0;

    float difficutlyFloat = 1; //Higher more difficult, 10 is max
    float customerDelayTime = 3;
    float spawnTracker = 0;
    bool loadedCustomer = false;

    [SerializeField]
    private Warden warden;

    private Warden currentWarden;
    public bool wardenActive = false;
    private bool wardenSpawn = false;
    private float wardenSpawnTimer = 15;

    [SerializeField] private bool endOfDay = false;

    private MistakePopup currentMistakePopup;

    // Start is called before the first frame update
    void Start()
    {
        //This script will manage all of the customers and keep track of them
        //It may also contain methods to allow customers to communicate with each other

        toppings = new List<FoodId>() {FoodId.onion, FoodId.mushroom, FoodId.olive, FoodId.pepper, FoodId.pepperoni, FoodId.beef, FoodId.bacon ,FoodId.pineapple };

        //List of customers so they can stand in a line
        customerList = new List<Customer>();

        //No audio should play upon start
        doorbellAudio.Stop();
        orderReceivedAudio.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        //Customer
        if (!endOfDay)
        {
            spawnTracker += Time.deltaTime;
            if (spawnTracker > customerDelayTime)
                loadedCustomer = true;
            if (loadedCustomer)
            {
                if (customerList.Count < linePositions.Count)
                {
                    StartCoroutine(PlaySound(doorbellAudio));
                    GenerateCustomer();
                    loadedCustomer = false;
                    spawnTracker = 0;
                    customerDelayTime = UnityEngine.Random.Range(20 - difficutlyFloat * 2, 40 - difficutlyFloat * 4);
                }
            }

            if (wardenSpawn)
            {
                wardenSpawnTimer -= Time.deltaTime;
                if (wardenSpawnTimer <= 0)
                {
                    Warden w = Instantiate(warden);
                    w.transform.position = new Vector3(-20, -20, 0);
                    wardenSpawnTimer = 15;
                    wardenSpawn = false;
                    wardenActive = true;
                }
            }
        }
    }

    /// <summary>
    /// Will find a pickup counter for the customer, will return false if not possible
    /// </summary>
    /// <param name="customer">The customer in question</param>
    /// <returns>If it is possible to find an open counter</returns>
    public bool FindPickupCounter(Customer customer)
    {
        StartCoroutine(PlaySound(orderReceivedAudio));
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
            customerList.RemoveAt(0);
            //Other customers move
            foreach (Customer c in customerList)
                c.MoveInLine();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Shifts line positions of customers, used when one has to leave prematurely
    /// </summary>
    public void ShiftLine(int posiitonInLine)
    {
        for (int i = linePositions.Count - 1; i > posiitonInLine; i--)
        {
            if (customerList.Count > i)
                customerList[i].MoveInLine();
        }
        if(customerList.Count > 0)
        {
            customerList.RemoveAt(posiitonInLine); //Remove the customer who left to trigger this
        }
        //customerList.RemoveAt(posiitonInLine); //Remove the customer who left to trigger this
    }

    /// <summary>
    /// Ask for line length of line
    /// </summary>
    /// <returns>Number of customers in line</returns>
    public int GetLineLength()
    {
        return customerList.Count;
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
    public void AddMoney(float mult, MistakeType mistake)
    {
        //hardcoded to $10 for now
        gameManager.addScore(10, mult);

        if (mistake != MistakeType.None) //Create and set the mistake popup
        {
            MistakePopup m = Instantiate(mistakePopup);
            m.mistakeType = mistake;

            if (currentMistakePopup != null)
                currentMistakePopup.RemoveQuick();

            currentMistakePopup = m;
        }
    }

    /// <summary>
    /// Make a new cutomer
    /// </summary>
    public void GenerateCustomer()
    {
        if (customerList.Count >= linePositions.Count)//Should never happen, just a failsafe
            return;

        //Randomly choose customer type
        Customer c;
        if (SceneManager.GetActiveScene().name != "Gameplay") //Regular customers only in tutorial
            c = Instantiate(customerDefault);
        else if (UnityEngine.Random.Range(0, difficutlyFloat) < .5f)
            c = Instantiate(customerChill);
        else if (UnityEngine.Random.Range(difficutlyFloat, 11) > 10.5f)
            c = Instantiate(customerGrump);
        else if (UnityEngine.Random.Range(0, 9) < 1)
            c = Instantiate(snitch);
        else
            c = Instantiate(customerDefault);
        c.customerManager = gameObject.GetComponent<CustomerManager>();
        c.pickupStations = pickUpStationList;
        c.register = register;
        c.id = customerTicker; customerTicker++;
        c.popup = popup;
        c.linePositions = linePositions;
        c.linePosition = customerList.Count; //Since this calls before this customer is added the position will be correct despite using count
        List<FoodId> order = new List<FoodId>() {FoodId.dough, FoodId.cheese, FoodId.sauce };
        int dayModifier = gameManager.CurrentDay;
        if(dayModifier >= 8)
        {
            dayModifier = 7;
        }
        // code for generating random pizza orders.
        for(int i = 0; i < 3; i++)
        {
            //for now there is only a 50% chance to get topping which is checked 3 times
            // depending the the number of days player has gone through, chance for toppings increase
            if(UnityEngine.Random.Range(1,11) > (9 - dayModifier))
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

    public void RemoveCustomer(int id)
    {
        for (int i = 0; i < customerList.Count; i++)
        {
            if (customerList[i].id == id)
                customerList.RemoveAt(i);
        }
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

    /// <summary>
    /// Method to call when the day ends
    /// </summary>
    public void EndDay()
    {
        for (int i = customerList.Count - 1; i >= 0; i--)
        {
            customerList[i].Leave();
        }
        if(register.currentCustomer!= null)
        {
            register.currentCustomer.Leave();
        }
        for(int i = 0; i < pickUpStationList.Count; i++)
        {
            if (pickUpStationList[i].currentCustomer != null)
            {
                pickUpStationList[i].currentCustomer.Leave();
            }
        }

        endOfDay = true;
        spawnTracker = 0;
        //warden.Leave();
        currentWarden.Leave();

        Debug.Log("Customer End Day called");
    }

    public void StartDay()
    {
        endOfDay = false; 
    }

    public void CheckForSnitch()
    {
        if (wardenActive)
        {
            //TODO: Game Over
        }

        bool snitched = false;
        for (int i = customerList.Count - 1; i >= 0; i--)
        {
            Customer c = customerList[i];
            if (c.GetComponent<SnitchCustomer>() != null)
            {
                snitched = true;
                c.GetComponent<SnitchCustomer>().SnitchOn();
            }
        }
        for (int i = pickUpStationList.Count - 1; i >= 0; i--)
        {
            PickUpStation p = pickUpStationList[i];
            if (p.currentCustomer != null && p.currentCustomer.GetComponent<SnitchCustomer>() != null)
            {
                snitched = true;
                p.currentCustomer.GetComponent<SnitchCustomer>().SnitchOn();
            }
        }
        if (register.currentCustomer != null && register.currentCustomer.GetComponent<SnitchCustomer>() != null)
        {
            snitched = true;
            register.currentCustomer.GetComponent<SnitchCustomer>().SnitchOn();
        }

        if (snitched)
        {
            wardenSpawn = true;
        }
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
