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
    List<Counter> counterList;
    [SerializeField]
    List<BoxCollider2D> areas;
    [SerializeField]
    Topping beef;
    [SerializeField]
    Topping pineapple;



    // Start is called before the first frame update
    void Start()
    {
        string currentScene= SceneManager.GetActiveScene().name;
        // will do different things based on which tutorial scene is currently running
        switch(currentScene){
            case "Tutorial1":
                counterList[0].inventory[0] =Instantiate(beef);
                counterList[1].inventory[0] = Instantiate(pineapple);
                counterList[0].SetIcons();
                counterList[1].SetIcons();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
