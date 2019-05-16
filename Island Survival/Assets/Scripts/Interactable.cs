using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public readonly new string name = "Null_Interactable";
    public readonly string title = "Interactable";

    public string getTitle()
    {
        return title;
    }

    public string getName()
    {
        return name;
    }

}
