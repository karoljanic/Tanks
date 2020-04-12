
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{
    [SerializeField]
    private Text _roomName;
    private Text RoomName
    {
        get { return _roomName; }
    }

    public void OnClick_CreateRoom()
    {
        RoomOptions roomoptions = new RoomOptions { isVisible = true, isOpen = true, MaxPlayers = 2 };

        if (PhotonNetwork.CreateRoom(RoomName.text, roomoptions, TypedLobby.Default))
        {
            print("create new room successfully sent.");
        }
        else
        {
            print("create room failed to send.");
        }
    }

    private void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        print("create room failed " + codeAndMessage[1]);
    }

    private void OnCreatedRoom()
    {
        print("Room created successfully!");
    }

    /*

    public void OnClick_CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 4 };

        if (PhotonNetwork.CreateRoom(RoomName.text, roomOptions, TypedLobby.Default))
        {
            print("create room successfully sent.");
        }
        else
        {
            print("create room failed to send");
        }
    }

    private void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        print("create room failed: " + codeAndMessage[1]);
    }

    */
}
