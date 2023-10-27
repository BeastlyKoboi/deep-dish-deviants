using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TutorialManager : MonoBehaviour
{ 

    // Player will be present in all tutorial scenes
    [SerializeField]
    Player player;


    // Tutorial 1 Objects
    [SerializeField]
    List<Counter> counterList1;
    [SerializeField]
    List<GameObject> areas;
    [SerializeField]
    Topping beef;
    [SerializeField]
    Topping pineapple;

    //Tutorial 2 Objects
    [SerializeField]
    List<Counter> counterList2;


    //Tutorial 3 Objects
    [SerializeField]
    List<Counter> counterList3;
    bool gotPlate;
    bool addedToPlate;
    bool trashedPlate;

    //Tutorial 4 Objects
    [SerializeField]
    List<Counter> counterList4;
    float timer = 10;

    // Tutorial 5 Objects
    [SerializeField]
    List<Counter> counterList5;



    [SerializeField]
    List<Station> sceneSpecificStations;

    string currentScene;
    int numAreasReached;
    // Start is called before the first frame update
    void Start()
    {
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
                break;
            case "Tutorial3":
                gotPlate = false;
                addedToPlate = false;
                trashedPlate = false;
                break;
            case "Tutorial4":
                break;
            case "Tutorial5":
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
                    }
                }
                // checks if player has moved food items between counters
                bool isFoodMoved = false;
                if (counterList1[0].inventory[0] != null && counterList1[1].inventory[0]!= null
                    && counterList1[0].inventory[0].id == FoodId.pineapple && counterList1[1].inventory[0].id == FoodId.beef)
                {
                    isFoodMoved = true;
                }

                if(numAreasReached == 3 && isFoodMoved)
                {
                   SceneManager.LoadScene("Tutorial2");
                }
                break;
            case "Tutorial2":
                break;
            case "Tutorial3":
                // if there is a plate on the counter, then the player picked one up and placed it there
                if (counterList3[0].inventory[0] != null && counterList3[0].inventory[0].id == FoodId.plate)
                {
                    gotPlate = true; 
                }
                if (!addedToPlate && gotPlate)
                {
                    Plate tempPlat =(Plate) counterList3[0].inventory[0];
                    if(tempPlat.coreFoodlist.Count >= 3)
                    {
                        addedToPlate= true;
                    }
                }
                if(gotPlate && addedToPlate)
                {
                    if (player.playerInventory[0] == null && counterList3[0].inventory[0] == null) 
                    {
                        trashedPlate= true;
                    }
                }
                if(gotPlate && addedToPlate && trashedPlate)
                {
                    SceneManager.LoadScene("Tutorial4");
                }
                break;
            case "Tutorial4":
                if(timer > 0)
                {
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

                Plate tempPlate = (Plate)counterList5[0].inventory[0];
                for(int i = 0; i < 3; i++)
                {
                    if (tempPlate.coreFoodlist[i].foodState == CookState.cooked)
                    {
                        isFoodCooked = true;
                    }
                    else
                    {
                        isFoodCooked = false;
                    }
                }
                if (counterList5[1].inventory[0].kneadState == KneadState.kneaded)
                {
                    isFoodKneaded = true;
                }
                if (counterList5[2].inventory[0].cutState == CutState.cut)
                {
                    isFoodCut = true;
                }
                if(isFoodCooked && isFoodCut && isFoodKneaded)
                {
                    SceneManager.LoadScene("MainMenu");
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
