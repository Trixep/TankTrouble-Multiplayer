using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OnAppQuit : MonoBehaviour
{
    //private void OnApplicationQuit()
    //{
    //    LobbyOperator.Instance.LeaveLobby();
    //    Debug.Log("game closed");
    //}

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Debug.Log("In focus");
        }
    }
}
