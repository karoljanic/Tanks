using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : Photon.PunBehaviour
{
    [SerializeField]private BlinkEffect _RoomNameEffect;
    [SerializeField]private MainMenuFlow _MainMenuFlow;
    [SerializeField]
    private Text _roomName;
    private Text RoomName
    {
        get { return _roomName; }
    }

    public void OnClick_CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
        if (PhotonNetwork.CreateRoom(RoomName.text, roomOptions, TypedLobby.Default))
        {
            print("create room successfully sent. :)");
        }
        else
        {
            print("create room failed to send. :(");
        }
    }

    override public void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        print("create room failed: " + codeAndMessage[1] + ":(");
        _RoomNameEffect.Blink();
    }

    override public void OnCreatedRoom()
    {
        print("Room created successfully! :)");
        _MainMenuFlow.MakeRoomDone();
    }
}
