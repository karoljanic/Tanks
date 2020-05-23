using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_control : MonoBehaviour
{
    public Transform target;
    public Vector3 Offset;

    private void FixedUpdate()
    {
        if (target != null)
        {
            var finalPos = target.position + Offset;
            transform.position = Vector3.Lerp(
                transform.position,
                finalPos,
                Time.deltaTime * 4.0f
                );
        }
    }
}
