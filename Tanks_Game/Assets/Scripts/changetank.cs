using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changetank : MonoBehaviour
{
    public Sprite[]  sprites;
    public GameObject img;

    private int i = 0;

    public void change()
    {
        //sprites = new Sprite[7];
        i += 1;
        i = i % 5;
        img.GetComponent<Image>().sprite = sprites[i];
    }
}
