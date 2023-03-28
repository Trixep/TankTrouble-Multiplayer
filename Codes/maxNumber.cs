using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class maxNumber : MonoBehaviour
{
    [SerializeField] int min;
    [SerializeField] int max;
    [SerializeField] TMPro.TMP_InputField limitNumber;
    int valueInt;

    void Start()
    {
        limitNumber.onValueChanged.AddListener(TextMeshUpdated);
    }

    private void TextMeshUpdated(string text)
    {
        ChangeValue();
    }

    private void ChangeValue()
    {
        string value = limitNumber.text;
        int.TryParse(value, out valueInt);
        if (valueInt < min)
        {
            limitNumber.text = "";
        }
        if (valueInt > max)
        {
            valueInt = max;
            value = valueInt.ToString();
            limitNumber.text = value;
        }
    }
}
