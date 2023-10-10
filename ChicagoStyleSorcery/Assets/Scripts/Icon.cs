using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    List<Sprite> plate;
    [SerializeField]
    List<Sprite> dough;
    [SerializeField]
    List<Sprite> cheese;
    [SerializeField]
    List<Sprite> sauce;
    [SerializeField]
    Sprite invisible;

    private FoodId iconType;
    private int row;

    void Start()
    {
        row = 0;
        iconType = FoodId.plate;
    }

    public void SetIconType(FoodItem type)
    {
        switch (type.foodState)
        {
            case (CookState.cooked): row = 1; break;
            case (CookState.raw): row = 0; break;
            case (CookState.burnt): row = 2; break;
        }

        iconType = type.id;
        switch (iconType)
        {
            case (FoodId.dough):
                gameObject.GetComponent<SpriteRenderer>().sprite = dough[row];
                break;
            case (FoodId.cheese):
                gameObject.GetComponent<SpriteRenderer>().sprite = cheese[row];
                break;
            case (FoodId.sauce):
                gameObject.GetComponent<SpriteRenderer>().sprite = sauce[row];
                break;
            case (FoodId.plate):
                gameObject.GetComponent<SpriteRenderer>().sprite = plate[row];
                break;
        }
    }

    public void SetIconType(FoodId type)
    {
        iconType = type;
        switch (iconType)
        {
            case (FoodId.dough):
                gameObject.GetComponent<SpriteRenderer>().sprite = dough[0];
                break;
            case (FoodId.cheese):
                gameObject.GetComponent<SpriteRenderer>().sprite = cheese[0];
                break;
            case (FoodId.sauce):
                gameObject.GetComponent<SpriteRenderer>().sprite = sauce[0];
                break;
            case (FoodId.plate):
                gameObject.GetComponent<SpriteRenderer>().sprite = plate[0];
                break;
        }
    }

    public void Invisible()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = invisible;
    }
}
