using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentRoomCanvas : MonoBehaviour
{
    [SerializeField]private UnityEngine.UI.Button _PlayButton;
    [SerializeField]private UnityEngine.UI.Button _PlayButton2;

    public void OnClickStartSync()
    {
        if (!PhotonNetwork.isMasterClient)
            return;

        PhotonNetwork.LoadLevel(2);
    }

    public void OnClickStartDelayed()
    {
        if (!PhotonNetwork.isMasterClient)
            return;

        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.room.IsVisible = false;
        PhotonNetwork.LoadLevel(2);
    }

    private void OnEnable()
    {
        PlayerListChanged(PhotonNetwork.playerList.Length);
    }


    public void PlayerListChanged(int playersCount)
    {
        bool showPlayButton = playersCount == 2 && PhotonNetwork.isMasterClient;
        _PlayButton.interactable = showPlayButton;
        _PlayButton2.interactable = showPlayButton;
    }
}
