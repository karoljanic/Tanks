using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_control : MonoBehaviour
{
    public Transform target;

    private void FixedUpdate()
    {
        Vector3 newPosition = target.position;
        newPosition.z = -10;
        newPosition.y = -1.2f;

        transform.position = newPosition;
    }
}
