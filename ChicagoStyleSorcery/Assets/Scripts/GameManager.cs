using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
//using static UnityEditor.Experimental.GraphView.GraphView;

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
    [SerializeField] private List<ToppingStation> toppingStations;
    private List<Station> allStations = new List<Station>();

    // UI Elements
    [SerializeField] private GameObject canvas;
    [SerializeField] private TextMeshProUGUI cashUI;
    [SerializeField] private TextMeshProUGUI clockUI;
    [SerializeField] private TextMeshProUGUI order1;
    [SerializeField] private GameObject pausedMenu;
    [SerializeField] private Animator IntroPopupAnimator;
    [SerializeField] private GameObject tooltipPrefab;
    [SerializeField] private GameObject tooltipParent; // Used to maintain visible order, Intro may pop up behind w/o
    private TooltipInfo tooltip;

    // Gameplay Variables 
    public enum GameState { Open, Closed };
    [SerializeField] private GameState gameState;
    [SerializeField] private float _cash;
    [SerializeField] private bool isPaused = false;
    [SerializeField] private int currentDay = 1;
    [SerializeField] private int currentHour = 8;
    [SerializeField] private float currentTime = 0.0f;
    [SerializeField] private float hourLength = 20.0f;
    [SerializeField] private bool hasTutorial = true;

    [SerializeField] private List<GameObject> currentCustomers = new List<GameObject>();

    [SerializeField] private OrderTag tag1;
    [SerializeField] private OrderTag tag2;
    [SerializeField] private OrderTag tag3;

    // Text Assets
    public TextAsset tooltips;
    string[] tooltipsJSON;
    int currentTooltipNum = 0;

    // Scene Loading 
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private GameObject loadingScreenPrefab;

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
        // Setting up UI start states
        if(pausedMenu != null)
        {
            pausedMenu.SetActive(false);
            IntroPopupAnimator.gameObject.SetActive(true);
        }
       

        // If there a is a loading animation to come, 
        // wait until its over to pause the game
        GameObject loadingScreenObj = GameObject.Find("LoadingScreen");
        if (loadingScreenObj == null)
            ToggleGamePause();
        else
            loadingScreenObj.GetComponent<SceneLoader>().fadeFinished.AddListener(ToggleGamePause);

        if(loadingScreenPrefab!= null)
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

        // Update the 
        if(cashUI!= null)
            cashUI.text = $"${_cash - _cash % .01}";
    }

    public void UpdateClock()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= hourLength)
        {
            currentHour++;
            if (currentHour > 12)
                currentHour = 1;
            if (currentHour == 5)
                EndDay();

            currentTime -= hourLength;
            clockUI.text = $"{currentHour}:00 {(currentHour < 8 ? "PM" : "AM")}";
        }
    }

    // Eventually will force customers to leave,
    // and pull up a screen to tally the restaurants gains and losses that day.
    private void EndDay()
    {
        gameState = GameState.Closed;
        // More stuff to come
    }
    // Eventually I hope to have it called on game start
    // and then when prompted after a day has ended.
    // This will restart the customers coming in and in effect allow the clock to work
    private void BeginDay()
    {
        gameState = GameState.Open;
        // more stuff to come
    }

    public void AnimateIntroPopup()
    {
        ToggleGamePause();
        IntroPopupAnimator.SetTrigger("Close");
        IntroPopupAnimator.SetBool("wasClosed", true);
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
        _cash += value * mult;
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
