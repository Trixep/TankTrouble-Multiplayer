using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyWindow : MonoBehaviour
{
    public static LobbyWindow Instance { get; private set; }


    [SerializeField] private Transform playerSingleTemplate;
    [SerializeField] private Transform container;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private TextMeshProUGUI isPrivateText;
    [SerializeField] private TextMeshProUGUI lobbyCodeText;
    [SerializeField] private Button leaveLobbyButton;

    private void Awake()
    {
        Instance = this;
        playerSingleTemplate.gameObject.SetActive(false);

        leaveLobbyButton.onClick.AddListener(() => {
            LobbyOperator.Instance.LeaveLobby();
        });
    }

    private void Start()
    {
        LobbyOperator.Instance.OnJoinedLobby += UpdateLobby_Event;
        LobbyOperator.Instance.OnJoinedLobbyUpdate += UpdateLobby_Event;
        LobbyOperator.Instance.OnLobbyGameModeChanged += UpdateLobby_Event;
        LobbyOperator.Instance.OnLeftLobby += LobbyManager_OnLeftLobby;
        LobbyOperator.Instance.OnKickedFromLobby += LobbyManager_OnLeftLobby;
    }

    private void LobbyManager_OnLeftLobby(object sender, System.EventArgs e)
    {
        ClearLobby();
        Hide();
    }

    private void UpdateLobby_Event(object sender, LobbyOperator.LobbyEventArgs e)
    {
        UpdateLobby();
    }

    private void UpdateLobby()
    {
        UpdateLobby(LobbyOperator.Instance.GetJoinedLobby());
    }

    private void UpdateLobby(Lobby lobby)
    {
        ClearLobby();

        foreach (Player player in lobby.Players)
        {
            Transform playerSingleTransform = Instantiate(playerSingleTemplate, container);
            playerSingleTransform.gameObject.SetActive(true);
            LobbyPlayerTemplate lobbyPlayerSingleUI = playerSingleTransform.GetComponent<LobbyPlayerTemplate>();

            lobbyPlayerSingleUI.SetKickPlayerButtonVisible(LobbyOperator.Instance.IsLobbyHost() && player.Id != AuthenticationService.Instance.PlayerId);

            lobbyPlayerSingleUI.UpdatePlayer(player);
        }

        lobbyNameText.text = lobby.Name;
        playerCountText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;
        isPrivateText.text = lobby.IsPrivate ? "Private" : "Public";
        lobbyCodeText.text = lobby.LobbyCode;
    }

    private void ClearLobby()
    {
        foreach (Transform child in container)
        {
            if (child == playerSingleTemplate) continue;
            Destroy(child.gameObject);
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}