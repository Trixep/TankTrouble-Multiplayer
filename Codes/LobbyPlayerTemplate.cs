using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;
using System.ComponentModel;
using Unity.Services.Authentication;
using Unity.VisualScripting;

public class LobbyPlayerTemplate : MonoBehaviour
{
    public static LobbyPlayerTemplate Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI readyButtonText;
    [SerializeField] private Image towerSkin;
    [SerializeField] private Image bodySkin;
    [SerializeField] private Image gunSkin;
    [SerializeField] private Image gunConnectorSkin;
    [SerializeField] private Image track1Skin;
    [SerializeField] private Image track2Skin;
    [SerializeField] private Image lightSkin;
    [SerializeField] private Button kickPlayerButton;
    [SerializeField] private Button readyButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button gameSettingsButton;

    private Player player;

    private void Awake()
    {
        Instance = this;
        kickPlayerButton.onClick.AddListener(KickPlayer);
        readyButton.onClick.AddListener(Ready);
    }

    public void SetKickPlayerButtonVisible(bool visible)
    {
        kickPlayerButton.gameObject.SetActive(visible);
    }
    
    public void SetReadyButtonInteractable(bool interactable)
    {
        readyButton.interactable = interactable;
    }

    public void SetStartButtonVisible(bool visible)
    {
        startButton.gameObject.SetActive(visible);
    }

    public void SetStartButtonInteractable(bool interactable)
    {
        startButton.interactable = interactable;
    }

    public void SetGameSettingsInteractable(bool interactable)
    {
        gameSettingsButton.interactable = interactable;
    }

    public void UpdatePlayer(Player player)
    {
        this.player = player;
        playerNameText.text = player.Data[LobbyOperator.KEY_PLAYER_NAME].Value;
        readyButtonText.text = player.Data[LobbyOperator.KEY_READY].Value;

        string[] skinIdsString = player.Data[LobbyOperator.KEY_PLAYER_CHARACTER].Value.Split(';');
        int[] skinIds = new int[4];
        for (int i = 0; i < skinIdsString.Length; i++)
        {
            skinIds[i] = int.Parse(skinIdsString[i]);
        }

        towerSkin.sprite = skinSelector.Instance.GetSkin("tower", skinIds[0]);
        gunSkin.sprite = skinSelector.Instance.GetSkin("gun", skinIds[0]);
        gunConnectorSkin.sprite = skinSelector.Instance.GetSkin("gunConnector", skinIds[0]);
        bodySkin.sprite = skinSelector.Instance.GetSkin("body", skinIds[1]);
        track1Skin.sprite = skinSelector.Instance.GetSkin("track1", skinIds[2]);
        track2Skin.sprite = skinSelector.Instance.GetSkin("track2", skinIds[2]);
        lightSkin.color = skinSelector.Instance.GetColor(skinIds[3]);

        towerSkin.transform.localScale = skinSelector.Instance.GetTransform("towerScale", skinIds[0]);
        towerSkin.transform.localPosition = skinSelector.Instance.GetTransform("towerPos", skinIds[0]);
        gunSkin.transform.localPosition = skinSelector.Instance.GetTransform("gunPos", skinIds[0]);
        gunConnectorSkin.transform.localPosition = skinSelector.Instance.GetTransform("gunConnectorPos", skinIds[0]);
        bodySkin.transform.localScale = skinSelector.Instance.GetTransform("bodyScale", skinIds[1]);
        track1Skin.transform.localPosition = skinSelector.Instance.GetTransform("track1Pos", skinIds[2]);
    }

    private void KickPlayer()
    {
        if (player != null)
        {
            LobbyOperator.Instance.KickPlayer(player.Id);
        }
    }

    private void Ready()
    {
        if (readyButtonText.text == "Ready")
        {
            LobbyOperator.Instance.UpdatePlayerReady("Unready");
        }
        else
        {
            LobbyOperator.Instance.UpdatePlayerReady("Ready");
        }
    }
}
