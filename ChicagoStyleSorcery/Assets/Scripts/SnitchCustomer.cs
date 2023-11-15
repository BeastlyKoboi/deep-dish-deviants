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
    new void Start()
    {
        exclamationPoint.enabled = false;
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        exclamationPoint.transform.position = transform.position + Vector3.up;
        base.Update();
    }

    public void SnitchOn()
    {
        Leave();

        //Change visual to exclamation point
        exclamationPoint.enabled = true;
        circle.enabled = false;
        trackerImage.enabled = false;
        face.enabled = false;
    }
}
