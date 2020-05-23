using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{
    public GameObject effect;
    [SerializeField]private PhotonView _View;

    private T GetComponentOnObjectOrParent<T>(GameObject obj)
    {
        var result = obj.GetComponent<T>();
        if (result == null)
        {
            result = obj.GetComponentInParent<T>();
        }
        return result;
    }

    private void OnDestroy()
    {
        if (effect != null)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
        }
    }

    private void DestroyObject()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            if (_View.isMine)
            {
                DestroyObject();
            }
        }
        else if (collision.gameObject.tag.Equals("Player"))
        {
            if (_View.isMine)
            {
                var player = GetComponentOnObjectOrParent<playerscript>(collision.gameObject);
                if (player != null && !player.View.isMine)
                {
                    player.TakeDamage(20);
                }
                DestroyObject();
            }
        }
        else if (collision.gameObject.tag.Equals("Base"))
        {
            if (_View.isMine)
            {
                var base1 = GetComponentOnObjectOrParent<basescript>(collision.gameObject);
                if (base1 != null && !base1.View.isMine)
                {
                    base1.TakeDamage(20);
                }
                DestroyObject();
            }
        }
    }
}
