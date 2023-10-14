using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public CanvasGroup canvasGroup;
    public string sceneToLoad;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene()
    {
        StartCoroutine(StartLoad());
    }

    IEnumerator StartLoad()
    {
        loadingScreen.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreen(1, 1));
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!load.isDone)
        {
            yield return null;
        }
        yield return StartCoroutine(FadeLoadingScreen(0, 1));
        loadingScreen.SetActive(false);
        Destroy(gameObject);
    }

    IEnumerator FadeLoadingScreen(float targetValue, float duration)
    {
        float startValue = canvasGroup.alpha;
        float time = 0;
        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = targetValue;
    }

}
