using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background_color : MonoBehaviour
{
    private Renderer render;

    [SerializeField]
    private Color  colorToTurnTo = Color.green;

    private void Start()
    {
        render = GetComponent<Renderer>();
        render.material.color = colorToTurnTo;
    }



}
