using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SnitchCustomer : Customer
{
    [SerializeField]
    private Image exclamationPoint;
    [SerializeField]
    private Image circle;
    // Start is called before the first frame update
    void Start()
    {
        exclamationPoint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        exclamationPoint.transform.position = transform.position + Vector3.up;
    }

    public void SnitchOn()
    {
        Leave();

        //Change visual to exclamation point
        exclamationPoint.enabled = true;
        circle.enabled = false;
        trackerImage.enabled = false;
    }
}
