using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class privateButton : MonoBehaviour
{
    public TMPro.TMP_Text buttonText;

    public void ChangeVisibility()
    {
        if (buttonText.text == "Public")
        {
            buttonText.text = "Private";
        }
        else
        {
            buttonText.text = "Public";
        }
    }
}
