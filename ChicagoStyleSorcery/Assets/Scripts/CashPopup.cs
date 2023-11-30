using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CashPopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro moneyText;
    [SerializeField]
    private Image bill;

    public float money;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = $"${money - money % .01}";
        transform.position += new Vector3(0, Time.deltaTime / 2, 0);

        //make slowly fade
        if (moneyText.alpha > 0)
        {
            moneyText.alpha -= Time.deltaTime / 1.5f;
            bill.color -= new Color(0, 0, 0, Time.deltaTime / 1.5f);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
