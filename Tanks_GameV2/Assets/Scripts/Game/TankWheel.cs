using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankWheel : MonoBehaviour
{

    private List<Collider2D> _ActiveCollisions;

    public int CollisionsCount { get { return _ActiveCollisions.Count; } }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _ActiveCollisions.Add(collision.collider);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _ActiveCollisions.Remove(collision.collider);
    }

    // Start is called before the first frame update
    void Start()
    {
        _ActiveCollisions = new List<Collider2D>();
    }
}
