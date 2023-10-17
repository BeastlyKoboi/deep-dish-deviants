using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Icon : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    List<Sprite> plate;
    [SerializeField]
    List<Sprite> dough;
    [SerializeField]
    List<Sprite> doughUnKneaded;
    [SerializeField]
    List<Sprite> cheese;
    [SerializeField]
    List<Sprite> sauce;
    [SerializeField]
    List<Sprite> onion;
    [SerializeField]
    List<Sprite> cutOnion;
    [SerializeField]
    List<Sprite> mushroom;
    [SerializeField]
    List<Sprite> cutMushroom;
    [SerializeField]
    List<Sprite> olive;
    [SerializeField]
    List<Sprite> cutOlive;
    [SerializeField]
    List<Sprite> pepper;
    [SerializeField]
    List<Sprite> cutPepper;
    [SerializeField]
    List<Sprite> pepperoni;
    [SerializeField]
    List<Sprite> beef;
    [SerializeField]
    List<Sprite> kneadedBeef;
    [SerializeField]
    List<Sprite> bacon;
    [SerializeField]
    List<Sprite> cutBacon;
    [SerializeField]
    List<Sprite> pineapple;
    [SerializeField]
    List<Sprite> cutPineapple;

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
                if (type.kneadState == KneadState.unkneaded)
                    gameObject.GetComponent<SpriteRenderer>().sprite = doughUnKneaded[row];
                else 
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
            case FoodId.onion:
                if (type.cutState == CutState.cut)
                    gameObject.GetComponent<SpriteRenderer>().sprite = cutOnion[row];
                else
                    gameObject.GetComponent<SpriteRenderer>().sprite = onion[row];
                break;
            case FoodId.mushroom:
                if (type.cutState == CutState.cut)
                    gameObject.GetComponent<SpriteRenderer>().sprite = cutMushroom[row];
                else
                    gameObject.GetComponent<SpriteRenderer>().sprite = mushroom[row];
                break;
            case FoodId.olive:
                if (type.cutState == CutState.cut)
                    gameObject.GetComponent<SpriteRenderer>().sprite = cutOlive[row];
                else
                    gameObject.GetComponent<SpriteRenderer>().sprite = olive[row];
                break;
            case FoodId.pepper:
                if (type.cutState == CutState.cut)
                    gameObject.GetComponent<SpriteRenderer>().sprite = cutPepper[row];
                else
                    gameObject.GetComponent<SpriteRenderer>().sprite = pepper[row];
                break;
            case FoodId.pepperoni:
                gameObject.GetComponent<SpriteRenderer>().sprite = pepperoni[row];
                break;
            case FoodId.beef:
                if (type.kneadState == KneadState.kneaded)
                    gameObject.GetComponent<SpriteRenderer>().sprite = kneadedBeef[row];
                else
                    gameObject.GetComponent<SpriteRenderer>().sprite = beef[row];
                break;
            case FoodId.bacon:
                if (type.cutState == CutState.cut)
                    gameObject.GetComponent<SpriteRenderer>().sprite = bacon[row];
                else
                    gameObject.GetComponent<SpriteRenderer>().sprite = cutBacon[row];
                break;
            case FoodId.pineapple:
                if (type.cutState == CutState.cut)
                    gameObject.GetComponent<SpriteRenderer>().sprite = cutPineapple[row];
                else
                    gameObject.GetComponent<SpriteRenderer>().sprite = pineapple[row];
                break;
        }
    }

    /// <summary>
    /// Set the icons image, ignores cooked, cut and kneaded
    /// </summary>
    /// <param name="type">fooditem type</param>
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
            case FoodId.onion:
                gameObject.GetComponent<SpriteRenderer>().sprite = onion[0];
                break;
            case FoodId.mushroom:
                gameObject.GetComponent<SpriteRenderer>().sprite = mushroom[0];
                break;
            case FoodId.olive:
                gameObject.GetComponent<SpriteRenderer>().sprite = olive[0];
                break;
            case FoodId.pepper:
                gameObject.GetComponent<SpriteRenderer>().sprite = pepper[0];
                break;
            case FoodId.pepperoni:
                gameObject.GetComponent<SpriteRenderer>().sprite = pepperoni[0];
                break;
            case FoodId.beef:
                gameObject.GetComponent<SpriteRenderer>().sprite = beef[0];
                break;
            case FoodId.bacon:
                gameObject.GetComponent<SpriteRenderer>().sprite = cutBacon[0];
                break;
            case FoodId.pineapple:
                gameObject.GetComponent<SpriteRenderer>().sprite = pineapple[0];
                break;
        }
    }

    /// <summary>
    /// Set the icon's image
    /// </summary>
    /// <param name="type">fooditem type</param>
    /// <param name="state">state of food item, 0-raw, 1-cooked, 3-burnt</param>
    /// <param name="kneaded">if food is kneaded, ingored in unapplicable cases</param>
    /// <param name="cut">if food is cut, ignored in unapplicable cases</param>
    public void SetIconType(FoodId type, int state, bool kneaded, bool cut)
    {
        iconType = type;
        switch (iconType)
        {
            case (FoodId.dough):
                if (!kneaded)
                    gameObject.GetComponent<SpriteRenderer>().sprite = doughUnKneaded[state];
                else
                    gameObject.GetComponent<SpriteRenderer>().sprite = dough[state];
                break;
            case (FoodId.cheese):
                gameObject.GetComponent<SpriteRenderer>().sprite = cheese[state];
                break;
            case (FoodId.sauce):
                gameObject.GetComponent<SpriteRenderer>().sprite = sauce[state];
                break;
            case (FoodId.plate):
                gameObject.GetComponent<SpriteRenderer>().sprite = plate[state];
                break;
            case FoodId.onion:
                if (cut)
                    gameObject.GetComponent<SpriteRenderer>().sprite = cutOnion[state];
                else
                    gameObject.GetComponent<SpriteRenderer>().sprite = onion[state];
                break;
            case FoodId.mushroom:
                if (cut)
                    gameObject.GetComponent<SpriteRenderer>().sprite = cutMushroom[state];
                else
                    gameObject.GetComponent<SpriteRenderer>().sprite = mushroom[state];
                break;
            case FoodId.olive:
                if (cut)
                    gameObject.GetComponent<SpriteRenderer>().sprite = cutOlive[state];
                else
                    gameObject.GetComponent<SpriteRenderer>().sprite = olive[state];
                break;
            case FoodId.pepper:
                if (cut)
                    gameObject.GetComponent<SpriteRenderer>().sprite = cutPepper[state];
                else
                    gameObject.GetComponent<SpriteRenderer>().sprite = pepper[state];
                break;
            case FoodId.pepperoni:
                gameObject.GetComponent<SpriteRenderer>().sprite = pepperoni[state];
                break;
            case FoodId.beef:
                if (kneaded)
                    gameObject.GetComponent<SpriteRenderer>().sprite = kneadedBeef[state];
                else
                    gameObject.GetComponent<SpriteRenderer>().sprite = beef[state];
                break;
            case FoodId.bacon:
                if (cut)
                    gameObject.GetComponent<SpriteRenderer>().sprite = bacon[state];
                else
                    gameObject.GetComponent<SpriteRenderer>().sprite = cutBacon[state];
                break;
            case FoodId.pineapple:
                if (cut)
                    gameObject.GetComponent<SpriteRenderer>().sprite = cutPineapple[state];
                else
                    gameObject.GetComponent<SpriteRenderer>().sprite = pineapple[state];
                break;
        }
    }

    public void Invisible()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = invisible;
    }
}
