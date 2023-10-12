using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    // Player
    [SerializeField] private Player player;

    // Stations
    [SerializeField] private Register registerScript;
    [SerializeField] private List<Counter> counterScripts;
    [SerializeField] private List<GarbageCan> garbageScripts;
    [SerializeField] private List<CoreIngredientStation> coreStations;
    [SerializeField] private PlateDespenser plateDespenser;
    [SerializeField] private List<PickUpStation> pickUpStations;
    private List<Station> allStations = new List<Station>();

    // UI Elements
    [SerializeField] private TextMeshProUGUI cashUI;
    [SerializeField] private TextMeshProUGUI clockUI;
    [SerializeField] private TextMeshProUGUI order1;
    [SerializeField] private GameObject pausedMenu;

    // Gameplay Variables 
    [SerializeField] private float _cash;
    private bool isPaused = false; 
    [SerializeField] private int currentDay = 1;
    [SerializeField] private int currentHour = 8;
    [SerializeField] private float currentTime = 0.0f;
    [SerializeField] private float hourLength = 5.0f;

    [SerializeField] private List<GameObject> currentCustomers = new List<GameObject>();
 
    // Properties 
    public float Cash
    {
        get { return _cash; }
        set
        { _cash = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        pausedMenu.SetActive(false);
        allStations.Add(registerScript);
        allStations.Add(plateDespenser);
        allStations.AddRange(counterScripts);
        allStations.AddRange(pickUpStations);
        allStations.AddRange(garbageScripts);
        allStations.AddRange(coreStations);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        InteractWithStation();
        // Update the clock
        currentTime += Time.deltaTime;
        if (currentTime >= hourLength)
        {
            currentHour++;
            if (currentHour > 12)
                currentHour = 1;

            currentTime -= hourLength;
            clockUI.text = $"{currentHour}:00 {(currentHour < 8? "PM": "AM")}";
        }
        cashUI.text = $"${_cash - _cash % .01}";
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausedMenu.SetActive(isPaused);
        if (isPaused)
            Time.timeScale = 0.0f;
        else
            Time.timeScale = 1.0f;
    }

    public void QuitGame()
    {
        // Eventually some sort of save system maybe
        Application.Quit();
        Debug.Log("Game Quit");
    }

    // checks which stations are currently able to be interacted with
    // if player is trying to interact with that station it will do something
    public void InteractWithStation()
    {
        // gets station closest to player
        Station closestStation = GetStationClosestToPlayer();
        // if that station is interactable, change color
        if(closestStation.isInteractable)
        {
            closestStation.GetComponent<SpriteRenderer>().color = closestStation.triggerColor;
        }
      
        // if player is trying to interact with the station, call that stations interact method
        if (closestStation != null && closestStation.isInteractable && player.isInteracting)
        {
            closestStation.onInteract();
            player.isInteracting = false; 
        }

       
        player.isInteracting = false;
    }

    // returns the station closest to the player
    public Station GetStationClosestToPlayer()
    {
        float closestToPlayer = float.MaxValue;
        Station closestStation=  null;
        // loops through all the stations and calculates distance between the station and the player
        // when it finds the closest station it sets closestStation = that station
        for(int i = 0; i < allStations.Count; i++)
        {
            if (Vector3.Distance(player.transform.position, allStations[i].transform.position) < closestToPlayer)
            {
                closestToPlayer = Vector3.Distance(player.transform.position, allStations[i].transform.position);
                closestStation = allStations[i];
            }
        }
        return closestStation;
    }

    public void addScore(int value, float mult)
    {
        _cash += value * mult;
    }

    public void ChangeOrder(List<FoodId> pizza)
    {
        string order = "Order 1\n";
        for (int i = 0; i < pizza.Count; i++)
        {
            order += StringifyFoodId(pizza[i]) + "\n";
        }
        order1.text = order;
    }

    public void EmptyOrder()
    {
        order1.text = "Order 1";
    }

    public string StringifyFoodId(FoodId id)
    {
        string type = "Error";
        switch (id)
        {
            case FoodId.dough:
                type = "Dough";
                break;
            case FoodId.cheese:
                type = "Cheese";
                break;
            case FoodId.sauce:
                type = "Sauce";
                break;
            case FoodId.plate:
                type = "Plate";
                break;
        }
        return type;
    }

    private void OnDrawGizmosSelected()
    {
        for(int i =0; i < allStations.Count; i++)
        {
            Gizmos.DrawLine(player.transform.position, GetStationClosestToPlayer().transform.position);
        }
    }
}
