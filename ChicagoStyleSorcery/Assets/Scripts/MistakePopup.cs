using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum MistakeType
{
    None,
    Patience,
    WrongToppings,
    Order,
    MissingToppings,
    ExtraToppings,
    UncutToppings,
    Burnt,
    Uncooked,
    Catastrophe,
    PlateFunny
}

public class MistakePopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro text;
    [SerializeField]
    private Image bar;
    [SerializeField]
    private Vector3 startPos;

    public MistakeType mistakeType = MistakeType.None;
    bool textChanged = false;

    private float fade = 0;
    private float fadeTimer = 0;
    private static float fadeMax = 2;
    private bool fadeQuick = false;

    protected Vector3 lerpAnchor = Vector3.zero;
    protected static float lerpDurration = 3f;
    protected float lerpTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPos;
        lerpAnchor = startPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (!textChanged)
        {
            textChanged = true;
            switch (mistakeType)
            {
                case MistakeType.None:
                    text.text = "";
                    break;
                case MistakeType.Patience:
                    text.text = "That customer was losing their patience!";
                    break;
                case MistakeType.WrongToppings:
                    text.text = "That pizza had some wrong toppings!";
                    break;
                case MistakeType.Order:
                    text.text = "The order of ingredients was wrong, remember, this is Chicago style!";
                    break;
                case MistakeType.MissingToppings:
                    text.text = "Some toppings were missing from that pizza!";
                    break;
                case MistakeType.ExtraToppings:
                    text.text = "That pizza had extra toppings!";
                    break;
                case MistakeType.UncutToppings:
                    text.text = "Some of those toppings weren't prepared right!";
                    break;
                case MistakeType.Burnt:
                    text.text = "That pizza was burnt!";
                    break;
                case MistakeType.Uncooked:
                    text.text = "That pizza wasn't fully cooked";
                    break;
                case MistakeType.Catastrophe:
                    text.text = "That pizza was a mess! Did you give them the wrong order?";
                    break;
                case MistakeType.PlateFunny:
                    text.text = "Dude, this is just a plate.";
                    break;
            }
        }

        if (fade < 1 && fadeTimer < fadeMax)
        {
            fade += Time.deltaTime;
            if (fadeQuick) //Move out quicker
            {
                fade += Time.deltaTime * 3;
                lerpTimer += Time.deltaTime * 3;
            }

            float t = lerpTimer / lerpDurration;
            t = t * t * (3f - 2f * t);
            transform.position = Vector3.Lerp(lerpAnchor, startPos + Vector3.down * 9f, t);//Move to destination in an interlopian curve
            lerpTimer += Time.deltaTime;
        }
        else if (fade >= 1 && fadeTimer < fadeMax)
        {
            lerpAnchor = transform.position;
            lerpTimer = 0;

            fadeTimer+= Time.deltaTime;
            if (fadeQuick)
            {
                fadeTimer = fadeMax;
            }
        }
        else if (fadeTimer >= fadeMax) 
        {
            if (fadeQuick) //Move out quicker
            {
                lerpTimer += Time.deltaTime * 3;
            }

            float t = lerpTimer / lerpDurration;
            t = t * t * (3f - 2f * t);
            transform.position = Vector3.Lerp(lerpAnchor, startPos + Vector3.up * 9f, t);//Move to destination in an interlopian curve
            lerpTimer += Time.deltaTime;

            if (lerpTimer >= lerpDurration)
                Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sets the mistake type
    /// </summary>
    /// <param name="type">The mistaketype to use</param>
    public void ChangeType(MistakeType type)
    {
        mistakeType = type;
        textChanged = true;
    }

    public void RemoveQuick()
    {
        fadeQuick = true;
    }
}