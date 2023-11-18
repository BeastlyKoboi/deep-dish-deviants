using System.Collections;
using System.Collections.Generic;
// using UnityEditor.Overlays;
using UnityEngine;

public class Warden : MonoBehaviour
{
    private AiState state;

    public CustomerManager customerManager;

    public float maxTime = 1200;
    private float timeLeft;

    protected Vector3 lerpAnchor = Vector3.zero;
    protected static float lerpDurration = 3f;
    protected float lerpTimer = 0;

    [SerializeField]
    private Vector3 standingPosition;
    [SerializeField]
    private Vector3 doorPosition;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = doorPosition;
        lerpAnchor = transform.position;
        lerpTimer = 0;

        timeLeft = maxTime;

        state = AiState.Entering;
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

    public void Leave()
    {
        timeLeft = 0;
    }
}
