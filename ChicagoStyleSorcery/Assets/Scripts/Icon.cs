using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Sprite icon;

    Icon(Sprite icon)
    {
        this.icon = icon;
    }

    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = icon;
    }
}
