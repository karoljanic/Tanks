using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Nickname : MonoBehaviour
{

    private TouchScreenKeyboard keyboard;
    public string stringToEdit;

    private void OnGUI()
    {
        stringToEdit = GUI.TextField(new Rect(xMax, yMax), stringToEdit, 25);
    }
    //keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    //stringToEdit = GUI.TextField(new Rect(x.Max, 10, 200, 20), stringToEdit, 25);
    

}

