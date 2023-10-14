using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject loadingScreen;
    public SceneLoader sceneLoader;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadGameplay()
    {
        sceneLoader.sceneToLoad = SceneNames.GAMEPLAY;
        sceneLoader.LoadScene();
    }

    public void QuitGame()
    {
        // Eventually some sort of save system maybe
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
