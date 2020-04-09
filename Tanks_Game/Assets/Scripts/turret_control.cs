using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret_control : MonoBehaviour
{
    public Transform shotpoint;
    public GameObject bulletprefab;
    public float force;
    public GameObject trajectorydot;
    public int numberofdots;

    private GameObject bullet;
    private Vector3 starpos;
    private Vector3 endpos;
    private Vector3 forceatplayer;
    private Rigidbody2D rb;
    private GameObject[] trajectorydots;

    private void Start()
    {
        trajectorydots = new GameObject[numberofdots];
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float rotZ = Mathf.Atan2(-difference.y, -difference.x) * Mathf.Rad2Deg;
            if (rotZ < -12)
                rotZ = -12;
            else if (rotZ > 140)
                rotZ = 140;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }
        

        if (Input.GetMouseButtonDown(0))
        {
            starpos = shotpoint.transform.position;

            for (int i = 0; i < numberofdots; i++)
                trajectorydots[i] = Instantiate(trajectorydot, shotpoint.transform);
            
        }
        if (Input.GetMouseButton(0))
        {
            endpos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
            forceatplayer = endpos - starpos;
            endpos = shotpoint.transform.position;

            for (int i = 0; i < numberofdots; i++)
                trajectorydots[i].transform.position = calculateposition(i*0.1f);
        }
        if (Input.GetMouseButtonUp(0))
        {
            bullet = Instantiate(bulletprefab, shotpoint.position, Quaternion.identity);
            rb = bullet.GetComponent<Rigidbody2D>();
            rb.gravityScale = 1;
            rb.velocity = new Vector2(-forceatplayer.x * force, -forceatplayer.y * force);

            for (int i = 0; i < numberofdots; i++)
                Destroy(trajectorydots[i]);
        }
    }

    private Vector2 calculateposition(float time)
    {
        return new Vector2(endpos.x, endpos.y) + new Vector2(-forceatplayer.x * force, -forceatplayer.y * force) * time + 0.5f * Physics2D.gravity * time * time;
    }
}
