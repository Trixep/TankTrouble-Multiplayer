using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;
public class LobbyPlayerTemplate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Button kickPlayerButton;


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

    public void UpdatePlayer(Player player)
    {
        this.player = player;
        playerNameText.text = player.Data[LobbyOperator.KEY_PLAYER_NAME].Value;
    }

    private void KickPlayer()
    {
        if (player != null)
        {
            LobbyOperator.Instance.KickPlayer(player.Id);
        }
    }
}
