using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuFlow : MonoBehaviour
{
    [Header("Main menu")]
    [SerializeField]private GameObject _MainMenu;
    [SerializeField]private UnityEngine.UI.InputField _NameField;
    [SerializeField]private BlinkEffect _NameFieldEffect;


    [Header("Lobby")]
    [SerializeField]private GameObject _LobbyView;
    [SerializeField]private GameObject _CurrentRoomView;
    [SerializeField]private CreateRoom _CreateRoom;
    [SerializeField]private UnityEngine.UI.InputField _RoomNameField;
    [SerializeField]private BlinkEffect _RoomNameFieldEffect;

    public static int AvatarID;
    public static int TankID;

    public void CheckNameAndSwitchView(GameObject onSuccessCanvas)
    {
        if (string.IsNullOrEmpty(_NameField.text))
        {
            _NameFieldEffect.Blink();
        }
        else
        {
            var playerData = new ExitGames.Client.Photon.Hashtable();
            playerData.Add("AvatarID", AvatarID);
            playerData.Add("TankID", TankID);

            PhotonNetwork.player.SetCustomProperties(playerData);
            PhotonNetwork.playerName = _NameField.text;
            PhotonNetwork.player.NickName = _NameField.text;
            _MainMenu.SetActive(false);
            onSuccessCanvas.SetActive(true);
        }
    }

    public void MakeRoomClickHandler()
    {
        if (string.IsNullOrEmpty(_RoomNameField.text))
        {
            _RoomNameFieldEffect.Blink();
        }
        else
        {
            _CreateRoom.OnClick_CreateRoom();
        }
    }

    public void MakeRoomDone()
    {
        _LobbyView.SetActive(false);
        _CurrentRoomView.SetActive(true);
        _MainMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void OnEnable()
    {
        var playerName = PhotonNetwork.player.NickName;
        if (!string.IsNullOrEmpty(playerName))
        {
            _NameField.text = playerName;
        }
    }

}
