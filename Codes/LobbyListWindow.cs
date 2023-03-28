using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyListWindow : MonoBehaviour
{
    public static LobbyListWindow Instance { get; private set; }



    [SerializeField] private Transform lobbySingleTemplate;
    [SerializeField] private Transform container;
    [SerializeField] private Button refreshButton;


    private void Awake()
    {
        Instance = this;

        lobbySingleTemplate.gameObject.SetActive(false);

        refreshButton.onClick.AddListener(RefreshButtonClick);
    }

    private void Start()
    {
        LobbyOperator.Instance.OnLobbyListChanged += LobbyManager_OnLobbyListChanged;
        LobbyOperator.Instance.OnLeftLobby += LobbyManager_OnLeftLobby;
        LobbyOperator.Instance.OnKickedFromLobby += LobbyManager_OnKickedFromLobby;
    }

    private void LobbyManager_OnKickedFromLobby(object sender, LobbyOperator.LobbyEventArgs e)
    {
        Show();
        LobbyOperator.Instance.RefreshLobbyList();
    }

    private void LobbyManager_OnLeftLobby(object sender, EventArgs e)
    {
        Show();
        LobbyOperator.Instance.RefreshLobbyList();
    }

    private void LobbyManager_OnLobbyListChanged(object sender, LobbyOperator.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        foreach (Transform child in container)
        {
            if (child == lobbySingleTemplate) continue;

            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbyList)
        {
            Transform lobbySingleTransform = Instantiate(lobbySingleTemplate, container);
            lobbySingleTransform.gameObject.SetActive(true);
            LobbyListTemplate lobbyListSingleUI = lobbySingleTransform.GetComponent<LobbyListTemplate>();
            lobbyListSingleUI.UpdateLobby(lobby);
        }
    }

    private void RefreshButtonClick()
    {
        LobbyOperator.Instance.RefreshLobbyList();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
