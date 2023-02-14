using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nameSaver : MonoBehaviour
{
    public TMPro.TMP_InputField playerName;

    private void Awake()
    {
        string username = PlayerPrefs.GetString("username");
        playerName.text = username;
    }

    public void setUsername()
    {
        PlayerPrefs.SetString("username", playerName.text);
        PlayerPrefs.Save();
    }
}
