using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipInfo : MonoBehaviour
{
    RectTransform rectTransform;

    [SerializeField] private TextMeshProUGUI mainTextUI;

    public string mainText = "";

    public float x;
    public float y;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Load(string savedData)
    {
        JsonUtility.FromJsonOverwrite(savedData, this);

        mainTextUI.text = mainText;
        rectTransform.localPosition = new Vector3(x, y, 0);
    }
}
