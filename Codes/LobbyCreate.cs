using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyCreate : MonoBehaviour
{
    public static LobbyCreate Instance { get; private set; }

    [SerializeField] private Button createButton;
    [SerializeField] private TMP_Text privateButtonText;
    [SerializeField] private TMP_InputField lobbyNameText;
    [SerializeField] private TMP_InputField maxPlayersText;
    [SerializeField] private TMP_InputField finishScoreText;
    [SerializeField] private GameObject fillInWindow;
    [SerializeField] private GameObject lobbyWindow;

    private string lobbyName;
    private int maxPlayers;
    private bool isPrivate;
    private string finishScore;

    private void Awake()
    {
        Instance = this;

        createButton.onClick.AddListener(() =>
        {
            if (lobbyNameText.text == "" || maxPlayersText.text == "" || finishScoreText.text == "")
            {
                fillInWindow.SetActive(true);
            }
            else
            {
                LobbyOperator.Instance.CreateLobby(
                    lobbyName,
                    maxPlayers,
                    isPrivate,
                    finishScore);
                lobbyWindow.SetActive(true);
                gameObject.SetActive(false);
            }
        });
    }

    public void LobbyName()
    {
        lobbyName = lobbyNameText.text;
    }

    public void MaxPlayers()
    {
        maxPlayers = int.Parse(maxPlayersText.text);
    }

    public void FinishScore()
    {
        finishScore = finishScoreText.text;
    }

    public void ChangeVisibility()
    {
        if (privateButtonText.text == "Public")
        {
            privateButtonText.text = "Private";
            isPrivate = true;
        }
        else
        {
            privateButtonText.text = "Public";
            isPrivate = false;
        }
    }
}
