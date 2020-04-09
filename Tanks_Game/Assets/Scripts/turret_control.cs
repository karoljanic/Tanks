using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret_control : MonoBehaviour
{

    private void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        if (rotZ < -12)
            rotZ = -12;
        else if (rotZ > 140)
            rotZ = 140;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
    }

}
