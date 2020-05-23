using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyNetwork : MonoBehaviour
{

    // Use this for initialization
    private void OnEnable()
    {
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.automaticallySyncScene = false;
            PhotonNetwork.JoinLobby(TypedLobby.Default);
        }
        else
        {
            if (!PhotonNetwork.connected)
            {
                print("Connecting to server...");
                PhotonNetwork.ConnectUsingSettings("0.0.0");
            }
        }
    }

    private void OnConnectedToMaster()
    {
        print("Connected to master.");
        PhotonNetwork.automaticallySyncScene = false;
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    private void OnJoinedLobby()
    {
        print("Joined lobby!");

            if (!PhotonNetwork.inRoom)
                MainCanvasManager.Instance.LobbyCanvas.transform.SetAsLastSibling();
    }
}
