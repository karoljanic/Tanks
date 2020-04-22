using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovementWheel : MonoBehaviour
{
    public float Speed = 1500;
    public WheelJoint2D Wheel;
    public JointMotor2D Motor;

    private PhotonView PhotonView;

    private float Movement;

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
        Wheel = GetComponent<WheelJoint2D>();
    }

    void Update()
    {
        if (PhotonView.isMine)
            CheckInput();
    }

    void CheckInput()
    {
        if (CrossPlatformInputManager.GetButtonDown("Move"))
            SetMovement();
        if (CrossPlatformInputManager.GetButtonUp("Move"))
            Movement = 0; 

        Motor = new JointMotor2D { motorSpeed = Movement, maxMotorTorque = 10000 };
        Wheel.motor = Motor;
    }

    void SetMovement()
    {
        if (CrossPlatformInputManager.GetAxis("Horizontal") < 0)
            Movement = -Speed;
        else
            Movement = Speed;
    }

    void FixedUpdate()
    {
        
    }

}
