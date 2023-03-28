using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using TMPro;
using System;
using static LobbyOperator;
using System.Security.Cryptography;

public class LobbyOperator : MonoBehaviour
{
    public static LobbyOperator Instance { get; private set; }

    private Lobby hostlobby;
    private Lobby joinedlobby;
    private float heartbeatTimer;
    private float lobbyPollTimer;
    private float refreshLobbyListTimer = 5f;
    private string playerName;
    public TMP_InputField lobbyCode;
    public GameObject codeNullWindow;
    public GameObject selectGames;
    public GameObject privateWindow;
    public GameObject lobbyWindow;
    public GameObject findWindow;
    public GameObject errorJoinLobbyWindow;

    public const string KEY_PLAYER_NAME = "PlayerName";
    public const string KEY_PLAYER_CHARACTER = "Character";
    public const string KEY_GAME_MODE = "GameMode";
    public const string KEY_FINISH_SCORE = "FinishScore";

    public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;
    public class OnLobbyListChangedEventArgs : EventArgs
    {
        public List<Lobby> lobbyList;
    }

    public event EventHandler OnLeftLobby;
    public event EventHandler<LobbyEventArgs> OnJoinedLobby;
    public event EventHandler<LobbyEventArgs> OnJoinedLobbyUpdate;
    public event EventHandler<LobbyEventArgs> OnKickedFromLobby;
    public event EventHandler<LobbyEventArgs> OnLobbyGameModeChanged;

    public class LobbyEventArgs : EventArgs
    {
        public Lobby lobby;
    }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
        HandleLobbyPolling();
    }

    public async void Authenticate(string playerName)
    {
        this.playerName = playerName;
        InitializationOptions initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile(playerName);

        await UnityServices.InitializeAsync(initializationOptions);

        AuthenticationService.Instance.SignedIn += () => {
            // do nothing
            Debug.Log("Signed in! " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private async void HandleLobbyHeartbeat()
    {
        if (IsLobbyHost())
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 15f;
                heartbeatTimer = heartbeatTimerMax;

                Debug.Log("Heartbeat");
                await LobbyService.Instance.SendHeartbeatPingAsync(joinedlobby.Id);
            }
        }
    }

    private async void HandleLobbyPolling()
    {
        if (joinedlobby != null)
        {
            lobbyPollTimer -= Time.deltaTime;
            if (lobbyPollTimer < 0f)
            {
                float lobbyPollTimerMax = 1.1f;
                lobbyPollTimer = lobbyPollTimerMax;

                joinedlobby = await LobbyService.Instance.GetLobbyAsync(joinedlobby.Id);

                OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = joinedlobby });

                if (!IsPlayerInLobby())
                {
                    Debug.Log("Kicked from Lobby!");

                    OnKickedFromLobby?.Invoke(this, new LobbyEventArgs { lobby = joinedlobby });

                    joinedlobby = null;
                }
            }
        }
    }

    public Lobby GetJoinedLobby()
    {
        return joinedlobby;
    }

    public bool IsLobbyHost()
    {
        return joinedlobby != null && joinedlobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    private bool IsPlayerInLobby()
    {
        if (joinedlobby != null && joinedlobby.Players != null)
        {
            foreach (Player player in joinedlobby.Players)
            {
                if (player.Id == AuthenticationService.Instance.PlayerId)
                {
                    // This player is in this lobby
                    return true;
                }
            }
        }
        return false;
    }

    public async void CreateLobby(string lobbyName, int maxPlayers, bool isPrivate, /*GameMode gameMode,*/ string finishScore)
    {
        try
        {
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = isPrivate,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, "FFA") },
                    { KEY_FINISH_SCORE, new DataObject(DataObject.VisibilityOptions.Public, finishScore) }
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);

            joinedlobby = lobby;

            OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });

            Debug.Log("Lobby created! " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode + " " + lobby.IsPrivate);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void RefreshLobbyList()
    {
        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 69;

            options.Filters = new List<QueryFilter> 
            {
                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0")
            };

            options.Order = new List<QueryOrder> 
            {
                new QueryOrder(
                    asc: false,
                    field: QueryOrder.FieldOptions.AvailableSlots)
            };

            QueryResponse lobbyListQueryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            OnLobbyListChanged?.Invoke(this, new OnLobbyListChangedEventArgs { lobbyList = lobbyListQueryResponse.Results });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 25,
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
                },
                Order = new List<QueryOrder>
                {
                    new QueryOrder(false, QueryOrder.FieldOptions.AvailableSlots)
                }
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies found: " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Data["GameMode"].Value);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public void JoinLobbyByCode()
    {

        JoinLobbyByCode(lobbyCode.text);
    }
    public async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer()
            };

            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
            joinedlobby = lobby;

            OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });

            Debug.Log("Joined lobby with code, " + lobbyCode);

            selectGames.SetActive(false);
            privateWindow.SetActive(false);
            lobbyWindow.SetActive(true);
        }
        catch
        {
            codeNullWindow.SetActive(true);
        }
    }

    public async void JoinLobby(Lobby lobby)
    {
        try
        {
            Player player = GetPlayer();

            joinedlobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id, new JoinLobbyByIdOptions
            {
                Player = player
            });

            OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });

            findWindow.SetActive(false);
            lobbyWindow.SetActive(true);
        }
        catch
        {
            errorJoinLobbyWindow.SetActive(true);
        }
    }

    private Player GetPlayer()
    {
        return new Player(AuthenticationService.Instance.PlayerId, null, new Dictionary<string, PlayerDataObject> {
            { KEY_PLAYER_NAME, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) }
        });
    }

    public void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Players in lobby " + lobby.Name + " " + lobby.Data["GameMode"].Value);
        foreach (Player player in lobby.Players)
        {
            Debug.Log(player.Id + " " + player.Data["PlayerName"].Value);
        }
    }

    public async void KickPlayer(string playerId)
    {
        if (IsLobbyHost())
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedlobby.Id, playerId);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    private async void UpdateLobbyGameMode(string gameMode)
    {
        try
        {
            hostlobby = await Lobbies.Instance.UpdateLobbyAsync(hostlobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, gameMode) }
                }
            });
            joinedlobby = hostlobby;

            PrintPlayers(hostlobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void LeaveLobby()
    {
        if (joinedlobby != null)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedlobby.Id, AuthenticationService.Instance.PlayerId);

                joinedlobby = null;

                OnLeftLobby?.Invoke(this, EventArgs.Empty);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public async void MigrateLobbyHost()
    {
        try
        {
            hostlobby = await Lobbies.Instance.UpdateLobbyAsync(hostlobby.Id, new UpdateLobbyOptions
            {
                HostId = joinedlobby.Players[1].Id
            });
            joinedlobby = hostlobby;

            PrintPlayers(hostlobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
