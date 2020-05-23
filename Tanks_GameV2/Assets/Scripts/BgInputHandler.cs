using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgInputHandler : MonoBehaviour
{


    public void PointerDownHandler()
    {
        InputController.IsDragActive = true;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            InputController.IsLeftDown = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            InputController.IsLeftDown = false;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            InputController.IsRightDown = true;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            InputController.IsRightDown = false;
        }


        if (Input.GetMouseButtonUp(0))
        {
            if (InputController.IsDragActive)
            {
                InputController.IsDragActive = false;
                FindObjectOfType<PlayerMovement>().TryShoot();
            }
            if (InputController.IsLeftDown)
            {
                InputController.IsLeftDown = false;
            }
            if (InputController.IsRightDown)
            {
                InputController.IsRightDown = false;
            }
        }
    }
}
