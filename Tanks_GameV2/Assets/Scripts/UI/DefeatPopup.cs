using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatPopup : MonoBehaviour
{

    [SerializeField]private GameObject _View;

    private static DefeatPopup _Instance;

    public static DefeatPopup Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<DefeatPopup>();
            }
            return _Instance;
        }
    }

    public void Show()
    {
        Debug.Log("SHOW");
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
