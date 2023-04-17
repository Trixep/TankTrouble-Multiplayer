using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class LobbyUpdate : MonoBehaviour
{
    public static LobbyUpdate Instance { get; private set; }

    [SerializeField] private Button updateLobbyButton;
    [SerializeField] private TMP_Text privateButtonText;
    [SerializeField] private TMP_InputField lobbyNameText;
    [SerializeField] private TMP_InputField finishScoreText;

    private string lobbyName;
    private bool isPrivate;
    private string finishScore;

    private string lobbyUpdateName;
    private string lobbyUpdateFinishScore;

    private void Awake()
    {
        Instance = this;

        updateLobbyButton.onClick.AddListener(() =>
        {
            if (SelectSettingsButton.Instance.InGameSettings())
            {
                LobbyOperator.Instance.UpdateLobby(
                GetUpdateLobbyName(),
                GetUpdateLobbyFinishScore(),
                isPrivate);
            }
        });
    }

    public void LobbyName()
    {
        lobbyName = lobbyNameText.text;
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

    private void Start()
    {
        VisibilityStart();
    }

    public void VisibilityStart()
    {
        bool isPrivateStart = LobbyCreate.Instance.GetLobbyVisibility();
        if (isPrivateStart == true)
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

    public string GetUpdateLobbyName()
    {
        if (lobbyNameText.text != "")
        {
            lobbyUpdateName = lobbyNameText.text;
        }
        else
        {
            lobbyName = LobbyCreate.Instance.GetLobbyName();
        }

        return lobbyUpdateName;
    }

    public string GetUpdateLobbyFinishScore()
    {
        if (finishScoreText.text != "")
        {
            lobbyUpdateFinishScore = finishScoreText.text;
        }
        else
        {
            lobbyUpdateFinishScore = LobbyCreate.Instance.GetLobbyFinishScore();
        }

        return lobbyUpdateFinishScore; 
    }
}
