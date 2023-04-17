using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;
using static LobbyManager;
using System.ComponentModel;

public class LobbyPlayerTemplate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Image towerSkin;
    [SerializeField] private Image bodySkin;
    [SerializeField] private Image gunSkin;
    [SerializeField] private Image gunConnectorSkin;
    [SerializeField] private Image track1Skin;
    [SerializeField] private Image track2Skin;
    [SerializeField] private Image lightSkin;
    [SerializeField] private Button kickPlayerButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button gameSettingsButton;

    private Player player;

    private void Awake()
    {
        kickPlayerButton.onClick.AddListener(KickPlayer);
    }

    public void SetKickPlayerButtonVisible(bool visible)
    {
        kickPlayerButton.gameObject.SetActive(visible);
    }

    public void SetStartButtonVisible(bool visible)
    {
        startButton.gameObject.SetActive(visible);
    }

    public void SetGameSettingsInteractable(bool interactable)
    {
        gameSettingsButton.interactable = interactable;
    }

    public void UpdatePlayer(Player player)
    {
        this.player = player;
        playerNameText.text = player.Data[LobbyOperator.KEY_PLAYER_NAME].Value;

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
}
