using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovementTank : Photon.MonoBehaviour
{
    private PhotonView PhotonView;
    public float movement;

    [SerializeField]
    private bool facingRight = true;
    Vector3 localScale;

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
        localScale = transform.localScale;
    }

    void Update()
    {
        if (PhotonView.isMine)
            CheckInput();
    }

    private void CheckInput()
    {
        movement = CrossPlatformInputManager.GetAxis("Horizontal");
        check_where_to_face();
    }

    void check_where_to_face()
    {
        if (movement > 0)
            facingRight = true;
        else if (movement < 0)
            facingRight = false;

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }


}