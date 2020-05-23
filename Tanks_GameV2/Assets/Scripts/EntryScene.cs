using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryScene : MonoBehaviour
{
    void Start()
    {
        PhotonNetwork.LoadLevel(1);
    }

}
