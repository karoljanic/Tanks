using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class tank_control : MonoBehaviour
{

    private Rigidbody2D rb;
    private float speed = 100;
    private float movement;
    private float getRifleUp;
    public Transform rifle; 
  
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
        movement = CrossPlatformInputManager.GetAxis("Horizontal") * speed;

        CheckIfRifleUp();
    }

    private void LateUpdate()
    {
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
    
    void CheckIfRifleUp()
    {
        if (CrossPlatformInputManager.GetButton("button_fire"))
            rifle.rotation = Quaternion.Euler(0, 0, 90 * localScale.x);
        else
            rifle.rotation = Quaternion.Euler(0, 0, 23);
    }
}
