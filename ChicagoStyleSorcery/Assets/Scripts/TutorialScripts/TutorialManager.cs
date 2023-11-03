using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TutorialManager : MonoBehaviour
{ 

    // Player will be present in all tutorial scenes
    [SerializeField]
    Player player;

    [Header("CheckBox sprites")]
    [SerializeField]
    Sprite checkedBox;
    [SerializeField]
    Sprite uncheckedBox;

    [Header("Tutorial 1 Fields")]
    // Tutorial 1 Objects
    [SerializeField]
    List<Counter> counterList1;
    [SerializeField]
    List<GameObject> areas;
    [SerializeField]
    Topping beef;
    [SerializeField]
    Topping pineapple;
    [SerializeField]
    List<SpriteRenderer> toDoListChecks1;
    [SerializeField]
    SpriteRenderer helpScreen1;
    [SerializeField]
    Sprite help1Screen2;

    [Header("Tutorial 2 Fields")]
    //Tutorial 2 Objects
    [SerializeField]
    List<Counter> counterList2;
    [SerializeField]
    List<SpriteRenderer> toDoListChecks2;
    bool pickedUpPizza;
    bool tookOrder;
    bool completedOrder;
    [Header("Tutorial 3 Fields")]
    //Tutorial 3 Objects
    [SerializeField]
    List<Counter> counterList3;
    [SerializeField]
    List<SpriteRenderer> toDoListChecks3;
    [SerializeField]
    SpriteRenderer help3screen;
    [SerializeField]
    Sprite help3screen2;
    bool gotPlate;
    bool addedToPlate;
    bool trashedPlate;

    [Header("Tutorial 4 Fields")]
    //Tutorial 4 Objects
    [SerializeField]
    List<Counter> counterList4;
    float timer = 10;
    bool putToppingOnPizza;
    float learnTimer = 20;
    [SerializeField]
    List<SpriteRenderer> toDoListChecks4;

    [Header("Tutorial 5 Fields")]
    // Tutorial 5 Objects
    [SerializeField]
    List<Counter> counterList5;
    [SerializeField]
    List<SpriteRenderer> toDoListChecks5;
    [SerializeField]
    List<Sprite> helpScreens5;
    [SerializeField]
    SpriteRenderer helpSprite5;

    [SerializeField]
    TextMeshProUGUI countdownUI;

    [SerializeField]
    List<Station> sceneSpecificStations;

    string currentScene;
    int numAreasReached;
    // Start is called before the first frame update
    void Start()
    {
        countdownUI.text = string.Empty;
        timer = 10;
        currentScene= SceneManager.GetActiveScene().name;
        // will do different things based on which tutorial scene is currently running
        switch(currentScene){
            case "Tutorial1":
                counterList1[0].inventory[0] =Instantiate(beef);
                counterList1[1].inventory[0] = Instantiate(pineapple);
                counterList1[0].SetIcons();
                counterList1[1].SetIcons();
               
                break;
            case "Tutorial2":
                pickedUpPizza = false ;
                tookOrder = false;
                completedOrder = false;
                break;
            case "Tutorial3":
                gotPlate = false;
                addedToPlate = false;
                trashedPlate = false;
                
                break;
            case "Tutorial4":
                timer = 10;
                putToppingOnPizza= false;
                break;
            case "Tutorial5":
                timer = 5; 
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        InteractWithStation();
        switch (currentScene)
        {
            case "Tutorial1":
                // checks if player has hit all 3 areas
                numAreasReached = 0;
                for (int i = 0; i < areas.Count; i++)
                {
                    if (areas[i].GetComponent<SpriteRenderer>().color == Color.green)
                    {
                        numAreasReached++;
                        toDoListChecks1[i].sprite = checkedBox;
                    }
                }
                if(numAreasReached == 3)
                {
                    helpScreen1.sprite = help1Screen2;
                }
                // checks if player has moved food items between counters
                bool isFoodMoved = false;
                if (counterList1[0].inventory[0] != null && counterList1[1].inventory[0]!= null
                    && counterList1[0].inventory[0].id == FoodId.pineapple && counterList1[1].inventory[0].id == FoodId.beef)
                {
                    isFoodMoved = true;
                    toDoListChecks1[3].sprite = checkedBox;
                }

                if(numAreasReached == 3 && isFoodMoved)
                {
                    countdownUI.text = "Moving to next tutorial in: " + (int) timer;
                    timer -= Time.deltaTime;
                    if(timer < 0)
                    {
                        SceneManager.LoadScene("Tutorial2");
                    }
                   
                }
                break;
            case "Tutorial2":
                if (counterList2[0].inventory[0] == null) {
                    pickedUpPizza = true;
                    toDoListChecks2[1].sprite= checkedBox;
                }

               
                Register tempRegister = (Register)sceneSpecificStations[1];
                PickUpStation tempPickUpStation = (PickUpStation)sceneSpecificStations[2];
                
                if (tempPickUpStation.currentCustomer != null && tempRegister.currentCustomer == null)
                {
                    tookOrder = true;
                    toDoListChecks2[0].sprite = checkedBox;
                }
                if(tookOrder && tempPickUpStation.currentCustomer== null)
                {
                    completedOrder = true;
                    toDoListChecks2[2].sprite = checkedBox;
                }
                if (pickedUpPizza && tookOrder && completedOrder)
                {
                    countdownUI.text = "Moving to next tutorial in: " + (int)timer;
                    timer -= Time.deltaTime;
                    if (timer < 0)
                    {
                        SceneManager.LoadScene("Tutorial3");
                    }
                }
                break;
            case "Tutorial3":
                // if there is a plate on the counter, then the player picked one up and placed it there
                if (counterList3[0].inventory[0] != null && counterList3[0].inventory[0].id == FoodId.plate)
                {
                    gotPlate = true;
                    toDoListChecks3[0].sprite = checkedBox;
                }
                if (!addedToPlate && gotPlate)
                {
                    Plate tempPlat =(Plate) counterList3[0].inventory[0];
                    if(tempPlat.coreFoodlist.Count >= 3)
                    {
                        addedToPlate= true;
                        toDoListChecks3[1].sprite = checkedBox;
                        help3screen.sprite = help3screen2;
                    }
                }
                if(gotPlate && addedToPlate)
                {
                    if (player.playerInventory[0] == null && counterList3[0].inventory[0] == null) 
                    {
                        trashedPlate= true;
                        toDoListChecks3[2].sprite = checkedBox;
                    }
                }
                if(gotPlate && addedToPlate && trashedPlate)
                {
                    countdownUI.text = "Moving to next tutorial in: " + (int) timer;
                    timer -= Time.deltaTime;
                    if(timer < 0)
                    {
                        SceneManager.LoadScene("Tutorial4");
                    }
                   
                }
                break;
            case "Tutorial4":
                learnTimer -= Time.deltaTime;
                if(learnTimer < 0)
                {
                    toDoListChecks4[0].sprite= checkedBox;
                }
                if (counterList4[0].inventory[0] != null)
                {
                    Plate toppingPlate = (Plate)counterList4[0].inventory[0];
                    if(toppingPlate.coreFoodlist.Count > 3)
                    {
                        putToppingOnPizza = true;
                        toDoListChecks4[1].sprite = checkedBox;
                    }
                }
                if (putToppingOnPizza && learnTimer <= 0) 
                {
                    countdownUI.text = "Moving to next tutorial in: " + (int)timer;
                    timer -= Time.deltaTime;
                }
                if (timer < 0)
                {
                    SceneManager.LoadScene("Tutorial5");
                }
                break;
            case "Tutorial5":
                // bools for if they have completed the scene's assignment
                bool isFoodCooked = false;
                bool isFoodCut = false;
                bool isFoodKneaded = false;
                bool isFoodSorted = false;

                helpSprite5.sprite = helpScreens5[0];
                Plate tempPlate = (Plate)counterList5[0].inventory[0];
                for(int i = 0; i < 3; i++)
                {
                    if (tempPlate.coreFoodlist[i].foodState == CookState.cooked)
                    {
                        isFoodCooked = true;
                        toDoListChecks5[0].sprite = checkedBox;
                        helpSprite5.sprite = helpScreens5[1];
                    }
                    else
                    {
                        isFoodCooked = false;
                    }
                }
                if (counterList5[1].inventory[0].kneadState == KneadState.kneaded)
                {
                    isFoodKneaded = true;
                    toDoListChecks5[1].sprite = checkedBox;
                    helpSprite5.sprite = helpScreens5[2];
                }
                if (counterList5[2].inventory[0].cutState == CutState.cut)
                {
                    isFoodCut = true;
                    toDoListChecks5[2].sprite = checkedBox;
                    helpSprite5.sprite = helpScreens5[3];
                }

                if (counterList5[3].inventory[0] != null)
                {
                    tempPlate = (Plate)counterList5[3].inventory[0];
                    if (tempPlate.IsSorted())
                    {
                        isFoodSorted= true;
                        toDoListChecks5[3].sprite= checkedBox;
                    }
                }
                if(isFoodCooked && isFoodCut && isFoodKneaded && isFoodSorted)
                {
                    countdownUI.text = "Returning to Main menu in: " +(int) timer;
                    timer -= Time.deltaTime;
                    if (timer < 0)
                    {
                        SceneManager.LoadScene("MainMenu");
                    }
                    
                }
                break;
        }
        
        

       
    }

    public void InteractWithStation()
    {
        // gets station closest to player
        Station closestStation = GetStationClosestToPlayer(sceneSpecificStations);
        // if that station is interactable, change color
        if (closestStation.isInteractable)
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


    public Station GetStationClosestToPlayer(List<Station> stationList)
    {
        float closestToPlayer = float.MaxValue;
        Station closestStation = null;
        // loops through all the stations and calculates distance between the station and the player
        // when it finds the closest station it sets closestStation = that station
        for (int i = 0; i < stationList.Count; i++)
        {
            if (Vector3.Distance(player.transform.position, stationList[i].transform.position) < closestToPlayer)
            {
                closestToPlayer = Vector3.Distance(player.transform.position, stationList[i].transform.position);
                closestStation = stationList[i];
            }
            else
                stationList[i].GetComponent<SpriteRenderer>().color = stationList[i].normalColor;
        }
        return closestStation;
    }
}
