using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputButton : MonoBehaviour
{
    [SerializeField]private bool _Left;

    public void PointerDownHandler()
    {
        if (_Left)
        {
            InputController.IsLeftDown = true;
        }
        else
        {
            InputController.IsRightDown = true;
        }
    }

    public void PointerUpHandler()
    {
        if (_Left)
        {
            InputController.IsLeftDown = true;
        }
        else
        {
            InputController.IsRightDown = true;
        }
    }
}
