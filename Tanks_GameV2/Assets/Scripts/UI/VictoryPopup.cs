using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryPopup: MonoBehaviour
{

    [SerializeField]private GameObject _View;

    private static VictoryPopup _Instance;

    public static VictoryPopup Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<VictoryPopup>();
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
