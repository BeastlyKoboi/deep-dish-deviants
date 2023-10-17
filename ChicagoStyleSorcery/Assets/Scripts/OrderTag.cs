using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderTag : MonoBehaviour
{
    [SerializeField]
    TextMeshPro orderNum;
    [SerializeField]
    int id;

    [SerializeField]
    List<Icon> iconList;
    [SerializeField]
    List<TextMeshPro> textList;

    public bool opened;

    private Vector3 hiddenPoint;
    private Vector3 lerpAnchor = Vector3.zero;
    private float lerpDurration = .5f;
    private float lerpTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        orderNum.text = "Order " + id;
        EmptyTag();
        opened = false;
        lerpAnchor = transform.position;
        hiddenPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (lerpTimer < lerpDurration)
        {
            float t = lerpTimer / lerpDurration;
            t = t * t * (3f - 2f * t);
            if (opened)
                transform.position = Vector3.Lerp(lerpAnchor, hiddenPoint + new Vector3(4.1f, 0, 0), t);//Move to destination in an interlopian curve, 3.4 is approximate width of the tag in world space
            else
                transform.position = Vector3.Lerp(lerpAnchor, hiddenPoint, t);//Move to destination in an interlopian curve, 3.4 is approximate width of the tag in world space
            lerpTimer += Time.deltaTime;
        }
    }

    public void FillTag(List<FoodId> order)
    {
        iconList[0].SetIconType(FoodId.plate);
        textList[0].text = "Plate";
        for (int i = 0; i < order.Count; i++)
        {
            switch(order[i])
            {
                case FoodId.plate:
                    iconList[i+1].SetIconType(FoodId.plate, 1);
                    textList[i+1].text = "Plate";
                    break;
                case FoodId.dough:
                    iconList[i+1].SetIconType(FoodId.dough, 1);
                    textList[i+1].text = "Dough";
                    break;
                case FoodId.cheese:
                    iconList[i+1].SetIconType(FoodId.cheese, 1);
                    textList[i+1].text = "Cheese";
                    break;
                case FoodId.sauce:
                    iconList[i+1].SetIconType(FoodId.sauce, 1);
                    textList[i+1].text = "Sauce";
                    break;
                default:
                    break;
            }
        }
    }

    public void EmptyTag()
    {
        foreach (Icon i in iconList)
            i.Invisible();
        foreach (TextMeshPro t in textList)
            t.text = "";
    }

    /// <summary>
    /// toggle whether this tag should show itself
    /// </summary>
    public void toggleOpended()
    {
        opened = !opened;
        lerpAnchor = transform.position;
        lerpTimer = 0;
        lerpDurration = .5f;
    }

    /// <summary>
    /// Toggles tag quickly, only if already open
    /// </summary>
    public void closeQuick()
    {
        if (opened)
        {
            opened = !opened;
            lerpAnchor = transform.position;
            lerpTimer = 0;
            lerpDurration = .15f;
        }
    }
}
