using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Player
    [SerializeField] private Player player;

    // Stations
    [SerializeField] private Register registerScript;
    [SerializeField] private List<Counter> counterScripts;
    [SerializeField] private List<GarbageCan> garbageScripts;
    [SerializeField] private List<CoreIngredientStation> coreStation;
    [SerializeField] private PlateDespenser plateDespenser;
    // UI Elements
    [SerializeField] private TextMeshProUGUI cashUI;
    [SerializeField] private TextMeshProUGUI clockUI;
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
        {
            _cash = value;
            cashUI.text = $"${_cash}"; 
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pausedMenu.SetActive(false);
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

    // checks which stations are currently able to be interacted with
    // if player is trying to interact with that station it will do something
    public void InteractWithStation()
    {
        if (registerScript.isInteractable && player.isInteracting)
        {
            registerScript.onInteract();
            player.isInteracting = false;
        }
        for(int i = 0; i <counterScripts.Count; i++)
        {
            if (counterScripts[i].isInteractable && player.isInteracting)
            {
                //Debug.Log("interacting");
                //player.GetComponent<SpriteRenderer>().color= Color.red;
                counterScripts[i].onInteract();
                player.isInteracting = false;
            }
           

        }
        for (int i = 0; i < garbageScripts.Count; i++)
        {
            if (garbageScripts[i].isInteractable && player.isInteracting)
            {
                //Debug.Log("interacting");
                //player.GetComponent<SpriteRenderer>().color= Color.red;
                garbageScripts[i].onInteract();
                player.isInteracting = false;
            }


        }
        for(int i = 0; i < coreStation.Count; i++)
        {
            if (coreStation[i].isInteractable && player.isInteracting)
            {
                coreStation[i].onInteract();
                player.isInteracting = false;
            }
        }
        if (plateDespenser.isInteractable && player.isInteracting)
        {
            plateDespenser.onInteract();
            player.isInteracting = false;
        }

    }
}
