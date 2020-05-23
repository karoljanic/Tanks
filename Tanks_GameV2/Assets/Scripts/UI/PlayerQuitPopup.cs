using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQuitPopup: MonoBehaviour
{

    [SerializeField]private GameObject _View;

    private static PlayerQuitPopup _Instance;

    public static PlayerQuitPopup Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<PlayerQuitPopup>();
            }
            return _Instance;
        }
    }

    public void Show()
    {
        _View.SetActive(true);
    }

    public void Close()
    {
        _View.SetActive(false);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(1);
    }

    public bool IsVisible { get { return _View.activeInHierarchy; } }
}
