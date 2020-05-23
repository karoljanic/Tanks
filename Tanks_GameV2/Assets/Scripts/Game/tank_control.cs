using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class tank_control : MonoBehaviour
{

    private Rigidbody2D rb;
    private float Speed = 100;
    private float Movement;

    [SerializeField]
    private bool facingRight = true;
    Vector3 localScale;

    private void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Movement = CrossPlatformInputManager.GetAxis("Horizontal") * Speed;
    }

    private void LateUpdate()
    {
        check_where_to_face();
    }

    void check_where_to_face()
    {
        if (Movement > 0)
            facingRight = true;
        else if (Movement < 0)
            facingRight = false;

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }

 
}