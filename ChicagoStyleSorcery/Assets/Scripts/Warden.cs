using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warden : MonoBehaviour
{
    private AiState state;

    public CustomerManager customerManager;

    public float maxTime = 60;
    private float timeLeft;

    protected Vector3 lerpAnchor = Vector3.zero;
    protected static float lerpDurration = 3f;
    protected float lerpTimer = 0;

    [SerializeField]
    private Vector3 standingPosition;
    [SerializeField]
    private Vector3 doorPosition;
    [SerializeField]
    private Image exclamation;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = doorPosition;
        lerpAnchor = transform.position;
        lerpTimer = 0;

        timeLeft = maxTime;

        state = AiState.Entering;

        exclamation.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case AiState.Entering:
                if (lerpTimer < lerpDurration)
                {
                    float t = lerpTimer / lerpDurration;
                    t = t * t * (3f - 2f * t);
                    transform.position = Vector3.Lerp(lerpAnchor, standingPosition, t);//Move to destination in an interlopian curve
                    lerpTimer += Time.deltaTime;
                }
                else
                {
                    state = AiState.Waiting;
                }
                break;
            case AiState.Leaving:
                if (lerpTimer < lerpDurration)
                {
                    float t = lerpTimer / lerpDurration;
                    t = t * t * (3f - 2f * t);
                    transform.position = Vector3.Lerp(lerpAnchor, doorPosition, t);//Move to destination in an interlopian curve
                    lerpTimer += Time.deltaTime;
                }
                else
                {
                    customerManager.wardenActive = false;
                    Destroy(gameObject);
                }
                break;
        }

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            state = AiState.Leaving;
            transform.position = doorPosition;
            lerpAnchor = transform.position;
            lerpTimer = 0;
        }
    }

    /// <summary>
    /// Makes the warden leave
    /// </summary>
    public void Leave()
    {
        timeLeft = 0;
    }

    /// <summary>
    /// Effects to trigger for catching the player
    /// </summary>
    public void CatchPlayer()
    {
        exclamation.enabled = true;
        timeLeft = float.MaxValue; //Never leave
    }
}
