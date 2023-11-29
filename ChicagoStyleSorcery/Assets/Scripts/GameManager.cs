using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    // Player
    [Header("Player")]
    [SerializeField] private Player player;

    // Stations
    [Header("Station Scripts")]
    [SerializeField] private Register registerScript;
    [SerializeField] private List<Counter> counterScripts;
    [SerializeField] private List<GarbageCan> garbageScripts;
    [SerializeField] private List<CoreIngredientStation> coreStations;
    [SerializeField] private PlateDespenser plateDespenser;
    [SerializeField] private List<PickUpStation> pickUpStations;
    [SerializeField] private List<ToppingStation> toppingStations;
    [SerializeField] public List<Oven> ovenList;
    [SerializeField] public List<Slicer> slicerList;
    [SerializeField] public List<Pounder> pounderList;
    private List<Station> allStations = new List<Station>();

    // UI Elements
    [Header("UI Elements")]
    [SerializeField] private GameObject canvas;
    [SerializeField] private TextMeshProUGUI cashTotalUI;
    [SerializeField] private TextMeshProUGUI currentDayUI;
    [SerializeField] private TextMeshProUGUI clockUI;
    [SerializeField] private GameObject pausedMenu;
    [SerializeField] private Animator IntroPopupAnimator;
    [SerializeField] private GameObject dayEndPopup;
    [SerializeField] private TextMeshProUGUI cashTodayUI;
    [SerializeField] private TextMeshProUGUI pizzasSoldUI;
    [SerializeField] private GameObject tooltipPrefab;
    [SerializeField] private GameObject tooltipParent; // Used to maintain visible order, Intro may pop up behind w/o
    private TooltipInfo tooltip;
    public enum GameState { Open, Closed };

    // Gameplay Variables 
    [Header("Gameplay Variables")]
    [SerializeField] public GameState gameState;
    [SerializeField] private float _cashTotal;
    [SerializeField] private float cashToday = 0;
    [SerializeField] private int numPizzaSoldToday = 0;
    [SerializeField] private bool isPaused = false;
    [SerializeField] private int currentDay = 0;
    [SerializeField] private int currentHour = 8;
    [SerializeField] private float currentTime = 0.0f;
    [SerializeField] private float hourLength = 30.0f; // is overwritten by inspector
    [SerializeField] private bool hasTutorial = true;

    // Customers and Orders
    [Header("Customers and Orders")]
    [SerializeField] CustomerManager customerManager;
    [SerializeField] private List<GameObject> currentCustomers = new List<GameObject>();
    [SerializeField] private OrderTag tag1;
    [SerializeField] private OrderTag tag2;
    [SerializeField] private OrderTag tag3;

    // Text Assets
    [Header("Text Assets")]
    public TextAsset tooltips;
    string[] tooltipsJSON;
    int currentTooltipNum = 0;

    // Scene Loading 
    [Header("Scene Loading")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private GameObject loadingScreenPrefab;

    // Properties 
    public float CashTotal
    {
        get { return _cashTotal; }
        set
        { 
            _cashTotal = value;
            cashTotalUI.text = $"${_cashTotal - _cashTotal % .01}";
        }
    }

    public int CurrentDay
    {
        get { return currentDay; } 
        set
        {
            currentDay = value;
            currentDayUI.text = $"Day {currentDay}";
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // Setting up UI start states
        if(pausedMenu != null)
        {
            pausedMenu.SetActive(false);
            if(IntroPopupAnimator!= null)
                IntroPopupAnimator.gameObject.SetActive(true);
        }

        if (dayEndPopup != null)
            dayEndPopup.SetActive(false);

        // If there a is a loading animation to come, 
        // wait until its over to pause the game
        GameObject loadingScreenObj = GameObject.Find("LoadingScreen");
        if (loadingScreenObj == null)
            ToggleGamePause();
        else
            loadingScreenObj.GetComponent<SceneLoader>().fadeFinished.AddListener(ToggleGamePause);
            // ToggleGamePause();

        if(loadingScreenPrefab != null)
        {
            // saves for use in later methods
            loadingScreenObj = Instantiate(loadingScreenPrefab);
            sceneLoader = loadingScreenObj.GetComponent<SceneLoader>();
            loadingScreen = loadingScreenObj.transform.GetChild(0).gameObject;
        }
       

        // Will be paused once loading is done
        // Then will be unpaused by popup screen

        // combining scripts for later use
        if(counterScripts != null)
            allStations.AddRange(counterScripts);
        if (registerScript != null)
        {
            allStations.Add(registerScript);
        }
        if (plateDespenser != null)
        {
            allStations.Add(plateDespenser);
        }
        
        if(pickUpStations != null)
            allStations.AddRange(pickUpStations);
        if (garbageScripts != null)
            allStations.AddRange(garbageScripts);
        if (coreStations != null)
            allStations.AddRange(coreStations);
        if (toppingStations != null)
            allStations.AddRange(toppingStations);
        if (ovenList != null)
            allStations.AddRange(ovenList);
        if(slicerList!= null)
            allStations.AddRange(slicerList);
        if(pounderList!= null) 
            allStations.AddRange(pounderList);
        // Setting up tooltips
        if (hasTutorial)
        {
            tooltip = Instantiate(tooltipPrefab, tooltipParent.transform, false).GetComponent<TooltipInfo>(); 
            string tooltipsString = tooltips.text;
            tooltipsJSON = tooltipsString.Split(";");

            tooltip.Load(tooltipsJSON[currentTooltipNum]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay")
            InteractWithStation();

        // This is the beginnings of the day/night cycle
        // I intend to fill out the BeginDay and EndDay methods 
        // to handle the gameState. To that end I will need to work 
        // others to implement a freeze state with other systems. 
        // Most important now is one for Customers, and Aiden has 
        // has already implemented this for customers and we just
        // need to connect it with the game manager.
        switch(gameState)
        {
            case GameState.Open:
                UpdateClock();
                break;
            case GameState.Closed:
                break;
        }

    }

    public void UpdateClock()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= hourLength)
        {
            currentHour++;
            if (currentHour > 12)
                currentHour = 1;

            currentTime -= hourLength;
            clockUI.text = $"{currentHour}:00 {(currentHour == 12 || currentHour < 8? "PM" : "AM")}";
            
            if (currentHour == 5)
                EndDay();
        }
    }

    private void ResetClock()
    {
        currentHour = 8;
        currentTime = 0;
        clockUI.text = $"{currentHour}:00 {(currentHour < 8 ? "PM" : "AM")}";
    }

    // Eventually will force customers to leave,
    // and pull up a screen to tally the restaurants gains and losses that day.
    private void EndDay()
    {
        gameState = GameState.Closed;
        dayEndPopup.SetActive(true);
        cashTodayUI.text = $"Earnings: ${cashToday - cashToday % .01}";
        pizzasSoldUI.text = $"Pizza's Sold: {numPizzaSoldToday}";
        cashToday = 0;
        numPizzaSoldToday = 0;

        customerManager.EndDay();
        // More stuff to come
        // when the day ends all of the counters are wiped, 
        foreach(Counter counter in counterScripts)
        {
            if (counter.inventory[0] != null)
            {
                Destroy(counter.inventory[0].gameObject);
                counter.inventory[0] = null;
                counter.SetIcons();
            }
            
        }
    }
    // Eventually I hope to have it called on game start
    // and then when prompted after a day has ended.
    // This will restart the customers coming in and in effect allow the clock to work
    public void BeginDay()
    {
        gameState = GameState.Open;
        ResetClock();
        CurrentDay++;
        dayEndPopup.SetActive(false);

        customerManager.StartDay();
        // more stuff to come
    }

    public void AnimateIntroPopup()
    {
        ToggleGamePause();
        IntroPopupAnimator.SetTrigger("Close");
        IntroPopupAnimator.SetBool("wasClosed", true);
        BeginDay();
    }

    public void OnNextTooltip(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (!IntroPopupAnimator.GetBool("wasClosed"))
        {
            AnimateIntroPopup();
            return;
        }    
        if (!hasTutorial)
        {
            tooltip.gameObject.SetActive(false);
            return;
        }
        if (currentTooltipNum == tooltipsJSON.Length - 1) return;

        currentTooltipNum++;

        // Overwrites the existing tooltip with new info
        tooltip.Load(tooltipsJSON[currentTooltipNum]);
    }

    // Turns off the tutorial messages
    public void OnHideTooltip(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (!IntroPopupAnimator.GetBool("wasClosed"))
            AnimateIntroPopup();
        hasTutorial = false;
        tooltip.gameObject.SetActive(false);
    }

    public void OnTogglePauseMenu(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!IntroPopupAnimator.GetBool("wasClosed"))
        {
            AnimateIntroPopup();
            return;
        }

        ToggleGamePause();
        pausedMenu.SetActive(isPaused);
    }

    private void ToggleGamePause()
    {
        isPaused = !isPaused;
        if (isPaused)
            Time.timeScale = 0.0f;
        else
            Time.timeScale = 1.0f;
    }

    public void LoadMainMenu()
    {
        ToggleGamePause();
        sceneLoader.sceneToLoad = SceneNames.MAINMENU;
        sceneLoader.LoadScene();
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

        // this is for telling player what the closest counter and pickupstations are
        // in order to use magic. since this is called in update, might as well put it here
        // since we already have closest station
        if(closestStation is Counter)
        {
            player.nearestCounter= (Counter)closestStation;
        }

        if(closestStation is PickUpStation)
        {
            player.nearestPickUp = (PickUpStation)closestStation;
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
            else
                allStations[i].GetComponent<SpriteRenderer>().color = allStations[i].normalColor;
        }
        return closestStation;
    }

    public void addScore(int value, float mult)
    {
        CashTotal += value * mult;
        cashToday += value * mult;
        numPizzaSoldToday++;
    }

    /// <summary>
    /// Changes the order of the given tag to the new order, pulls out that tag
    /// </summary>
    /// <param name="pizza">The pizza order</param>
    /// <param name="station">The station number this is for</param>
    public void ChangeOrder(List<FoodId> pizza, int station)
    {
        switch(station)
        {
            case 1:
                tag1.FillTag(pizza);
                tag1.openQuick();
                tag2.closeQuick();
                tag3.closeQuick();
                break;
            case 2:
                tag2.FillTag(pizza);
                tag2.openQuick();
                tag1.closeQuick();
                tag3.closeQuick();
                break;
            case 3:
                tag3.FillTag(pizza);
                tag3.openQuick();
                tag2.closeQuick();
                tag1.closeQuick();
                break;
        }
    }

    /// <summary>
    /// Empties the given order from an order tag
    /// </summary>
    public void EmptyOrder(int station)
    {
        switch (station)
        {
            case 1:
                tag1.EmptyTag();
                break;
            case 2:
                tag2.EmptyTag();
                break;
            case 3:
                tag3.EmptyTag();
                break;
        }
    }

    /// <summary>
    /// Gets string version of FoodItem
    /// </summary>
    /// <param name="id">FoodId of food</param>
    /// <returns>string name of food item</returns>
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
            case FoodId.onion:
                type = "Onion";
                break;
            case FoodId.mushroom:
                type = "Mushroom";
                break;
            case FoodId.olive:
                type = "Olive";
                break;
            case FoodId.pepper:
                type = "Bell Pepper";
                break;
            case FoodId.pepperoni:
                type = "Pepperoni";
                break;
            case FoodId.beef:
                type = "Ground Beef";
                break;
            case FoodId.bacon:
                type = "Bacon";
                break;
            case FoodId.pineapple:
                type = "Pineapple";
                break;
        }
        return type;
    }

    private void OnDrawGizmos()
    {
        for(int i =0; i < allStations.Count; i++)
        {
            Gizmos.DrawLine(player.transform.position, GetStationClosestToPlayer().transform.position);
        }
    }
}
