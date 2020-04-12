﻿using UnityEngine;


public class LobbyNetwork : MonoBehaviour
{
    public bool t = false;
    
    void Start()
    {
        t = true;
        print("Connecting to server..");
        PhotonNetwork.ConnectUsingSettings("0.0.0");
    }

    private void OnConnectedToMaster()
    {
        print("Connectes to master!");
        PhotonNetwork.playerName = PlayerNetwork.Instance.PlayerName;
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    private void OnJoinedLobby()
    {
        print("Joined lobby!");

    }

}
