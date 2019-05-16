using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public string Name { get; set; }
    public string Title { get; set; }

    public Interactable()
    {
        Title = "Interactable";
        Name = "Null_Interactable";
    }

}
