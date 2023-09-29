using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // UI Elements
    [SerializeField] private TextMeshProUGUI cashUI;
    [SerializeField] private TextMeshProUGUI clockUI;

    // Gameplay Variables 
    [SerializeField] private float _cash;
    private bool isPaused = false; 
    [SerializeField] private int currentDay = 1;
    [SerializeField] private int currentHour = 9;
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
            // cashUI.text = $"${_cash}"; 
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Update the clock
        currentTime += Time.deltaTime;
        if (currentTime >= hourLength)
        {
            currentHour++;
            currentTime -= hourLength;
            // clockUI.text = $"{currentHour}:00";
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
            Time.timeScale = 0.0f;
        else
            Time.timeScale = 1.0f;
    }
}
