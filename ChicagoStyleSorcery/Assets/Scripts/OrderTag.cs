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

    // Start is called before the first frame update
    void Start()
    {
        orderNum.text = "Order " + id;
        EmptyTag();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
