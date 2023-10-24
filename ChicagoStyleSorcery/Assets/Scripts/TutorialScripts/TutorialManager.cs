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
                break;
            case "Tutorial4":
                break;
            case "Tutorial5":
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
