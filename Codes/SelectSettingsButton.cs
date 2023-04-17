using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSettingsButton : MonoBehaviour
{
    public static SelectSettingsButton Instance { get; private set; }

    [SerializeField] private Button gameSettingsButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private GameObject gameSettingsWindow;
    [SerializeField] private GameObject optionsWindow;

    private void Awake()
    {
        Instance = this;
    }

    public void SettingsButtonDisable()
    {
        gameSettingsButton.interactable = false;
        gameSettingsWindow.SetActive(false);
        optionsWindow.SetActive(true);
    }

    public bool InGameSettings()
    {
        return gameSettingsButton.interactable ? true : false;
    }
}
