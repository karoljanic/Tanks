using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField]private Rigidbody2D _R;
    [SerializeField]private PhotonView _View;


    void Update()
    {
        if (_View.isMine)
        {
            if (_R.velocity.magnitude > 0.1f)
            {
                transform.LookAt(new Vector3(transform.position.x + _R.velocity.x, transform.position.y + _R.velocity.y, transform.position.z));
            }
        }    
    }
}
