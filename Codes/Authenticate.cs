using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Authenticate : MonoBehaviour
{
    [SerializeField] private Button authenticateButton;
    [SerializeField] private TMPro.TMP_InputField playerName;
    public GameObject nullWindow;
    public GameObject selectGames;
    public GameObject startWindow;

    private void Awake()
    {
        authenticateButton.onClick.AddListener(() =>
        {
            if (playerName.text != "")
            {
                LobbyOperator.Instance.Authenticate(playerName.text);
                selectGames.SetActive(true);
                startWindow.SetActive(false);
            }
            else
            {
                nullWindow.SetActive(true);
            }
        });
    }
}