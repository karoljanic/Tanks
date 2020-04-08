using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class motor_control : MonoBehaviour
{
    public float speed = 1500;

    public WheelJoint2D wheel;
    public JointMotor2D motor;

    private float movement;

    private void Start()
    {
        wheel = GetComponent<WheelJoint2D>();
    }

    void Update()
    {
        movement = CrossPlatformInputManager.GetAxis("Horizontal") * speed;
    }

   void FixedUpdate()
    {
        JointMotor2D motor = new JointMotor2D { motorSpeed = movement, maxMotorTorque = 10000 };
        
        wheel.motor = motor;  
    }

}
