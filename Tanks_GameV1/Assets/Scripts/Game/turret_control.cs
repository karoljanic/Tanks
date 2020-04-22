using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class turret_control : MonoBehaviour
{
    public GameObject bulletprefab;
    public GameObject bulletprefebaleft;
    public GameObject trajectorydot;
    public Rigidbody2D tank;
    public Transform shotpoint;
    public Transform turret;
    public Transform tank_trans;
    public float force;
    public int numberofdots;

    private GameObject bullet;
    private GameObject[] trajectorydots;
    private Rigidbody2D rb;
    private Vector3 starpos;
    private Vector3 endpos;
    private Vector3 pos;
    public Vector3 forceatplayer;
    private Vector3 difference;
    private Quaternion rot;
    private bool t = false;
    private bool k = false;
    private bool facingRight = true;
    private Vector3 localScale;
    private float speed = 100;
    private float movement;
    private float rotZ;

    private void Start()
    {
        pos = shotpoint.position;
        trajectorydots = new GameObject[numberofdots];
        localScale = tank.transform.localScale;
    }

    private void Update()
    {
        movement = CrossPlatformInputManager.GetAxis("Horizontal") * speed;

        if(!k & !t)
            rot = turret.rotation;

        if (Input.mousePosition.x < 1200 & Input.mousePosition.x > 900 & Input.mousePosition.y < 700 & Input.mousePosition.y > 350)
            t = true;

        if (Input.GetMouseButton(0) & t)
        {
            k = true;
        
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            
            if(facingRight)
            {
                rotZ = Mathf.Atan2(-difference.y, -difference.x ) * Mathf.Rad2Deg;

                if (rotZ < 0)
                    rotZ = 0;
                else if (rotZ > 80)
                    rotZ = 80;
            }
            else
            {
                rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                if (rotZ > 0)
                    rotZ = 0;
                else if (rotZ < -80)
                    rotZ = -80;
            }
  
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

        }

        if (Input.GetMouseButtonDown(0) & t)
        {
            t = true;
            starpos = shotpoint.transform.position;

            for (int i = 0; i < numberofdots; i++)
                trajectorydots[i] = Instantiate(trajectorydot, shotpoint.transform);

        }
        if (Input.GetMouseButton(0) & t)
        {
            endpos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
            forceatplayer = endpos - starpos;
            if (facingRight)
            {
                if (forceatplayer.x > -1)
                    forceatplayer.x = (float)-1;
                if (forceatplayer.y > 2)
                    forceatplayer.y = (float)2;
            }
            else
            {
                if (forceatplayer.x < 1)
                    forceatplayer.x = (float)1;
                if (forceatplayer.y > 2)
                    forceatplayer.y = (float)2;
            }
            
            endpos = shotpoint.transform.position;

            for (int i = 0; i < numberofdots; i++)
                trajectorydots[i].transform.position = calculateposition(i * 0.1f);
        }
        if (Input.GetMouseButtonUp(0) & t)
        {
            t = false;
            if(facingRight)
                bullet = Instantiate(bulletprefab, shotpoint.position, Quaternion.identity);
            else
                bullet = Instantiate(bulletprefebaleft, shotpoint.position, Quaternion.identity);

            rb = bullet.GetComponent<Rigidbody2D>();
            rb.gravityScale = 1;
            rb.velocity = new Vector2(-forceatplayer.x * force, -forceatplayer.y * force);

            for (int i = 0; i < numberofdots; i++)
                Destroy(trajectorydots[i]);

            turret.rotation = rot;
        }

    }

    private void LateUpdate()
    {
        check_where_to_face();
    }

    private Vector2 calculateposition(float time)
    {
        return new Vector2(endpos.x, endpos.y) + new Vector2(-forceatplayer.x * force, -forceatplayer.y * force) * time + 0.5f * Physics2D.gravity * time * time;
    }

    void check_where_to_face()
    {
        if (movement > 0)
            facingRight = true;
        else if (movement < 0)
            facingRight = false;
    }

}