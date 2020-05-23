using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeplayer : MonoBehaviour
{
    public Sprite[] sprites;
    public GameObject img;

    private int i = 0;

    private void OnEnable()
    {
        i = MainMenuFlow.AvatarID;
        img.GetComponent<Image>().sprite = sprites[i];
    }

    public void change()
    {
        //sprites = new Sprite[7];
        i += 1;
        if (i >= sprites.Length)
        {
            i = 0;
        }
        img.GetComponent<Image>().sprite = sprites[i];
        MainMenuFlow.AvatarID = i;
    }
}
